#!/usr/bin/env

PROJECT_DIR=src/navigatio
BASE_DIR=${PROJECT_DIR}/bin/Release

rm -rf ${BASE_DIR}/win-x86 && \
dotnet publish $PROJECT_DIR --configuration Release --output ${BASE_DIR}/win-x86 --runtime win-x86 --self-contained true && \
powershell Compress-Archive -Path "'${BASE_DIR}/win-x86/*'" -DestinationPath 'win-x86.zip' -Force

rm -rf ${BASE_DIR}/win-x64 && \
dotnet publish $PROJECT_DIR --configuration Release --output ${BASE_DIR}/win-x64 --runtime win-x64 --self-contained true && \
powershell Compress-Archive -Path "'${BASE_DIR}/win-x64/*'" -DestinationPath 'win-x64.zip' -Force
