#!/usr/bin/env bash

dotnet build src/navigatio --no-restore && scripts/setup.sh src/navigatio/bin/debug/net6.0/
