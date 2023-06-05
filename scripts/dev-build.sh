#!/usr/bin/env bash

dotnet build src/Navigatio --no-restore && scripts/setup.sh src/Navigatio/bin/debug/net6.0/
