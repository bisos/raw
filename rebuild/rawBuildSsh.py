# -*- coding: utf-8 -*-

""" #+begin_org
* ~[Summary]~ :: rawBuildSsh --- the =steps= of a box or a guest: an ssh target, reached as the Debian
  install account (=debInstAcct=: intra on a box, vagrant on a guest), on top of =unsitedBisosDeploy.sh=
  (bisos/bsip4). =rawBuildBox.cs -i procBox= runs them; =rawBuildGuest.cs= adds the VM lifecycle around
  them. AI-WorkPlan.org 3.1 (the generic steps) and 3.4.
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
import ast
import pathlib
import posixpath
import shlex
import shutil
import subprocess
import tempfile
import time

from bisos import b
from bisos.b import b_io

import rawBuildVerify


###############################################################################
# SshTarget --- one box or guest, as its Debian install account
#
# Every remote command starts in / : on a box the install account's home is 0700, and a sudo -u
# bisos / bystar child that inherits it cannot read it (bxGenesis/start AI-WorkPlan 3.7).
###############################################################################

def debInstDefaults(debInstAcct=None, debInstAcctPasswd=None, debInstRootPasswd=None):
    """The defaults of unsitedBisosDeploy.sh (debInstDefaults): intra/intra/intra, vagrant/vagrant/NA."""
    acct = debInstAcct or "intra"
    if acct == "intra":
        return acct, debInstAcctPasswd or "intra", debInstRootPasswd or "intra"
    if acct == "vagrant":
        return acct, debInstAcctPasswd or "vagrant", debInstRootPasswd or "NA"
    return acct, debInstAcctPasswd, debInstRootPasswd


class SshTarget:

    def __init__(self, targetName, debInstAcct=None, debInstAcctPasswd=None, debInstRootPasswd=None):
        self.targetName = targetName
        self.acct, self.acctPasswd, self.rootPasswd = debInstDefaults(
            debInstAcct, debInstAcctPasswd, debInstRootPasswd)

    def _sshArgv(self, remoteCmd):
        return ['sshpass', '-p', self.acctPasswd, 'ssh', '-o', 'StrictHostKeyChecking=no',
                '-o', 'ConnectTimeout=10', f"{self.acct}@{self.targetName}", remoteCmd]

    def run(self, script, show=False, timeout=None):
        """Run a bash script as the install account. Returns (returncode, stdout); show=True: to the terminal."""
        argv = self._sshArgv("cd / && bash -c " + shlex.quote(script))
        try:
            if show:
                return subprocess.run(argv, check=False, timeout=timeout).returncode, ""
            run = subprocess.run(argv, capture_output=True, text=True, check=False, timeout=timeout)
            return run.returncode, run.stdout
        except subprocess.TimeoutExpired:
            return 255, ""

    def runAsBystar(self, script, show=False):
        """As bystar, in a forced-interactive shell (bystar's ~/.bashrc sets PATH only for interactive shells)."""
        return self.run("sudo -u bystar -H bash -ic " + shlex.quote("cd ~ && " + script), show=show)

    def fetch(self, remotePath, localPath: pathlib.Path) -> bool:
        """Copy a path of the target to the manager, through sudo tar (a path only bystar can read comes too).
        As docker cp: to localPath when it does not exist, into it when it is a directory."""
        remotePath = remotePath.rstrip('/')
        parent, name = posixpath.split(remotePath)
        dest = localPath / name if localPath.is_dir() else localPath
        with tempfile.TemporaryDirectory(dir=dest.parent) as tmp:
            ssh = subprocess.Popen(self._sshArgv(f"cd / && sudo tar -C {shlex.quote(parent)} -cf - {shlex.quote(name)}"),
                                   stdout=subprocess.PIPE)
            untar = subprocess.run(['tar', '-C', tmp, '-xf', '-'], stdin=ssh.stdout, check=False)
            ssh.stdout.close()
            if ssh.wait() != 0 or untar.returncode != 0 or not (pathlib.Path(tmp) / name).exists():
                return False
            shutil.move(str(pathlib.Path(tmp) / name), str(dest))
        return True

    def deployArgs(self):
        """The -p params of unsitedBisosDeploy.sh for this target."""
        return (f"-p targetName={shlex.quote(self.targetName)} -p debInstAcct={shlex.quote(self.acct)}"
                f" -p debInstAcctPasswd={shlex.quote(self.acctPasswd)}"
                f" -p debInstRootPasswd={shlex.quote(self.rootPasswd)}")


###############################################################################
# The steps. Each is step(cmnd, target, ctx) -> b.op.Outcome, ctx = {'steps': [...], 'start': ...}.
# A step decides from evidence on the target, not from the exit code of what it ran: today a PyCS or
# ICM command exits 0 when it fails (bisos-pip/b Stage 4).
###############################################################################

def _ok(note=None):
    if note:
        print(note)
    return b.op.Outcome().set(opError=b.op.OpError.Success)

def _fail(message):
    """A failed Outcome. The error must be set: b_io.eh.badOutcome only prints, and an Outcome with no
    error is not isProblematic(), so stepsRun went on past a failed step (found 2026-11-27)."""
    b_io.eh.problem_usageError(message)
    return b_io.eh.badOutcome(b.op.Outcome().set(opError=b.op.OpError.Failure, opErrInfo=message))

def _hasBisos(target):
    rc, out = target.run('test -d /bisos && echo present || echo absent')
    return None if rc != 0 else out.strip().endswith('present')

# The steps that run as bystar, so need an installed box (or rawBisosBase before them).
BYSTAR_STEPS = ('rawBisosCBMs', 'verify')

def _hasBystar(target):
    """True when the target has the bystar account; None when ssh failed."""
    rc, out = target.run('id bystar >/dev/null 2>&1 && echo present || echo absent')
    return None if rc != 0 else out.strip().endswith('present')

def _needBystar(target, step):
    """None when bystar exists, else a failed Outcome: the steps that run as bystar need an installed box."""
    has = _hasBystar(target)
    if has is None:
        return _fail(f"{step}: ssh to {target.targetName} failed")
    if not has:
        return _fail(f"{step}: {target.targetName} has no bystar account (BISOS not installed):"
                     " add the rawBisosBase step")
    return None

def runStart(stepsList):
    """How the run starts: fresh (rawBisosBase without deBisosIfy), deBisosIfied, or None (an existing install)."""
    if 'deBisosIfy' in stepsList:
        return 'deBisosIfied'
    if 'rawBisosBase' in stepsList:
        return 'fresh'
    return None


def stepReach(cmnd, target, ctx):
    """ssh works; who, sudo without a password, the Debian version, whether /bisos exists.
    A run that installs without deBisosIfy needs a fresh box (raw-bisos.sh aborts on an existing /bisos);
    a run that does not install but has steps that run as bystar needs an installed one (/bisos and bystar).
    A bare ['reach'] is a probe: it reports and passes either way."""
    rc, out = target.run('echo "acct=$(id -un)"; sudo -n true 2>/dev/null && echo sudo=nopasswd || echo sudo=password;'
                         ' . /etc/os-release; echo "debian=${VERSION_ID}";'
                         ' test -d /bisos && echo bisos=present || echo bisos=absent;'
                         ' id bystar >/dev/null 2>&1 && echo bystar=present || echo bystar=absent')
    if rc != 0:
        return _fail(f"reach: ssh {target.acct}@{target.targetName} failed (rc={rc})")
    facts = dict(line.split('=', 1) for line in out.splitlines() if '=' in line)
    print(f"reach: {target.targetName}: " + ", ".join(f"{k} {v}" for k, v in facts.items()))
    if ctx['start'] == 'fresh' and facts.get('bisos') == 'present':
        return _fail(f"reach: {target.targetName} is not fresh (/bisos exists): add the deBisosIfy step, or reinstall Debian")
    needsInstall = ctx['start'] is None and any(s in BYSTAR_STEPS for s in ctx['steps'])
    if needsInstall and (facts.get('bisos') != 'present' or facts.get('bystar') != 'present'):
        return _fail(f"reach: {target.targetName} has no BISOS install (/bisos or bystar absent):"
                     " add the rawBisosBase step (fresh: reach, rawBisosBase, reboot, rawBisosCBMs, verify)")
    return _ok()

def stepDeBisosIfy(cmnd, target, ctx):
    """unsitedBisosDeploy.sh -i l1_deBisosIfy: delete the BISOS users, groups and trees. Warm, not fresh."""
    b.subProc.WOpW(invedBy=cmnd, log=1).bash(
        f"unsitedBisosDeploy.sh -h -v -n showRun {target.deployArgs()} -i l1_deBisosIfy")
    if _hasBisos(target) is not False:
        return _fail(f"deBisosIfy: /bisos is still on {target.targetName} (or ssh failed)")
    return _ok(f"deBisosIfy: {target.targetName}: /bisos removed")

def stepRawBisosBase(cmnd, target, ctx):
    """unsitedBisosDeploy.sh -i l1_raw_bisos: fetch and run raw-bisos.sh -i installUnsitedBisos on the target."""
    b.subProc.WOpW(invedBy=cmnd, log=1).bash(
        f"unsitedBisosDeploy.sh -h -v -n showRun {target.deployArgs()} -i l1_raw_bisos")
    rc, out = target.run('test -d /bisos/core/bsip/bin && id bystar >/dev/null 2>&1 && echo installed || echo missing')
    if rc != 0 or not out.strip().endswith('installed'):
        return _fail(f"rawBisosBase: {target.targetName} has no /bisos/core/bsip/bin or no bystar")
    return _ok(f"rawBisosBase: {target.targetName}: installed")

def stepReboot(cmnd, target, ctx, waitSeconds=900):
    """Reboot and wait for ssh, so the groups raw-bisos.sh created and its services start as on a real boot."""
    rc, before = target.run('cat /proc/sys/kernel/random/boot_id')
    if rc != 0:
        return _fail(f"reboot: ssh to {target.targetName} failed")
    target.run('sudo systemctl reboot', timeout=30)
    print(f"reboot: {target.targetName}: waiting for it to come back (up to {waitSeconds}s)")
    deadline = time.time() + waitSeconds
    time.sleep(20)
    while time.time() < deadline:
        rc, after = target.run('cat /proc/sys/kernel/random/boot_id', timeout=30)
        if rc == 0 and after.strip() and after.strip() != before.strip():
            return _ok(f"reboot: {target.targetName}: back up")
        time.sleep(10)
    return _fail(f"reboot: {target.targetName} did not come back in {waitSeconds}s")

def stepRawBisosCBMs(cmnd, target, ctx):
    """As bystar: sysCbmFullyMaterialize.sh (/bisos/core/raw/bin; sysCbmManage.cs -i fullyMaterialize, logged)."""
    if (failed := _needBystar(target, 'rawBisosCBMs')) is not None:
        return failed
    rc, _ = target.runAsBystar('sysCbmFullyMaterialize.sh', show=True)
    if rc == 127:
        return _fail(f"rawBisosCBMs: sysCbmFullyMaterialize.sh not found on {target.targetName} (is bisos/raw current?)")
    return _ok(f"rawBisosCBMs: {target.targetName}: exited {rc} (not trusted: verify decides)")

def stepVerify(cmnd, target, ctx):
    """rawBuildVerify.collect over ssh. The install account's raw-bisos log is copied to bystar's home first,
    so that fullyVerify scans it: bystar cannot read the install account's 0700 home."""
    if (failed := _needBystar(target, 'verify')) is not None:
        return failed
    target.run('f=$(ls -1t ~/raw-bisos-*-log.org 2>/dev/null | head -1);'
               ' [ -n "$f" ] && sudo install -o bystar -m 0644 "$f" ~bystar/')
    return rawBuildVerify.collect(target.targetName, target.runAsBystar, target.fetch,
                                  start=ctx['start'], steps=ctx['steps'])


# Ordered registry of the ssh steps; rawBuildGuest.cs adds create / destroy to its own copy.
sshStepTable = {
    'reach':        stepReach,
    'deBisosIfy':   stepDeBisosIfy,
    'rawBisosBase': stepRawBisosBase,
    'reboot':       stepReboot,
    'rawBisosCBMs': stepRawBisosCBMs,
    'verify':       stepVerify,
}


def stepsListParse(steps, stepTable):
    """--steps (a Python-literal list of step names) to a list, or None after a usage error."""
    if steps is None:
        b_io.eh.problem_usageError(f"--steps is required, e.g. --steps='[\"reach\"]'; valid: {list(stepTable)}")
        return None
    try:
        stepsList = ast.literal_eval(steps) if isinstance(steps, str) else steps
    except (ValueError, SyntaxError):
        b_io.eh.problem_usageError(f"--steps must be a Python-literal list; got: {steps!r}")
        return None
    if not isinstance(stepsList, list) or not all(isinstance(s, str) for s in stepsList):
        b_io.eh.problem_usageError(f"--steps must be a list of strings; got: {stepsList!r}")
        return None
    unknown = [s for s in stepsList if s not in stepTable]
    if unknown:
        b_io.eh.problem_usageError(f"unknown step(s) {unknown}; valid: {list(stepTable)}")
        return None
    return stepsList


def stepsRun(cmnd, target, stepsList, stepTable) -> bool:
    """Run the steps serially, fail-fast. True when all passed."""
    ctx = {'steps': stepsList, 'start': runStart(stepsList)}
    print(f"# {target.targetName} ({target.acct}) --- steps: {' -> '.join(stepsList)}"
          + (f" --- start: {ctx['start']}" if ctx['start'] else ""))
    for step in stepsList:
        if stepTable[step](cmnd, target, ctx).isProblematic():
            b_io.eh.problem_usageError(f"step failed: {step}")
            return False
    return True
