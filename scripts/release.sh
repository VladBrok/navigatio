#!/usr/bin/env

PROJECT_DIR=src/navigatio
BASE_DIR=${PROJECT_DIR}/bin/Release

rm -rf ${BASE_DIR}/x86 && \
dotnet publish $PROJECT_DIR -c Release -o ${BASE_DIR}/x86 -a x86 --sc true && \
powershell Compress-Archive -Path "'${BASE_DIR}/x86/*'" -DestinationPath 'windows-x86.zip' -Force

rm -rf ${BASE_DIR}/x64 && \
dotnet publish $PROJECT_DIR -c Release -o ${BASE_DIR}/x64 -a x64 --sc true && \
powershell Compress-Archive -Path "'${BASE_DIR}/x64/*'" -DestinationPath 'windows-x64.zip' -Force
