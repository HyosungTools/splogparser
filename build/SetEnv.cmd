@ECHO OFF
:: First check for Visual Studio 2019 Professional and, if it's present, use that version
SET VisualStudioDir=%ProgramFiles%\Microsoft Visual Studio\2022\Community
IF EXIST "%VisualStudioDir%" GOTO VisualStudio2022

:: Next check for Visual Studio 2019 Enterprise and, if it's present, use that version
SET VisualStudioDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2022\Community
IF EXIST "%VisualStudioDir%" GOTO VisualStudio2019

:: Next check for Visual Studio 2019 Build Tools and, If not present, exit.
SET VisualStudioDir=%ProgramFiles(x86)%\Microsoft Visual Studio\2022\BuildTools
IF EXIST "%VisualStudioDir%" GOTO VisualStudio2019

IF NOT EXIST "%VisualStudioDir%" (
  ECHO Failed to establish VS2022 build environment
  GOTO End
)

:VisualStudio2022
ECHO Establishing VS 2022 build environment using: %VisualStudioDir%
CALL "%VisualStudioDir%\VC\Auxiliary\Build\vcvarsall.bat" x86 10.0.22621.755 -vcvars_ver=14.3
SET VisualStudioDir=

:CloseOut
:: Add the current folder to the PATH so that it's simple to run scripts
:: located in this folder from any location (without trailing backslash)
SET CurDir=%~dp0
SET CurDir=%CurDir:~0,-1%
SET PATH=%PATH%;%CurDir%
SET CurDir=

:End
