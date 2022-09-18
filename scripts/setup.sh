#!/usr/bin/env bash

export ROOT=/usr/local/bin/navigatio
export EXE_PATH=$1

function create_dir() {
  mkdir -p $ROOT
  cp -r ${EXE_PATH}. $ROOT
}

function sudo_create_dir() {
  sudo -E bash -c "$(declare -f create_dir); create_dir"
}

function set_permissions() {
  sudo touch ${ROOT}/output.sh
  sudo touch ${ROOT}/aliases.json
  sudo touch ${ROOT}/history.json

  sudo chmod a+x ${ROOT}/nav.sh
  sudo chmod a+x ${ROOT}/Navigatio
  sudo chmod a+wx ${ROOT}/output.sh
  sudo chmod a+w ${ROOT}/settings.json
  sudo chmod a+w ${ROOT}/aliases.json
  sudo chmod a+w ${ROOT}/history.json
}

function create_alias() {
  echo "" >> ~/.bashrc
  echo "alias nav='source ${ROOT}/nav.sh ${ROOT} ${ROOT}/output.sh'" >> ~/.bashrc
}

unamestr=$(uname)
if [[ "$unamestr" == 'Linux' ]]
then
  sudo_create_dir
  set_permissions
else
  create_dir
fi

create_alias

unset ROOT
unset EXE_PATH
