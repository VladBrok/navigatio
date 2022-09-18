#!/usr/bin/env bash

ROOT=$1
OUTPUT_FILE=$2

${ROOT}/Navigatio ${OUTPUT_FILE} ${@:3}

if test -f ${OUTPUT_FILE}; then
  source ${OUTPUT_FILE}
  > ${OUTPUT_FILE}
fi
