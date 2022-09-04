#!/usr/bin/env

ROOT=/usr/local/bin/navigatio
EXE_PATH=$1

mkdir -p $ROOT
cp -r ${EXE_PATH}. $ROOT
chmod +x ${ROOT}/nav.sh

echo "" >> ~/.bashrc
echo "alias nav='source ${ROOT}/nav.sh ${ROOT}/'" >> ~/.bashrc
source ~/.bashrc
