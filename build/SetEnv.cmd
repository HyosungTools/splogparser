@ECHO OFF
:: To best mimic github builds use the VS2022 build tools
SET VisualStudioDir="C:\Program Files (x86)\Microsoft Visual Studio\2022\BuildTools"
IF EXIST %VisualStudioDir% GOTO VisualStudio2022
ECHO Failed to find VS2022 build environment
ECHO Visit https://visualstudio.microsoft.com/downloads/
ECHO to install the VS2022 build tools. 
GOTO End

:VisualStudio2022
ECHO Establishing VS 2022 build environment
%VisualStudioDir%\Common7\Tools\VsMSBuildCmd.bat
SET VisualStudioDir=

:CloseOut
:: Add the current folder to the PATH so that it's simple to run scripts
:: located in this folder from any location (without trailing backslash)
SET CurDir=%~dp0
SET CurDir=%CurDir:~0,-1%
SET PATH=%PATH%;%CurDir%
SET CurDir=

:End
