#!/usr/bin/env python
# -*- coding: utf-8 -*-

""" #+begin_org
* ~[Summary]~ :: A =CmndSvc= for rebuilding Raw-BISOS Docker/Podman containers. See ./README.org.
#+end_org """

####+BEGIN: b:py3:cs:file/dblockControls :classification "cs-mu"
""" #+begin_org
* [[elisp:(org-cycle)][| /Control Parameters Of This File/ |]] :: dblk ctrls classifications=cs-mu
#+BEGIN_SRC emacs-lisp
(setq-local b:dblockControls t) ; (setq-local b:dblockControls nil)
(put 'b:dblockControls 'py3:cs:Classification "cs-mu") ; one of cs-mu, cs-u, cs-lib, bpf-lib, pyLibPure
#+END_SRC
#+RESULTS:
: cs-mu
#+end_org """
####+END:

####+BEGIN: b:prog:file/proclamations :outLevel 1
""" #+begin_org
* *[[elisp:(org-cycle)][| Proclamations |]]* :: Libre-Halaal Software --- Part Of BISOS ---  Poly-COMEEGA Format.
** This is Libre-Halaal Software. © Neda Communications, Inc. Subject to AGPL.
** It is part of BISOS (ByStar Internet Services OS)
** Best read and edited  with Blee in Poly-COMEEGA (Polymode Colaborative Org-Mode Enhance Emacs Generalized Authorship)
#+end_org """
####+END:

####+BEGIN: b:prog:file/particulars :authors ("./inserts/authors-mb.org")
""" #+begin_org
* *[[elisp:(org-cycle)][| Particulars |]]* :: Authors, version
** This File: /l/pip/siteRegistrars/py3/bin/siteRegistrars-assemble.cs
** Authors: Mohsen BANAN, http://mohsen.banan.1.byname.net/contact
#+end_org """
####+END:

####+BEGIN: b:py3:file/particulars-csInfo :status "inUse"
""" #+begin_org
* *[[elisp:(org-cycle)][| Particulars-csInfo |]]*
#+end_org """
import typing
csInfo: typing.Dict[str, typing.Any] = { 'moduleName': ['rawBuildContainer'], }
csInfo['version'] = '202409222627'
csInfo['status']  = 'inUse'
csInfo['panel'] = 'rawBuildContainer-Panel.org'
csInfo['groupingType'] = 'IcmGroupingType-pkged'
csInfo['cmndParts'] = 'IcmCmndParts[common] IcmCmndParts[param]'
####+END:

""" #+begin_org
* [[elisp:(org-cycle)][| ~Description~ |]] :: [[file:/bisos/git/auth/bxRepos/blee-binders/bisos-core/PyFwrk/bisos-pip/bisos.cs/_nodeBase_/fullUsagePanel-en.org][BISOS CmndSvcs Panel]]   [[elisp:(org-cycle)][| ]]

Top-level orchestrator for rebuilding Raw-BISOS Docker/Podman containers from
Fresh-Debian. This is a thin layer on top of the per-leaf planted command
services (=dockerProc.spcs= / =podmanProc.spcs=) under =bxObjects/bro_dockerfiles=.

See ./README.org for the bigger picture (Boxes, Guests and Containers).

** Status: In use with BISOS
#+end_org """

####+BEGIN: b:prog:file/orgTopControls :outLevel 1
""" #+begin_org
* [[elisp:(org-cycle)][| Controls |]] :: [[elisp:(delete-other-windows)][(1)]] | [[elisp:(show-all)][Show-All]]  [[elisp:(org-shifttab)][Overview]]  [[elisp:(progn (org-shifttab) (org-content))][Content]] | [[file:Panel.org][Panel]] | [[elisp:(blee:ppmm:org-mode-toggle)][Nat]] | [[elisp:(bx:org:run-me)][Run]] | [[elisp:(bx:org:run-me-eml)][RunEml]] | [[elisp:(progn (save-buffer) (kill-buffer))][S&Q]]  [[elisp:(save-buffer)][Save]]  [[elisp:(kill-buffer)][Quit]] [[elisp:(org-cycle)][| ]]
** /Version Control/ ::  [[elisp:(call-interactively (quote cvs-update))][cvs-update]]  [[elisp:(vc-update)][vc-update]] | [[elisp:(bx:org:agenda:this-file-otherWin)][Agenda-List]]  [[elisp:(bx:org:todo:this-file-otherWin)][ToDo-List]]

#+end_org """
####+END:

####+BEGIN: b:py3:file/workbench :outLevel 1
""" #+begin_org
* [[elisp:(org-cycle)][| Workbench |]] :: [[elisp:(python-check (format "/bisos/venv/py3/bisos3/bin/python -m pyclbr %s" (bx:buf-fname))))][pyclbr]] || [[elisp:(python-check (format "/bisos/venv/py3/bisos3/bin/python -m pydoc ./%s" (bx:buf-fname))))][pydoc]] || [[elisp:(python-check (format "/bisos/pipx/bin/pyflakes %s" (bx:buf-fname)))][pyflakes]] | [[elisp:(python-check (format "/bisos/pipx/bin/pychecker %s" (bx:buf-fname))))][pychecker (executes)]] | [[elisp:(python-check (format "/bisos/pipx/bin/pycodestyle %s" (bx:buf-fname))))][pycodestyle]] | [[elisp:(python-check (format "/bisos/pipx/bin/flake8 %s" (bx:buf-fname))))][flake8]] | [[elisp:(python-check (format "/bisos/pipx/bin/pylint %s" (bx:buf-fname))))][pylint]]  [[elisp:(org-cycle)][| ]]
#+end_org """
####+END:

####+BEGIN: b:py3:cs:framework/imports :basedOn "classification"
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CsFrmWrk   [[elisp:(outline-show-subtree+toggle)][||]] *Imports* =Based on Classification=cs-mu=
#+end_org """
from bisos import b
from bisos.b import cs
from bisos.b import b_io
from bisos.common import csParam

import collections
import pathlib
####+END:

""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CsFrmWrk   [[elisp:(outline-show-subtree+toggle)][||]] ~csuList emacs-list Specifications~  [[elisp:(blee:org:code-block/above-run)][ /Eval Below/ ]] [[elisp:(org-cycle)][| ]]
#+BEGIN_SRC emacs-lisp
(setq  b:py:cs:csuList
  (list
   "bisos.csPlayer.bleep"
   "rawBuild_csu"
 ))
#+END_SRC
#+RESULTS:
| bisos.csPlayer.bleep | rawBuild_csu |
#+end_org """

####+BEGIN: b:py3:cs:framework/csuListProc :pyImports t :csuImports t :csuParams t :csmuParams nil
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CsFrmWrk   [[elisp:(outline-show-subtree+toggle)][||]] ~Process CSU List~ with /2/ in csuList pyImports=t csuImports=t csuParams=t
#+end_org """

from bisos.csPlayer import bleep
import rawBuild_csu

csuList = [ 'bisos.csPlayer.bleep', 'rawBuild_csu', ]

g_importedCmndsModules = cs.csuList_importedModules(csuList)

def g_extraParams():
    csParams = cs.param.CmndParamDict()
    cs.csuList_commonParamsSpecify(csuList, csParams)
    cs.argsparseBasedOnCsParams(csParams)

####+END:

####+BEGIN: b:py3:cs:main/exposedSymbols :classes ()
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CsFrmWrk   [[elisp:(outline-show-subtree+toggle)][||]] ~CS Controls and Exposed Symbols List Specification~ with /0/ in Classes List
#+end_org """
####+END:

cs.invOutcomeReportControl(cmnd=True, ro=True)

####+BEGIN: blee:bxPanel:foldingSection :outLevel 0 :sep nil :title "CmndSvcs" :anchor ""  :extraInfo "Command Services Section"
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*     [[elisp:(outline-show-subtree+toggle)][| _CmndSvcs_: |]]  Command Services Section  [[elisp:(org-shifttab)][<)]] E|
#+end_org """
####+END:

def rawBuild_connectInfoShow(self, platformModel, instancePath):
    """Print clickable ssh/vnc/noVNC URLs (this host's IP + the leaf's published ports).

    Ports are read live from the engine (=<engine> port <name>=) rather than
    hardcoded; the host IP is =hostname -I='s first address.
    """
    engine = 'podman' if platformModel == 'sysd-podman' else 'docker'
    imageName = instancePath.name

    # b.subProc WOpW reuses one outcome object across calls, so capture each
    # command's stdout into a string immediately, before issuing the next.
    portsStr = (b.subProc.WOpW(invedBy=self, log=0).bash(f"{engine} port {imageName}").stdout or "")
    hostStr = (b.subProc.WOpW(invedBy=self, log=0).bash("hostname -I").stdout or "")

    hostToks = hostStr.split()
    ipAddr = hostToks[0] if hostToks else "localhost"

    # Map container-side port -> host published port, e.g. "22/tcp -> 0.0.0.0:2224".
    portMap = {}
    for line in portsStr.splitlines():
        if '->' not in line:
            continue
        cSide, hSide = line.split('->')
        cPort = cSide.strip().split('/')[0]
        hPort = hSide.strip().rsplit(':', 1)[-1]
        portMap[cPort] = hPort

    novnc, vnc, ssh = portMap.get('6901'), portMap.get('5901'), portMap.get('22')

    if not (novnc or vnc or ssh):
        print(f"# {imageName} is not running --- no connection info.")
        return

    # Each line is a runnable CLI command (RET in a shell) with the URL kept as a
    # trailing comment. noVNC uses visitUrl (to be defined) in front of the URL.
    print(f"# Connect to {imageName} (go to a line and hit RET):")
    if vnc:   print(f"vncviewer {ipAddr}:{vnc} # vnc://{ipAddr}:{vnc}")
    if ssh:   print(f"ssh -p {ssh} bystar@{ipAddr} # ssh://bystar@{ipAddr}:{ssh}")
    if novnc: print(f"visitUrl http://{ipAddr}:{novnc}/")

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "examples" :extent "verify" :ro "noCli" :comment "FrameWrk: CS-Main-Examples" :parsMand "" :parsOpt "" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<examples>>  *FrameWrk: CS-Main-Examples*  =verify= ro=noCli   [[elisp:(org-cycle)][| ]]
#+end_org """
class examples(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}
    rtInvConstraints = cs.rtInvoker.RtInvoker.new_noRo() # NO RO From CLI

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
    ) -> b.op.Outcome:
        """FrameWrk: CS-Main-Examples"""
        failed = b_io.eh.badOutcome
        callParamsDict = {}
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
####+END:
        self.cmndDocStr(f""" #+begin_org
***** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Conventional top level example.
        #+end_org """)

        cs.examples.myName(cs.G.icmMyName(), cs.G.icmMyFullName())
        cs.examples.commonBrief()

        od = collections.OrderedDict
        cmnd = cs.examples.cmndEnter
        literal = cs.examples.execInsert

        # Current containers --- run docker + podman ps -a up front so you can see
        # what is up just by running the examples menu.
        cs.examples.menuChapter('=Current Containers=')
        cmnd('allContainersPs', comment=" # docker + podman ps -a")
        allContainersPs(cmndOutcome=cmndOutcome).pyCmnd()

        # Image layer --- kept out of the main workflow. A single entry opens a
        # dedicated image-construction menu; image builds are infrequent.
        cs.examples.menuChapter('=Build rawBisos Images=')
        cmnd('buildImageExamples', comment=" # dedicated image-construction examples menu")

        # Instance layer --- one section per leaf: the common procContainer commands
        # inline (up, build), plus a drill-down to the fuller per-leaf example menu.
        for freshDeb in ('12', '13'):
            for platformModel in ('confined', 'sysd-priv', 'sysd-podman'):
                leafPars = od(freshDeb=freshDeb, platformModel=platformModel)
                cs.examples.menuChapter(f'=Build rawBisos -- deb{freshDeb} {platformModel}=')
                cmnd('procContainer',
                     pars=od(**leafPars, steps=['up', 'verifyUp']),
                     comment=" # up and verify")
                cmnd('procContainer',
                     pars=od(**leafPars, steps=['up', 'rawBisosBase']),
                     comment=" # build rawBisos (up + install)")
                cmnd('procContainer',
                     pars=od(**leafPars, steps=['delete', 'up']),
                     comment=" # recreate: delete + up")
                cmnd('procContainer',
                     pars=od(**leafPars, steps=['delete', 'up', 'rawBisosBase']),
                     comment=" # recreate + build: delete + up + install")
                cmnd('procContainer',
                     pars=od(**leafPars, steps=['delete', 'up', 'rawBisosBase', 'down', 'up', 'rawBisosCBMs']),
                     comment=" # full: recreate + install + reconnect (groups) + CBMs")
                cmnd('procContainerExamples',
                     pars=leafPars,
                     comment=" # more examples (verify, recreate, delete, status)")


        cs.examples.menuChapter('=Container Path Obtain=')
        cmnd('containerPathObtain',
             pars=od(freshDeb='12', platformModel='confined', instance='0'),
             comment=" # return path -- deb12 confined")
        cmnd('containerPathObtain',
             pars=od(freshDeb='13', platformModel='sysd-priv', instance='0'),
             comment=" # return path -- deb13 sysd-priv")
        cmnd('containerPathObtain',
             pars=od(freshDeb='13', platformModel='sysd-podman', instance='0'),
             comment=" # return path -- deb13 sysd-podman")


        cs.examples.menuChapter('=Full Update=')
        cmnd('fullUpdate', comment=" # inotifyUserMax")

        cs.examples.menuChapter(f'*Current Settings*')
        literal(f"echo as an example")


        return(cmndOutcome)


####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "buildImageExamples" :extent "verify" :ro "noCli" :comment "Image-construction examples sub-menu" :parsMand "" :parsOpt "" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<buildImageExamples>>  *Image-construction examples sub-menu*  =verify= ro=noCli   [[elisp:(org-cycle)][| ]]
#+end_org """
class buildImageExamples(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}
    rtInvConstraints = cs.rtInvoker.RtInvoker.new_noRo() # NO RO From CLI

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
    ) -> b.op.Outcome:
        """Image-construction examples sub-menu"""
        failed = b_io.eh.badOutcome
        callParamsDict = {}
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
####+END:
        self.cmndDocStr(f""" #+begin_org
***** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Dedicated image-construction menu, out of the main workflow.
        #+end_org """)

        cs.examples.myName(cs.G.icmMyName(), cs.G.icmMyFullName())
        cs.examples.commonBrief()

        od = collections.OrderedDict
        cmnd = cs.examples.cmndEnter

        # Only buildImage_rawBisos entries --- one section per leaf, plain and --noCache.
        for freshDeb in ('12', '13'):
            for platformModel in ('confined', 'sysd-priv', 'sysd-podman'):
                buildPars = od(freshDeb=freshDeb, platformModel=platformModel)
                cs.examples.menuChapter(f'=Build Image -- deb{freshDeb} {platformModel}=')
                cmnd('buildImage_rawBisos',
                     pars=buildPars,
                     comment=" # build image")
                cmnd('buildImage_rawBisos',
                     pars=od(**buildPars, noCache=True),
                     comment=" # build image -- no cache")

        return(cmndOutcome)


def _rawBuild_stepUp(self, proc, instancePath, imageName):
    """Ensure the instance is up --- bring it up only if not already running."""
    psOutcome = b.subProc.WOpW(invedBy=self, log=1).bash(
        f"cd {instancePath} && ./{proc} -i containerProc_instancePs")
    if psOutcome.isProblematic():
        return psOutcome
    isUp = any(imageName in line and "Up" in line
               for line in (psOutcome.stdout or "").splitlines())
    if isUp:
        return psOutcome
    return b.subProc.WOpW(invedBy=self, log=1).bash(
        f"cd {instancePath} && ./{proc} -i containerProc_instanceUp")

def _rawBuild_stepDown(self, proc, instancePath, imageName):
    """Stop (but do not remove) the instance --- e.g. so groups created by
    rawBisosBase take effect on the next 'up'.

    Uses the spcs =containerProc_instanceDown= verb (bisos.dockerProc). This
    used to incorrectly run 'docker compose down' (stop+remove) on the docker
    engine, contradicting its own docstring and the podman branch's plain
    'stop'; that bug has been fixed upstream in
    bxRepos/bisos-pip/dockerProc/py3/bisos/dockerProc/containerProc_csu.py to
    use 'docker compose stop' instead, matching the intended vocabulary
    (up/down/delete) where 'down' never removes.
    """
    return b.subProc.WOpW(invedBy=self, log=1).bash(
        f"cd {instancePath} && ./{proc} -i containerProc_instanceDown")

def _rawBuild_stepDelete(self, proc, instancePath, imageName):
    """Stop and remove the instance (replaces the old force flag)."""
    return b.subProc.WOpW(invedBy=self, log=1).bash(
        f"cd {instancePath} && ./{proc} -i containerProc_instanceDelete")

def _rawBuild_stepRawBisosBase(self, proc, instancePath, imageName):
    """Run ~/raw-bisos/installRawBisos.sh inside the running container."""
    return b.subProc.WOpW(invedBy=self, log=1).bash(
        f"cd {instancePath} && ./{proc} -i containerProc_exec_installRawBisos")

def _rawBuild_stepRawBisosCBMs(self, proc, instancePath, imageName):
    """Run sysCbmManage.cs -i fullyMaterialize inside the running container, as
    the bystar user.

    PATH gotcha: bystar's ~/.bashrc has the standard Debian guard
    ('case $- in *i*) ;; *) return;; esac') that skips the rest of .bashrc
    (where PIPX_BIN_DIR/other PATH exports live, incl. nvm) for
    NON-interactive shells. 'bash -l -c ...' (login, non-interactive) sources
    ~/.profile -> ~/.bashrc, but the guard fires immediately, so PATH ends up
    minimal and 'sysCbmManage.cs' is not found. Forcing an interactive shell
    with 'bash -i -c ...' makes the guard pass and PATH gets populated (verified
    live: resolves to /bisos/venv/py3/bisos3/bin/sysCbmManage.cs). '-i' without
    a tty prints harmless "cannot set terminal process group" / "no job
    control" warnings to stderr; exit status is unaffected.
    """
    engine = 'podman' if 'podmanProc' in proc else 'docker'
    return b.subProc.WOpW(invedBy=self, log=1).bash(
        f"{engine} exec -u bystar {imageName} bash -ic 'sysCbmManage.cs -i fullyMaterialize'")

def _rawBuild_stepVerifyUp(self, proc, instancePath, imageName):
    """Verify the running instance (ports, noVNC HTTP, SSH/systemd)."""
    return b.subProc.WOpW(invedBy=self, log=1).bash(
        f"cd {instancePath} && ./{proc} -i containerProc_instanceVerify")

# Ordered registry of known steps; anything else is a hard error. Add cap*/verify*
# handlers here as they are implemented.
rawBuild_stepTable = {
    'delete':       _rawBuild_stepDelete,
    'up':           _rawBuild_stepUp,
    'down':         _rawBuild_stepDown,
    'rawBisosBase': _rawBuild_stepRawBisosBase,
    'rawBisosCBMs': _rawBuild_stepRawBisosCBMs,
    'verifyUp':     _rawBuild_stepVerifyUp,
}

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "procContainer" :comment "" :extent "verify" :ro "cli" :parsMand "" :parsOpt "freshDeb platformModel instance steps" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<procContainer>>  =verify= parsOpt=freshDeb platformModel instance steps ro=cli   [[elisp:(org-cycle)][| ]]
#+end_org """
class procContainer(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ 'freshDeb', 'platformModel', 'instance', 'steps', ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
             freshDeb: typing.Optional[str]=None,  # Cs Optional Param
             platformModel: typing.Optional[str]=None,  # Cs Optional Param
             instance: typing.Optional[str]=None,  # Cs Optional Param
             steps: typing.Optional[str]=None,  # Cs Optional Param
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {'freshDeb': freshDeb, 'platformModel': platformModel, 'instance': instance, 'steps': steps, }
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
        freshDeb = csParam.mappedValue('freshDeb', freshDeb)
        platformModel = csParam.mappedValue('platformModel', platformModel)
        instance = csParam.mappedValue('instance', instance)
        steps = csParam.mappedValue('steps', steps)
####+END:
        if self.cmndDocStr(""" #+begin_org
** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Process a container leaf through a serial list of =steps=.

        =steps= is a Python-literal list, run in order, fail-fast. Known steps:
        =delete= (stop+rm), =up= (bring up if down), =down= (stop, without
        removing --- so groups created by =rawBisosBase= take effect on the next
        =up=), =rawBisosBase= (run installRawBisos.sh inside), =rawBisosCBMs=
        (run sysCbmManage.cs -i fullyMaterialize as bystar), =verifyUp=
        (containerProc_instanceVerify).
        Connect URLs are always shown at the end. Examples:
          -i procContainer --steps='["up", "verifyUp"]'
          -i procContainer --steps='["delete", "up", "rawBisosBase", "down", "up", "rawBisosCBMs"]'
        #+end_org """): return(cmndOutcome)

        self.captureRunStr(""" #+begin_org
#+begin_src sh :results output :session shared
  rawBuildContainer.cs -i procContainer --freshDeb=13 --platformModel=sysd-priv --steps='["up", "verifyUp"]'
#+end_src
#+RESULTS:
        #+end_org """)

        import ast

        if steps is None:
            b_io.eh.problem_usageError(
                "procContainer requires --steps, e.g. --steps='[\"up\", \"verifyUp\"]'")
            return failed(cmndOutcome)
        try:
            stepsList = ast.literal_eval(steps) if isinstance(steps, str) else steps
        except (ValueError, SyntaxError):
            b_io.eh.problem_usageError(f"--steps must be a Python-literal list; got: {steps!r}")
            return failed(cmndOutcome)
        if not isinstance(stepsList, list) or not all(isinstance(s, str) for s in stepsList):
            b_io.eh.problem_usageError(f"--steps must be a list of strings; got: {stepsList!r}")
            return failed(cmndOutcome)
        unknown = [s for s in stepsList if s not in rawBuild_stepTable]
        if unknown:
            b_io.eh.problem_usageError(
                f"unknown step(s) {unknown}; valid: {sorted(rawBuild_stepTable)}")
            return failed(cmndOutcome)

        # instance is not part of the leaf path; pass a placeholder when unset.
        if not (results := containerPathObtain(cmndOutcome=cmndOutcome).pyCmnd(
                freshDeb=freshDeb,
                platformModel=platformModel,
                instance=(instance or '0'),
        ).results): return(b_io.eh.badOutcome(cmndOutcome))

        instancePath = results
        proc = 'podmanProc.spcs' if platformModel == 'sysd-podman' else 'dockerProc.spcs'
        imageName = instancePath.name

        print(f"# procContainer {imageName} --- steps: {' -> '.join(stepsList)}")

        # Run the steps serially, fail-fast.
        for step in stepsList:
            if rawBuild_stepTable[step](self, proc, instancePath, imageName).isProblematic():
                b_io.eh.problem_usageError(f"step failed: {step}")
                return failed(cmndOutcome)

        # Connect info is always shown at the end.
        rawBuild_connectInfoShow(self, platformModel, instancePath)

        return cmndOutcome

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "procContainerExamples" :extent "verify" :ro "noCli" :comment "Per-leaf procContainer examples sub-menu" :parsMand "" :parsOpt "freshDeb platformModel" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<procContainerExamples>>  *Per-leaf procContainer examples sub-menu*  =verify= parsOpt=freshDeb platformModel ro=noCli   [[elisp:(org-cycle)][| ]]
#+end_org """
class procContainerExamples(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ 'freshDeb', 'platformModel', ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}
    rtInvConstraints = cs.rtInvoker.RtInvoker.new_noRo() # NO RO From CLI

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
             freshDeb: typing.Optional[str]=None,  # Cs Optional Param
             platformModel: typing.Optional[str]=None,  # Cs Optional Param
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {'freshDeb': freshDeb, 'platformModel': platformModel, }
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
        freshDeb = csParam.mappedValue('freshDeb', freshDeb)
        platformModel = csParam.mappedValue('platformModel', platformModel)
####+END:
        self.cmndDocStr(""" #+begin_org
***** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  procContainer step recipes for one specific leaf.
        #+end_org """)

        od = collections.OrderedDict
        cmnd = cs.examples.cmndEnter
        leafPars = od(freshDeb=freshDeb, platformModel=platformModel)

        cs.examples.menuChapter(f'=procContainer --- deb{freshDeb} {platformModel}=')
        cmnd('containerPs',
             pars=leafPars,
             comment=" # status + connect lines")
        cmnd('procContainer',
             pars=od(**leafPars, steps=['up', 'verifyUp']),
             comment=" # up + verify (no install)")
        cmnd('procContainer',
             pars=od(**leafPars, steps=['delete', 'up', 'verifyUp']),
             comment=" # recreate + verify")
        cmnd('procContainer',
             pars=od(**leafPars, steps=['up', 'rawBisosBase', 'verifyUp']),
             comment=" # build rawBisos")
        cmnd('procContainer',
             pars=od(**leafPars, steps=['delete', 'up', 'rawBisosBase', 'verifyUp']),
             comment=" # full rebuild (recreate + install)")
        cmnd('procContainer',
             pars=od(**leafPars, steps=['delete', 'up', 'rawBisosBase', 'down', 'up', 'rawBisosCBMs']),
             comment=" # full: recreate + install + reconnect (groups) + CBMs")
        cmnd('procContainer',
             pars=od(**leafPars, steps=['delete']),
             comment=" # delete instance")

        return(cmndOutcome)

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "containerPs" :comment "" :extent "verify" :ro "cli" :parsMand "" :parsOpt "freshDeb platformModel" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<containerPs>>  =verify= parsOpt=freshDeb platformModel ro=cli   [[elisp:(org-cycle)][| ]]
#+end_org """
class containerPs(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ 'freshDeb', 'platformModel', ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
             freshDeb: typing.Optional[str]=None,  # Cs Optional Param
             platformModel: typing.Optional[str]=None,  # Cs Optional Param
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {'freshDeb': freshDeb, 'platformModel': platformModel, }
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
        freshDeb = csParam.mappedValue('freshDeb', freshDeb)
        platformModel = csParam.mappedValue('platformModel', platformModel)
####+END:
        if self.cmndDocStr(""" #+begin_org
** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Status of one leaf's instance --- UP (with connect URLs) or not.

        If the instance is up, acknowledge it and show the clickable ssh/vnc/noVNC
        lines; otherwise just report that it is not up.
        #+end_org """): return(cmndOutcome)

        self.captureRunStr(""" #+begin_org
#+begin_src sh :results output :session shared
  rawBuildContainer.cs -i containerPs --freshDeb=13 --platformModel=sysd-priv
#+end_src
#+RESULTS:
        #+end_org """)


        # instance is irrelevant to the leaf path; pass a placeholder.
        if not (results := containerPathObtain(cmndOutcome=cmndOutcome).pyCmnd(
                freshDeb=freshDeb,
                platformModel=platformModel,
                instance='0',
        ).results): return(b_io.eh.badOutcome(cmndOutcome))

        instancePath = results
        proc = 'podmanProc.spcs' if platformModel == 'sysd-podman' else 'dockerProc.spcs'
        imageName = instancePath.name

        psOutcome = b.subProc.WOpW(invedBy=self, log=1).bash(
            f"cd {instancePath} && ./{proc} -i containerProc_instancePs")
        if psOutcome.isProblematic(): return failed(cmndOutcome)
        isUp = any(
            imageName in line and "Up" in line
            for line in (psOutcome.stdout or "").splitlines()
        )

        if isUp:
            print(f"# {imageName} is UP")
            rawBuild_connectInfoShow(self, platformModel, instancePath)
        else:
            print(f"# {imageName} is NOT up")

        return cmndOutcome

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "allContainersPs" :comment "" :extent "verify" :ro "cli" :parsMand "" :parsOpt "" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<allContainersPs>>  =verify= ro=cli   [[elisp:(org-cycle)][| ]]
#+end_org """
class allContainersPs(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {}
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
####+END:
        if self.cmndDocStr(""" #+begin_org
** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Overview of all containers --- docker and podman =ps -a=.

        Runs =ps -a= for each engine that is installed (docker misses podman
        containers and vice versa), so you see everything at a glance.
        #+end_org """): return(cmndOutcome)

        for engine in ('docker', 'podman'):
            print(f"# {engine} ps -a")
            b.subProc.WOpW(invedBy=self, log=1).bash(
                f"command -v {engine} >/dev/null 2>&1 && {engine} ps -a || echo '({engine} not installed)'")

        return cmndOutcome

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "buildImage_rawBisos" :comment "" :extent "verify" :ro "cli" :parsMand "" :parsOpt "freshDeb platformModel noCache" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<buildImage_rawBisos>>  =verify= parsOpt=freshDeb platformModel noCache ro=cli   [[elisp:(org-cycle)][| ]]
#+end_org """
class buildImage_rawBisos(cs.Cmnd):
    cmndParamsMandatory = [ ]
    cmndParamsOptional = [ 'freshDeb', 'platformModel', 'noCache', ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
             freshDeb: typing.Optional[str]=None,  # Cs Optional Param
             platformModel: typing.Optional[str]=None,  # Cs Optional Param
             noCache: typing.Optional[str]=None,  # Cs Optional Param
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {'freshDeb': freshDeb, 'platformModel': platformModel, 'noCache': noCache, }
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
        freshDeb = csParam.mappedValue('freshDeb', freshDeb)
        platformModel = csParam.mappedValue('platformModel', platformModel)
        noCache = csParam.mappedValue('noCache', noCache)
####+END:
        if self.cmndDocStr(f""" #+begin_org
** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Build (rebuild) the container image for a rawBisos leaf.

        This is the image layer, separate from the instance layer (=buildContainer_rawBisos=).
        With =containerPathObtain= use =freshDeb= and =platformModel= to locate the leaf,
        then run /containerProc_imageBuild/ in that directory. Image-related source edits
        (Dockerfile, the ~/raw-bisos payload) are picked up here; a subsequent instance
        build (=buildContainer_rawBisos=) recreates the container from the rebuilt image.
        #+end_org """): return(cmndOutcome)

        self.captureRunStr(""" #+begin_org
#+begin_src sh :results output :session shared
  rawBuildContainer.cs -i buildImage_rawBisos --freshDeb=13 --platformModel=sysd-priv
#+end_src
#+RESULTS:
        #+end_org """)

        # instance is irrelevant to the image; pass a placeholder to reuse the path resolver.
        if not (results := containerPathObtain(cmndOutcome=cmndOutcome).pyCmnd(
                freshDeb=freshDeb,
                platformModel=platformModel,
                instance='0',
        ).results): return(failed(cmndOutcome))

        instancePath = results
        proc = 'podmanProc.spcs' if platformModel == 'sysd-podman' else 'dockerProc.spcs'

        # noCache=True (delivered as the string "True") forwards to the leaf's
        # --noCache=True (docker/podman build --no-cache; the leaf treats it as truthy).
        noCacheOpt = ' --noCache=True' if noCache == "True" else ''

        if b.subProc.WOpW(invedBy=self, log=1).bash(
                f"cd {instancePath} && ./{proc} -i containerProc_imageBuild{noCacheOpt}").isProblematic():
            return failed(cmndOutcome)

        return cmndOutcome

####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "containerPathObtain" :comment "" :extent "verify" :ro "cli" :parsMand "freshDeb platformModel instance" :parsOpt "" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<containerPathObtain>>  =verify= parsMand=freshDeb platformModel instance ro=cli   [[elisp:(org-cycle)][| ]]
#+end_org """
class containerPathObtain(cs.Cmnd):
    cmndParamsMandatory = [ 'freshDeb', 'platformModel', 'instance', ]
    cmndParamsOptional = [ ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
             freshDeb: typing.Optional[str]=None,  # Cs Mandatory Param
             platformModel: typing.Optional[str]=None,  # Cs Mandatory Param
             instance: typing.Optional[str]=None,  # Cs Mandatory Param
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {'freshDeb': freshDeb, 'platformModel': platformModel, 'instance': instance, }
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
        freshDeb = csParam.mappedValue('freshDeb', freshDeb)
        platformModel = csParam.mappedValue('platformModel', platformModel)
        instance = csParam.mappedValue('instance', instance)
####+END:
        if self.cmndDocStr(f""" #+begin_org
** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Return a path object based on input parameters

        Use the =freshDeb=, =platformMode= and =instance= parameters to get a full path of a container image (containerInstancePath)
        For example:: freshDeb=13 platformMode=sysd-priv instance=0
        maps to:: /bisos/git/bxRepos/bxObjects/bro_dockerfiles/debian/13/privileged/vnc/xfce/bisos_deb13-sysd
        #+end_org """): return(cmndOutcome)

        self.captureRunStr(""" #+begin_org
#+begin_src sh :results output :session shared
  rawBuildContainer.cs --freshDeb=13 --platformModel=sysd-priv --instance=0 -i containerPathObtain
#+end_src
#+RESULTS:
        #+end_org """)

        # platformModel maps to the (subDir, leafSuffix) pair under bro_dockerfiles.
        platformModelMap = {
            'confined':    ('confined',      'fresh'),
            'sysd-priv':   ('privileged',    'sysd'),
            'sysd-podman': ('rootless-sysd', 'rootless-sysd'),
        }

        if platformModel not in platformModelMap:
            b_io.eh.problem_usageError(f"Unknown platformModel: {platformModel}")
            return failed(cmndOutcome)

        subDir, leafSuffix = platformModelMap[platformModel]

        # instance is accepted but not yet part of the path. In the future we may
        # append a per-instance subdir (e.g. .../bisos_deb{freshDeb}-{leafSuffix}/{instance}).

        broBase = pathlib.Path("/bisos/git/bxRepos/bxObjects/bro_dockerfiles")
        containerInstancePath = (
            broBase / "debian" / freshDeb / subDir / "vnc" / "xfce"
            / f"bisos_deb{freshDeb}-{leafSuffix}"
        )

        if not containerInstancePath.exists():
            b_io.eh.problem_usageError(f"Missing containerInstancePath: {containerInstancePath}")
            return failed(cmndOutcome)

        return cmndOutcome.set(
            opResults=containerInstancePath,
        )


    ####+BEGIN: blee:bxPanel:foldingSection :outLevel 0 :sep nil :title "Main" :anchor ""  :extraInfo "Framework DBlock"
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*     [[elisp:(outline-show-subtree+toggle)][| _Main_: |]]  Framework DBlock  [[elisp:(org-shifttab)][<)]] E|
#+end_org """
####+END:

####+BEGIN: b:py3:cs:framework/main :csInfo "csInfo" :noCmndEntry "examples" :extraParamsHook "g_extraParams" :importedCmndsModules "g_importedCmndsModules"
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CsFrmWrk   [[elisp:(outline-show-subtree+toggle)][||]] =g_csMain= (csInfo, _examples_, g_extraParams, g_importedCmndsModules)
#+end_org """

if __name__ == '__main__':
    cs.main.g_csMain(
        csInfo=csInfo,
        noCmndEntry=examples,  # specify a Cmnd name
        extraParamsHook=g_extraParams,
        importedCmndsModules=g_importedCmndsModules,
    )

####+END:

####+BEGIN: b:py3:cs:framework/endOfFile :basedOn "classification"
""" #+begin_org
* [[elisp:(org-cycle)][| *End-Of-Editable-Text* |]] :: emacs and org variables and control parameters
#+end_org """

#+STARTUP: showall

### local variables:
### no-byte-compile: t
### end:
####+END:
