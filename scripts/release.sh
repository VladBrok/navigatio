#!/usr/bin/env bash

PROJECT_DIR=src/navigatio
BASE_DIR=${PROJECT_DIR}/bin/Release

function release_for_os() {
  rm -rf ${BASE_DIR}/$1 && \

  dotnet publish $PROJECT_DIR --configuration Release --output ${BASE_DIR}/$1 --runtime $1 --self-contained true && \

  powershell Compress-Archive -Path "'${BASE_DIR}/$1/*'" -DestinationPath "$1.zip" -Force
}

release_for_os win-x86
release_for_os win-x64
release_for_os linux-x64
