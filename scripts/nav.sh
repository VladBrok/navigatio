#!/usr/bin/env

ROOT=$1
OUTPUT_FILE=${ROOT}output.sh

${ROOT}Navigatio ${OUTPUT_FILE} ${@:2}

if test -f ${OUTPUT_FILE}; then
  source ${OUTPUT_FILE}
  > ${OUTPUT_FILE}
fi
