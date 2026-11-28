#!/usr/bin/env python
# -*- coding: utf-8 -*-

""" #+begin_org
* ~[Summary]~ :: A =CmndSvc= for rebuilding Raw-BISOS on a box (raw metal). See ./README.org.
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
** This File: /bisos/git/auth/bxRepos/bisos/raw/rebuild/rawBuildBox.cs
** Authors: Mohsen BANAN, http://mohsen.banan.1.byname.net/contact
#+end_org """
####+END:

####+BEGIN: b:py3:file/particulars-csInfo :status "inUse"
""" #+begin_org
* *[[elisp:(org-cycle)][| Particulars-csInfo |]]*
#+end_org """
import typing
csInfo: typing.Dict[str, typing.Any] = { 'moduleName': ['rawBuildBox'], }
csInfo['version'] = '202409222627'
csInfo['status']  = 'inUse'
csInfo['panel'] = 'rawBuildBox-Panel.org'
csInfo['groupingType'] = 'IcmGroupingType-pkged'
csInfo['cmndParts'] = 'IcmCmndParts[common] IcmCmndParts[param]'
####+END:

""" #+begin_org
* [[elisp:(org-cycle)][| ~Description~ |]] :: [[file:/bisos/git/auth/bxRepos/blee-binders/bisos-core/PyFwrk/bisos-pip/bisos.cs/_nodeBase_/fullUsagePanel-en.org][BISOS CmndSvcs Panel]]   [[elisp:(org-cycle)][| ]]

Top-level orchestrator for rebuilding Raw-BISOS on a box (raw metal), over ssh. The same =steps=
model as rawBuildContainer.cs, on top of =unsitedBisosDeploy.sh= (rawBuildSsh.py).

See ./README.org for the bigger picture (Boxes, Guests and Containers).

** Status: In development (AI-WorkPlan.org 3.4); not yet run on a box.
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

        # A box is up already: no create / delete. deBisosIfy is the fast way back to a box without
        # BISOS (warm: some residue stays); a fresh box is a fresh Debian install. AI-WorkPlan.org 3.4.
        exampleTarget = "192.168.0.45"   # edit: the box's IP address
        boxPars = od(targetName=exampleTarget)
        cs.examples.menuChapter(f'=Build rawBisos on a Box --- {exampleTarget} (intra)=')
        cmnd('procBox', pars=od(**boxPars, steps=['reach']),
             comment=" # ssh, sudo, Debian version, /bisos present?")
        cmnd('procBox', pars=od(**boxPars, debInstAcct='intra', debInstAcctPasswd='intra', debInstRootPasswd='intra',
                                steps=['reach']),
             comment=" # the install account and its passwords given (these are the defaults)")
        cmnd('procBox', pars=od(**boxPars, steps=['reach', 'rawBisosBase', 'verify']),
             comment=" # fresh Debian: install + verify (the base only: no reboot, no CBMs)")
        cmnd('procBox', pars=od(**boxPars, steps=['reach', 'rawBisosBase', 'reboot', 'rawBisosCBMs', 'verify']),
             comment=" # CI, fresh Debian: install + reboot (groups) + CBMs + verify")
        cmnd('procBox', pars=od(**boxPars, steps=['reach', 'deBisosIfy', 'rawBisosBase', 'reboot', 'rawBisosCBMs', 'verify']),
             comment=" # CI, iterating: deBisosIfy first (warm, not fresh)")
        cmnd('procBox', pars=od(**boxPars, steps=['reach', 'deBisosIfy']),
             comment=" # remove BISOS from the box")
        cmnd('procBox', pars=od(**boxPars, steps=['reach', 'rawBisosCBMs', 'verify']),
             comment=" # CBMs + verify on an installed box")
        cmnd('procBox', pars=od(**boxPars, steps=['verify']),
             comment=" # verify only (report and logs to /bisos/var/rawBuild/<box>/, compared)")

        # The lower layers --- what the steps run, for running one by hand (bisos/bsip4, on the manager).
        deployPars = f'-p targetName="{exampleTarget}" -p debInstAcct="intra" -p debInstAcctPasswd="intra" -p debInstRootPasswd="intra"'
        cs.examples.menuChapter('=Lower Layers --- unsitedBisosDeploy.sh, sysCharBoxDeploy.sh, ssh=')
        literal('unsitedBisosDeploy.sh   # its own examples menu')
        literal('sysCharBoxDeploy.sh     # its own examples menu')
        literal(f'unsitedBisosDeploy.sh -h -v -n showRun {deployPars} -i l1_raw_bisos   # step rawBisosBase')
        literal(f'unsitedBisosDeploy.sh -h -v -n showRun {deployPars} -i l1_deBisosIfy  # step deBisosIfy')
        literal(f'unsitedBisosDeploy.sh -h -v -n showRun {deployPars} -i l1_reInstall   # deBisosIfy + sysBasePlatform (skips raw-bisos.sh)')
        literal(f'unsitedBisosDeploy.sh -h -v -n showRun {deployPars} -i l1_boxUUID     # the box identity (dmidecode)')
        literal(f'sysCharBoxDeploy.sh -h -v -n showRun -p targetName="{exampleTarget}" -i l1_boxUUIDToBoxNu   # the box identity, registered')
        literal(f'sshpass -p intra ssh -o StrictHostKeyChecking=no intra@{exampleTarget}   # a shell on the box, as the install account')

        return(cmndOutcome)


####+BEGIN: b:py3:cs:cmnd/classHead :cmndName "procBox" :comment "" :extent "verify" :ro "cli" :parsMand "targetName" :parsOpt "debInstAcct debInstAcctPasswd debInstRootPasswd steps" :argsMin 0 :argsMax 0 :pyInv ""
""" #+begin_org
*  _[[elisp:(blee:menu-sel:outline:popupMenu)][±]]_ _[[elisp:(blee:menu-sel:navigation:popupMenu)][Ξ]]_ [[elisp:(outline-show-branches+toggle)][|=]] [[elisp:(bx:orgm:indirectBufOther)][|>]] *[[elisp:(blee:ppmm:org-mode-toggle)][|N]]*  CmndSvc-   [[elisp:(outline-show-subtree+toggle)][||]] <<procBox>>  =verify= parsMand=targetName parsOpt=debInstAcct debInstAcctPasswd debInstRootPasswd steps ro=cli   [[elisp:(org-cycle)][| ]]
#+end_org """
class procBox(cs.Cmnd):
    cmndParamsMandatory = [ 'targetName', ]
    cmndParamsOptional = [ 'debInstAcct', 'debInstAcctPasswd', 'debInstRootPasswd', 'steps', ]
    cmndArgsLen = {'Min': 0, 'Max': 0,}

    @cs.track(fnLoc=True, fnEntry=True, fnExit=True)
    def cmnd(self,
             rtInv: cs.RtInvoker,
             cmndOutcome: b.op.Outcome,
             targetName: typing.Optional[str]=None,  # Cs Mandatory Param
             debInstAcct: typing.Optional[str]=None,  # Cs Optional Param
             debInstAcctPasswd: typing.Optional[str]=None,  # Cs Optional Param
             debInstRootPasswd: typing.Optional[str]=None,  # Cs Optional Param
             steps: typing.Optional[str]=None,  # Cs Optional Param
    ) -> b.op.Outcome:

        failed = b_io.eh.badOutcome
        callParamsDict = {'targetName': targetName, 'debInstAcct': debInstAcct, 'debInstAcctPasswd': debInstAcctPasswd, 'debInstRootPasswd': debInstRootPasswd, 'steps': steps, }
        if self.invocationValidate(rtInv, cmndOutcome, callParamsDict, None).isProblematic():
            return failed(cmndOutcome)
        targetName = csParam.mappedValue('targetName', targetName)
        debInstAcct = csParam.mappedValue('debInstAcct', debInstAcct)
        debInstAcctPasswd = csParam.mappedValue('debInstAcctPasswd', debInstAcctPasswd)
        debInstRootPasswd = csParam.mappedValue('debInstRootPasswd', debInstRootPasswd)
        steps = csParam.mappedValue('steps', steps)
####+END:
        if self.cmndDocStr(""" #+begin_org
** [[elisp:(org-cycle)][| *CmndDesc:* | ]]  Process a box (raw metal) through a serial list of =steps=, over ssh.

        The box is reached as its Debian install account (=debInstAcct=, default intra/intra/intra as
        unsitedBisosDeploy.sh). =steps= is a Python-literal list, run in order, fail-fast. Known steps:
        =reach= (ssh, sudo, Debian version, /bisos), =deBisosIfy= (unsitedBisosDeploy.sh -i
        l1_deBisosIfy: remove BISOS; warm), =rawBisosBase= (unsitedBisosDeploy.sh -i l1_raw_bisos),
        =reboot= (and wait for ssh), =rawBisosCBMs= (sysCbmFullyMaterialize.sh as bystar), =verify=
        (fullyVerify as bystar; its report and logs to /bisos/var/rawBuild/<targetName>/<date>/, compared
        with the previous run of the same start --- fresh or deBisosIfied --- and steps). Examples:
          -i procBox --targetName=192.168.0.45 --steps='["reach"]'
          -i procBox --targetName=192.168.0.45 --steps='["reach", "deBisosIfy", "rawBisosBase", "reboot", "rawBisosCBMs", "verify"]'
        #+end_org """): return(cmndOutcome)

        self.captureRunStr(""" #+begin_org
#+begin_src sh :results output :session shared
  rawBuildBox.cs -i procBox --targetName=192.168.0.45 --steps='["reach"]'
#+end_src
#+RESULTS:
        #+end_org """)

        import rawBuildSsh

        if (stepsList := rawBuildSsh.stepsListParse(steps, rawBuildSsh.sshStepTable)) is None:
            return failed(cmndOutcome)

        target = rawBuildSsh.SshTarget(targetName, debInstAcct, debInstAcctPasswd, debInstRootPasswd)
        if not rawBuildSsh.stepsRun(self, target, stepsList, rawBuildSsh.sshStepTable):
            return failed(cmndOutcome)

        return cmndOutcome

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
