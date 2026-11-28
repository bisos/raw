# -*- coding: utf-8 -*-

""" #+begin_org
* ~[Summary]~ :: rawBuildVerify --- the =verify= step of every rawBuild platform (container, box, guest):
  run =sysCbmManage.cs -i fullyVerify= on the target, collect its report and logs to the manager, compare
  with the previous run. The transport is the caller's: =docker exec= / =docker cp= for a container,
  ssh / tar for a box or a guest. AI-WorkPlan.org 3.1.
#+end_org """

####+BEGIN: b:py3:cs:file/dblockControls :classification "cs-u"
""" #+begin_org
* [[elisp:(org-cycle)][| /Control Parameters Of This File/ |]] :: dblk ctrls classifications=cs-u
#+end_org """
####+END:

####+BEGIN: b:prog:file/proclamations :outLevel 1
""" #+begin_org
* *[[elisp:(org-cycle)][| Proclamations |]]* :: Libre-Halaal Software --- Part Of BISOS ---  Poly-COMEEGA Format.
** This is Libre-Halaal Software. © Neda Communications, Inc. Subject to AGPL.
** It is part of BISOS (ByStar Internet Services OS)
#+end_org """
####+END:

import typing
import datetime
import json
import pathlib
import re

from bisos import b
from bisos.b import b_io


###############################################################################
# Where each run's results are kept, on the manager (the host that runs rawBuild*.cs):
#   <resultsBase>/<target>/<yymmddHHMMSS>/
#       verify/report.json, verify/report.org     fullyVerify's report, copied from the target
#       *-log.org                                  the target's newest raw-bisos and sysCbmFullyMaterialize logs
#       run.json                                   how the run started (fresh, deBisosIfied) and its steps
#       compare.txt                                against the previous run of the same start and steps
###############################################################################

resultsBase = pathlib.Path("/bisos/var/rawBuild")

# A transport runs a bash script on the target as bystar, in a forced-interactive shell (bystar's
# ~/.bashrc sets PATH only for interactive shells), and returns (returncode, stdout). With show=True
# the output goes to the terminal instead (stdout is then "").
RunAsBystar = typing.Callable[[str, bool], typing.Tuple[int, str]]
# ... and copies a path of the target to a path of the manager (a directory to the given name, as
# `docker cp` and `scp -r` do when it does not exist; a file into it when it is a directory).
Fetch = typing.Callable[[str, pathlib.Path], bool]

LOGS_SCRIPT = 'for p in raw-bisos sysCbmFullyMaterialize; do ls -1t ~/$p-*-log.org 2>/dev/null | head -1; done'


def _runKind(runDir: pathlib.Path) -> tuple:
    """(start, steps) of a run, from its run.json; (None, None) without one (a container's, or older)."""
    try:
        run = json.loads((runDir / 'run.json').read_text())
    except (OSError, ValueError):
        return (None, None)
    return (run.get('start'), run.get('steps'))


def collect(
        target: str,
        runAsBystar: RunAsBystar,
        fetch: Fetch,
        start: typing.Optional[str] = None,
        steps: typing.Optional[list] = None,
) -> b.op.Outcome:
    """Is the built platform healthy? bisos.capability AI-WorkPlan.org 5.5.

    1. Runs =sysCbmManage.cs -i fullyVerify= on the target (read-only; it also scans the target's
       ~/raw-bisos-*-log.org and ~/sysCbmFullyMaterialize-*-log.org).
    2. Copies its report and those two logs to <resultsBase>/<target>/<yymmddHHMMSS>/.
    3. Compares the report with the previous run there of the same =start= and the same =steps= (a
       warm deBisosIfied run is not compared with a fresh one, nor a base-only run with one that also
       materialized the CBMs): new failures, fixed, exit codes changed --- the measurement of
       bisos-pip/b Stage 4 (capability 5.6).
    4. endNotify: prints the summary (no channel yet).

    Fails only when no report could be collected. A report whose verdict is fail is a result,
    printed, not a failure: the verdicts come from evidence, not exit codes.
    """
    outcome = b.op.Outcome()

    rc, _ = runAsBystar('sysCbmManage.cs -i fullyVerify', True)
    print(f"verify: fullyVerify on {target} exited {rc} (not trusted: the report decides)")

    # latest points at the bare run tag. Not readlink -f: /bisos/var is a symlink to /var/bisos, so the
    # resolved path would not start with /bisos/var (found on the first container run, 2026-11-26).
    _, out = runAsBystar('readlink /bisos/var/cbm/verify/latest', False)
    lines = out.strip().splitlines()
    runTag = lines[-1].strip() if lines else ""
    if not re.fullmatch(r"\d{12}", runTag):
        b_io.eh.problem_usageError(f"verify: no report on {target} (/bisos/var/cbm/verify/latest)")
        return b_io.eh.badOutcome(outcome)
    reportDir = f"/bisos/var/cbm/verify/{runTag}"

    tag = datetime.datetime.now().strftime("%y%m%d%H%M%S")
    targetBase = resultsBase / target
    runDir = targetBase / tag
    runDir.mkdir(parents=True, exist_ok=True)
    if not fetch(reportDir, runDir / 'verify') or not (runDir / 'verify' / 'report.json').is_file():
        b_io.eh.problem_usageError(f"verify: could not copy {reportDir} from {target}")
        return b_io.eh.badOutcome(outcome)
    (runDir / 'run.json').write_text(json.dumps({'target': target, 'start': start, 'steps': steps}, indent=2) + "\n")

    _, logs = runAsBystar(LOGS_SCRIPT, False)
    for log in [line.strip() for line in logs.splitlines() if line.strip().endswith('-log.org')]:
        fetch(log, runDir)

    report = json.loads((runDir / 'verify' / 'report.json').read_text())
    counts = ", ".join(f"{k} {v}" for k, v in sorted(report['counts'].items()))
    masked = report.get('maskedFailures') or []

    previous = sorted(d for d in targetBase.iterdir()
                      if d.is_dir() and d.name < tag and (d / 'verify' / 'report.json').is_file()
                      and _runKind(d) == (start, steps))
    if previous:
        from bisos.capability import cbmVerify
        old = json.loads((previous[-1] / 'verify' / 'report.json').read_text())
        compareStr = cbmVerify.compareText(old, report, cbmVerify.compare(old, report))
        (runDir / 'compare.txt').write_text(compareStr + "\n")
        print(compareStr)
    else:
        print(f"verify: first run under {targetBase}" + (f" that started {start}" if start else "")
              + (f" with steps {steps}" if steps else "") + ": nothing to compare with")

    # endNotify: no channel is defined yet; the summary is printed.
    print(f"verify: {target}" + (f" ({start})" if start else "") + f": {report['verdict']} --- leaves {counts}; "
          f"platform problems {report['platformProblems']}; log hits {len(report.get('logHits', []))}"
          + (f"; masked: {', '.join(masked)}" if masked else ""))
    print(f"verify: results in {runDir}")
    return outcome.set(opError=b.op.OpError.Success, opResults=str(runDir))
