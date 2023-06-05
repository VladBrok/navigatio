#!/usr/bin/env bash

export ROOT=/usr/local/bin/navigatio

_navigatio_completions() {
  COMPREPLY=($(compgen -W "$(_extract_keys_from_json $ROOT/aliases.json)" -- "${COMP_WORDS[-1]}"))
}

_extract_keys_from_json() {
  local file=$1
  cat $file | tr -d "[:space:]" | grep -o '"[^"]\+":' | tr -d '":'
}

complete -F _navigatio_completions nav
