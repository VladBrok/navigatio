#!/usr/bin/env

ROOT=$1
${ROOT}navigatio.exe ${@:2}
if test -f ${ROOT}output.sh; then
  source ${ROOT}output.sh
  > ${ROOT}output.sh
fi
