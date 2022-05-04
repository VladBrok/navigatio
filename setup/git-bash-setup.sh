#!/usr/bin/env

ROOT=/usr/local/bin/navigatio
EXE_PATH=src/navigatio/bin/debug/net6.0/

mkdir -p $ROOT
cp nav.sh $ROOT
cp -r ${EXE_PATH}. $ROOT
chmod +x ${ROOT}/nav.sh

# Uncomment to create an alias
#echo "" >> ~/.bashrc
#echo "alias nav='source ${ROOT}/nav.sh ${ROOT}/'" >> ~/.bashrc
#source ~/.bashrc
