#define MyAppName "splogparser"
#define MyAppPublisher "HyosungTools"
#define MyAppURL "https://github.com/HyosungTools/splogparser"
#define MyAppExeName "splogparser.exe"
#ifndef MyAppVersion
  #define MyAppVersion GetStringFileInfo("..\dist\splogparser.exe", "ProductVersion")
#endif

[Setup]
AppId={{A7B3C2D4-E5F6-4A7B-8C9D-0E1F2A3B4C5D}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}/releases
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\installer\output
OutputBaseFilename=splogparser-setup-{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequiredOverridesAllowed=dialog
CloseApplications=yes
CloseApplicationsFilter={#MyAppExeName}
RestartApplications=no

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "modifypath"; Description: "Add {app} to the system PATH"; Flags: checkedonce

[Files]
Source: "..\dist\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs; Excludes: "*.pdb"

[InstallDelete]
Type: filesandordirs; Name: "{app}"

[Run]
Filename: "{cmd}"; Parameters: "/C echo splogparser {#MyAppVersion} installed successfully"; Flags: runhidden

[Code]
const
  ModifyPath_Add = 1;
  ModifyPath_Remove = 2;

function GetSystemPath: string;
var
  Path: string;
begin
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE,
    'SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
    'Path', Path) then
    Path := '';
  Result := Path;
end;

procedure SetSystemPath(Path: string);
begin
  RegWriteStringValue(HKEY_LOCAL_MACHINE,
    'SYSTEM\CurrentControlSet\Control\Session Manager\Environment',
    'Path', Path);
end;

function GetUserPath: string;
var
  Path: string;
begin
  if not RegQueryStringValue(HKEY_CURRENT_USER,
    'Environment', 'Path', Path) then
    Path := '';
  Result := Path;
end;

procedure SetUserPath(Path: string);
begin
  RegWriteStringValue(HKEY_CURRENT_USER, 'Environment', 'Path', Path);
end;

procedure ModifyPath(Action: Integer);
var
  AppDir, Path, NewPath: string;
  IsAdmin: Boolean;
begin
  AppDir := ExpandConstant('{app}');
  IsAdmin := IsAdminInstallMode;

  if IsAdmin then
    Path := GetSystemPath
  else
    Path := GetUserPath;

  if Action = ModifyPath_Add then
  begin
    if Pos(Lowercase(AppDir), Lowercase(Path)) = 0 then
    begin
      if (Length(Path) > 0) and (Path[Length(Path)] <> ';') then
        Path := Path + ';';
      NewPath := Path + AppDir;
      if IsAdmin then
        SetSystemPath(NewPath)
      else
        SetUserPath(NewPath);
    end;
  end
  else if Action = ModifyPath_Remove then
  begin
    // Remove the entry including any leading/trailing semicolons
    NewPath := Path;
    if Pos(';' + AppDir, NewPath) > 0 then
      StringChangeEx(NewPath, ';' + AppDir, '', True)
    else if Pos(AppDir + ';', NewPath) > 0 then
      StringChangeEx(NewPath, AppDir + ';', '', True)
    else if Pos(AppDir, NewPath) > 0 then
      StringChangeEx(NewPath, AppDir, '', True);
    if IsAdmin then
      SetSystemPath(NewPath)
    else
      SetUserPath(NewPath);
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    if WizardIsTaskSelected('modifypath') then
      ModifyPath(ModifyPath_Add);
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usPostUninstall then
    ModifyPath(ModifyPath_Remove);
end;
