@echo off
set ROOT=%~dp0
set OUTPUT_FILE=%ROOT%output.bat

%ROOT%navigatio.exe %OUTPUT_FILE% %*

if exist %OUTPUT_FILE% (
  %OUTPUT_FILE%
  break>%OUTPUT_FILE%
)
