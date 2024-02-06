cd /d %~dp0
set EMVpath=D:\EMVINI

rem *** Note: Drive name D is not exist,copy ini file in C drive instead
if not exist "D:\" set EMVpath=C:\EMVINI

rem *** Note: EMV files will probably evaluate to: C:\EMVINI
if not exist "%EMVpath%" md "%EMVpath%"

for /f "delims=" %%i in ('dir /b') do (
  xcopy /R /F /Y "%%i" "%EMVpath%\"
)
