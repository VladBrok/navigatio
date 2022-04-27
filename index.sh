#!/usr/bin/env

ROOT=/usr/local/bin/navigatio/
${ROOT}navigatio.exe $@
if test -f ${ROOT}output.sh; then
  source ${ROOT}output.sh
  > ${ROOT}output.sh
fi
