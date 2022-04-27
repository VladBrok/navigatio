#!/usr/bin/env

ROOT=/usr/local/bin/navigatio
EXE_PATH=src/navigatio/bin/debug/net6.0/

mkdir -p $ROOT
cp index.sh $ROOT
cp -r ${EXE_PATH}. $ROOT
chmod +x ${ROOT}/index.sh

echo "" >> ~/.bashrc
echo "alias nav='source ${ROOT}/index.sh'" >> ~/.bashrc
source ~/.bashrc
