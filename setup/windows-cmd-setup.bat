@echo off
set ROOT=C:\navigatio\
set EXE_PATH=src\navigatio\bin\debug\net6.0\

if not exist %ROOT% (
  mkdir %ROOT%
)
xcopy /Y nav.bat %ROOT%
xcopy /Y %EXE_PATH% %ROOT%

:: Uncomment to create a path variable
:: setx /M PATH "%PATH%;%ROOT%
