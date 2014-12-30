#include <ISPPBuiltins.iss>

#pragma option -v+
#pragma verboselevel 9

; The main information is the release date. 
; We generate it from the current date/time upun compliation of this script
; Example result: 2014.12.30.1401
; This will also be used inside the version info of the resulting Setup.exe
#define public CurrentDateISO GetDateTimeString('yyyy/mm/dd.hhnn', '.', '');

; Extract version from x64\Release\TestUtil.Engine.dll
; SourcePath ({#SourcePath} in scripts) is a pre defined variable that contains 
; the path where this file is located
#define public ProgramVersion GetFileVersion(SourcePath +'..\TestUtilEngine\bin\x64\Release\TestUtil.Engine.dll')

; Default file to be started
#define public StartExeName 'TestUtilLauncher.exe'

  

[Setup]
AppName=TestUtil 
AppVersion={#CurrentDateISO} ({#ProgramVersion}) 

;Better add something to the AppID to avoid a name collision
AppId=MTHTestUtil

AppPublisher=Michael 'Tex' Hex
AppPublisherURL=http://www.testutil.com/
AppSupportURL=https://github.com/texhex/testutil/wiki
AppComments=Build with Inno Setup

;VersionInfoVersion={#ProgramVersion}
VersionInfoVersion={#CurrentDateISO}
VersionInfoCopyright=Copyright © 2010-2015 Michael 'Tex' Hex 

;I really think we should set this to NO...
Uninstallable=Yes 

;We want to write to program files
PrivilegesRequired=admin

;Install in x64 mode (when available)
ArchitecturesInstallIn64BitMode=x64 

;Allow to be installed in x32 or x64 mode
ArchitecturesAllowed=x86 x64

;This should be a good name for the files
DefaultDirName={pf}\TestUtil

;Icon inside Add/Remove programs
UninstallDisplayIcon={app}\{#StartExeName}

;Place resulting Setup.exe in the same folder as the ISS file
OutputDir={#SourcePath}
OutputBaseFilename=TestUtilSetup

;Set source dir to folder above the location of this file.
;Example: This file is located at C:\dev\gitrepos\testutil\src\Setup\
;The source dir is in this case C:\dev\gitrepos\testutil\
SourceDir={#SourcePath}..\..\

;License is Apache 2.0
LicenseFile=licenses\LICENSE-Apache-2.0.txt
;Readme is our license summary
InfoBeforeFile=licenses\LICENSE.txt


;No cancel during install
AllowCancelDuringInstall=False

;If TestUtil is running, close it without question if running silently
CloseApplications=yes

;Do not restart if TestUtil was closed
RestartApplications=no

;Do not warn if we install into a folder that already exists
DirExistsWarning=no

;Only allow setup to run on Windows 7 and upwards. Needed anyway because of Restart Manager
MinVersion=6.1

;LZMA2 compression at level MAX
Compression=lzma2

;We can use solid compression as all files will be installed
SolidCompression=yes

;Do not allow use to select a folder
DisableDirPage=yes

;We create a single icon anyway 
DisableProgramGroupPage=yes

;Give the user the last chance to stop the installation
DisableReadyPage=no

;;If we change file associations this flag will instruct InnoSetup at the end to refresh all Explorer icons
;ChangesAssociations=yes



[Icons]
;Start menu icon
Name: "{commonprograms}\TestUtil"; Filename: "{app}\{#StartExeName}"; Parameters: ""; IconFilename: "{app}\{#StartExeName}"

;Link to folder in commonappdata within the program folder for easier access
Name: "{app}\Files in ProgramData"; Filename: "{commonappdata}\TestUtil\"; Comment: "Files loaded from ProgramData";


[Files]
;Copy helper module to PS Modules path of the current user to make sure the user is able to use them outside TestUtil as well
;See MSDN: http://msdn.microsoft.com/en-us/library/dd878350%28v=vs.85%29.aspx
;Path: C:\Users\<<USERNAME>>\Documents\WindowsPowerShell\Modules\TestUtilHelpers
Source: "scripts\modules\TestUtilHelpers\TestUtilHelpers.psm1"; DestDir: "{userdocs}\WindowsPowerShell\Modules\TestUtilHelpers\"; Flags: ignoreversion;

;Copy all scripts to commonappdata
Source: "scripts\*.*"; DestDir: "{commonappdata}\TestUtil\"; Flags: ignoreversion recursesubdirs;

;All license files go to \licenses
Source: "licenses\*.*"; DestDir: "{app}\licenses"; Flags: ignoreversion recursesubdirs;

;TestUtilLauncher 
Source: "src\TestUtilLauncher\bin\release\*.exe"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs;
Source: "src\TestUtilLauncher\bin\release\*.config"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs;

;TestUtilGUI (64 bit)
Source: "src\TestUtilGUI\bin\x64\release\*.exe"; DestDir: "{app}\64bit"; Flags: ignoreversion recursesubdirs;
Source: "src\TestUtilGUI\bin\x64\release\*.dll"; DestDir: "{app}\64bit"; Flags: ignoreversion recursesubdirs;
Source: "src\TestUtilGUI\bin\x64\release\*.config"; DestDir: "{app}\64bit"; Flags: ignoreversion recursesubdirs;

;TestUtilGUI (32 bit)
Source: "src\TestUtilGUI\bin\x86\release\*.exe"; DestDir: "{app}\32bit"; Flags: ignoreversion recursesubdirs;
Source: "src\TestUtilGUI\bin\x86\release\*.dll"; DestDir: "{app}\32bit"; Flags: ignoreversion recursesubdirs;
Source: "src\TestUtilGUI\bin\x86\release\*.config"; DestDir: "{app}\32bit"; Flags: ignoreversion recursesubdirs;


[Dirs]
;Create a folder in common app data (C:\ProgramData\TestUtil) to store the scripts there
;Also grant users the modify permission, in case they which to add addtional scripts 
Name: "{commonappdata}\TestUtil"; Permissions: users-modify


[Registry]
;Add testutillauncher.exe to app paths  This means Windows knows where to find the file when "testutil.exe" is requested somewhere
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\testutillauncher.exe"; ValueType: string; ValueName: ; ValueData: "{app}\{#StartExeName}";
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\testutillauncher.exe"; ValueType: string; ValueName: "Path"; ValueData: "{app}";

;Do the same but with a little cheating: Define TestUtilLauncher.exe as TestUtil.exe 
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\testutil.exe"; ValueType: string; ValueName: ; ValueData: "{app}\{#StartExeName}";
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\testutil.exe"; ValueType: string; ValueName: "Path"; ValueData: "{app}";



[InstallDelete]
;In case this is a left over from a manual installation -> KILL IT! 
Type: files; Name: "{app}";

;I'm not sure if we should do this?
;;;Type: files; Name: "{commonappdata}\TestUtil";


;Code Section to determin the language
[Code]
{
function GetLanguageDLL(Param: String): String;
var
 iLng:integer;
 iLngGeneral:integer;
begin
 //Full list of language identifiers: http://msdn.microsoft.com/en-us/library/dd318693.aspx
  iLng:=GetUILanguage 

  //The result contains the EXACT language, e.g. DE-DE. However we are only interested in
  //the primary language (e.g. "DE") so we do an AND $3FF to extract it
  iLngGeneral:=iLng and $3FF

  case iLngGeneral of 
  $01:
    begin 
      Result:='Arabic';
    end; 
  $04:
    begin 
      Result:='Chinese_Simp';
    end; 
  $05:
    begin 
      Result:='Czech1';
    end; 
  $07:
    begin 
      Result:='Deutsch';
    end; 
  $09:
    begin 
      Result:='English';
    end; 
  $0C:
    begin 
      Result:='French';
    end; 
  $10:
    begin 
      Result:='Italian';
    end; 
  $11:
    begin 
      Result:='Japanese';
    end; 
  $16:
    begin 
      Result:='Portugues-Brasil';
    end; 
  $19:
    begin 
      Result:='Russian';
    end; 
  $0A:
    begin 
      Result:='Spanish';
    end; 

  else
    begin 
       Result:='';
    end; 
  end;

  //Just for tests:
  //Result:='Portugues-Brasil';
     
end;

//Returns TRUE of the .ico file type is set incorrectly (assocaiated with IrfanView - first version of this setup)
function CheckICOFileTypeIsIncorrect(): Boolean;
var
 sFiletype:string;
begin
   result:=false;

   if RegQueryStringValue(HKEY_CLASSES_ROOT, '.ico','', sFiletype) then begin
      if sFileType='IrfanViewDefaultFile' then
         result:=true;
   end;      
end;
}





#expr SaveToFile(AddBackslash(SourcePath) + "zz_Temp_Preprocessed.iss")
