#!/bin/bash

# Fully materialize this system's CBMs (Capability Bundle Materializations):
#     sysCbmManage.cs -i fullyMaterialize
#
# The one copy for every platform (bisos/raw rebuild AI-WorkPlan.org 3.3): on an installed system it is
# /bisos/core/raw/bin/sysCbmFullyMaterialize.sh, on PATH. A container's ~/raw-bisos/ copy (from
# bro_dockerfiles/common/raw-bisos/) runs this one; a box or a guest runs it over ssh.
#
# Run as bystar, AFTER raw-bisos.sh and a fresh login (a container down+up, a box reboot), so the
# groups that raw-bisos.sh created take effect:
#     sysCbmFullyMaterialize.sh
#
# bystar's ~/.bashrc returns early for non-interactive shells (the standard
# Debian 'case $- in *i*)' guard), before it sets PATH (pipx / bisos3 bin) and
# nvm. Force an interactive shell so sysCbmManage.cs (and node via nvm)
# resolve. Without a tty, -i prints harmless "no job control" warnings.
#
# As raw-bisos.sh does, all output (stdout and stderr) also goes to a log in ~:
#     ~/sysCbmFullyMaterialize-<yymmddHHMMSS>-log.org

localName="sysCbmFullyMaterialize"   # scripts name sans .sh
dateTag=$( date +%y%m%d%H%M%S )
logFile=~/${localName}-${dateTag}-log.org

cd ~ || exit 1

exec &> >(tee "${logFile}")

echo "${localName}: logging to ${logFile}"

bash -ic 'sysCbmManage.cs -i fullyMaterialize'
exitCode=$?

echo "${localName}: exit ${exitCode}; log: ${logFile}"
exit ${exitCode}
