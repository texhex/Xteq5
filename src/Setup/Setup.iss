#include <ISPPBuiltins.iss>

#pragma option -v+
#pragma verboselevel 9


; The main information is the release date. 
; We generate it from the current date/time upun compliation of this script
; Example result: 2014.12.30.1401
; This will also be used inside the version info of the resulting SetupXteq5.exe
; Unfortunately, I dot find a way to use UTC.
#define public CurrentDateISO GetDateTimeString('yyyy/mm/dd.hhnn', '.', '');

; Extract version from x64\Release\Xteq5.Engine.dll
; SourcePath ({#SourcePath} in scripts) is a pre-defined variable that contains the path where the ISS file is located
#define public ProgramVersion GetFileVersion(SourcePath +'..\Xteq5Engine\bin\x64\Release\Xteq5.Engine.dll')

; Default file to be started
#define public StartExeName 'Xteq5Launcher.exe'

  

[Setup]
AppName=Xteq5 
AppVersion={#CurrentDateISO} ({#ProgramVersion}) 

;Internal AppId. DO NOT CHANGE! 
AppId=Xteq5

AppPublisher=Michael 'Tex' Hex
AppPublisherURL=http://www.Xteq5.com/
AppSupportURL=https://github.com/texhex/Xteq5/wiki
AppComments=Build with Inno Setup

;VersionInfoVersion={#ProgramVersion}
VersionInfoVersion={#CurrentDateISO}
VersionInfoCopyright=Copyright © 2010-2015 Michael Hex 

;I really think we should set this to NO...
Uninstallable=Yes 

;We want to write to program files
PrivilegesRequired=admin

;Install in x64 mode (when available)
ArchitecturesInstallIn64BitMode=x64 

;Allow to be installed in x32 or x64 mode
ArchitecturesAllowed=x86 x64

;Store to C:\Program Files\Xteq5 
DefaultDirName={pf}\Xteq5

;Icon inside Add/Remove programs
UninstallDisplayIcon={app}\{#StartExeName}

;Place resulting Setup.exe in the same folder as the ISS file
OutputDir={#SourcePath}
OutputBaseFilename=Xteq5Setup

;Set source dir to folder above the location of this file.
;Example: This file is located at C:\dev\git\xteq5\src\Setup\
;The source dir is in this case C:\dev\git\xteq5\
SourceDir={#SourcePath}..\..\

;License is Apache 2.0
LicenseFile=licenses\LICENSE-Apache-2.0.txt
;Readme is our license summary
InfoBeforeFile=licenses\LICENSE.txt


;No cancel during install
AllowCancelDuringInstall=False

;If program is running, close it without question if running silently
CloseApplications=yes

;Do not restart the program was closed
RestartApplications=no

;Do not warn if we install into a folder that already exists
DirExistsWarning=no

;Only allow setup to run on Windows 7 and upwards. Needed anyway because of Restart Manager
MinVersion=6.1

;LZMA2 compression at level MAX
Compression=lzma2

;We can use solid compression as all files will be installed
SolidCompression=yes

;Do not allow use to select a folder because we want to be installed into the native C:\Program Files folder and 
DisableDirPage=yes

;We create a single icon anyway 
DisableProgramGroupPage=yes

;Give the user the last chance to stop the installation
DisableReadyPage=no

;;If we change file associations this flag will instruct InnoSetup at the end to refresh all Explorer icons
;ChangesAssociations=yes




[Icons]
;Start menu icon
Name: "{commonprograms}\Xteq5"; Filename: "{app}\{#StartExeName}"; Parameters: ""; IconFilename: "{app}\{#StartExeName}"

;Link to folder in commonappdata within the program folder for easier access
Name: "{app}\Files in ProgramData"; Filename: "{commonappdata}\Xteq5\"; Comment: "Files in ProgramData";


[Dirs]
;Create a folder in common app data (C:\ProgramData\Xteq5) to store the scripts there
;Also grant users the modify permission, in case they which to add addtional scripts 
Name: "{commonappdata}\Xteq5"; Permissions: users-modify


[InstallDelete]
;Upon installation, delete all files in the APP path (C:\Program Files\Xteq5) to make sure we have a clean state
Type: files; Name: "{app}";

;Also, clean \assets and \tests to make sure scripts we have deleted from the repo in the meantime are deleted
Type: files; Name: "{commonappdata}\Xteq5\assets\-*.ps1";
Type: files; Name: "{commonappdata}\Xteq5\tests\-*.ps1";


;Up to version 2015.01.03 we have used "_" instead of "-"
Type: files; Name: "{commonappdata}\Xteq5\assets\_*.ps1";
Type: files; Name: "{commonappdata}\Xteq5\tests\_*.ps1";


[Files]
;If selected by the user: 
; Copy helper module to PS Modules path of the current user to make sure the user is able to use them outside Xteq5 as well
; See MSDN: http://msdn.microsoft.com/en-us/library/dd878350%28v=vs.85%29.aspx
; Path: C:\Users\<<USERNAME>>\Documents\WindowsPowerShell\Modules\Xteq5Helpers
Source: "scripts\modules\Xteq5Helpers\Xteq5Helpers.psm1"; DestDir: "{userdocs}\WindowsPowerShell\Modules\Xteq5Helpers\"; Flags: ignoreversion; Check: InstallModulesToUserModules;
;Do the same for the MPX module
Source: "scripts\modules\MPXmodule\MPXmodule.psm1"; DestDir: "{userdocs}\WindowsPowerShell\Modules\MPXmodule\"; Flags: ignoreversion; Check: InstallModulesToUserModules; 


;Copy entire scripts folder to commonappdata
Source: "scripts\*.*"; DestDir: "{commonappdata}\Xteq5\"; Flags: ignoreversion recursesubdirs;

;All license files go to \licenses
Source: "licenses\*.*"; DestDir: "{app}\licenses"; Flags: ignoreversion recursesubdirs;

;Xteq5Launcher 
Source: "src\Xteq5Launcher\bin\release\*.exe"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs;
Source: "src\Xteq5Launcher\bin\release\*.config"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs;

;Xteq5GUI (64 bit)
Source: "src\Xteq5GUI\bin\x64\release\*.exe"; DestDir: "{app}\64bit"; Flags: ignoreversion recursesubdirs;
Source: "src\Xteq5GUI\bin\x64\release\*.dll"; DestDir: "{app}\64bit"; Flags: ignoreversion recursesubdirs;
Source: "src\Xteq5GUI\bin\x64\release\*.config"; DestDir: "{app}\64bit"; Flags: ignoreversion recursesubdirs;

;Xteq5GUI (32 bit)
Source: "src\Xteq5GUI\bin\x86\release\*.exe"; DestDir: "{app}\32bit"; Flags: ignoreversion recursesubdirs;
Source: "src\Xteq5GUI\bin\x86\release\*.dll"; DestDir: "{app}\32bit"; Flags: ignoreversion recursesubdirs;
Source: "src\Xteq5GUI\bin\x86\release\*.config"; DestDir: "{app}\32bit"; Flags: ignoreversion recursesubdirs;


[Ini]
;Create VERSION.ini to allow the user to easily see the version currently installed. 
Filename: "{app}\VERSION.ini"; Section: "Version"; Key: "Version"; String: "{#CurrentDateISO}"
Filename: "{app}\VERSION.ini"; Section: "Version"; Key: "BinaryVersion"; String: "{#ProgramVersion}"


[Registry]
;Add xteq5launcher.exe to app paths  This means Windows knows where to find the file when "xteq5.exe" is requested somewhere
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\xteq5launcher.exe"; ValueType: string; ValueName: ; ValueData: "{app}\{#StartExeName}";
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\xteq5launcher.exe"; ValueType: string; ValueName: "Path"; ValueData: "{app}";

;Do the same but with a little cheating: Define Xteq5Launcher.exe as Xteq5.exe 
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\xteq5.exe"; ValueType: string; ValueName: ; ValueData: "{app}\{#StartExeName}";
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\xteq5.exe"; ValueType: string; ValueName: "Path"; ValueData: "{app}";








[Code]

var
 bDotNetAvailable:boolean;
 bPowerShellAvailable:boolean;
 
 bComponentMissing:boolean;
 
 sComponentNotFoundWizard_Text:string;
 pagComponentNotFound:TOutputMsgMemoWizardPage;

 pagInstallModules:TInputOptionWizardPage;
 bInstallModules:boolean;
 

//Source: [Check .NET Version with Inno Setup](http://kynosarges.org/DotNetVersion.html) by [Christoph Nahr](http://kynosarges.org/index.html#Contact)
function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1.4322'     .NET Framework 1.1
//    'v2.0.50727'    .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//    'v4.5'          .NET Framework 4.5
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
    key: string;
    install, release, serviceCount: cardinal;
    check45, success: boolean;
begin
    // .NET 4.5 installs as update to .NET 4.0 Full
    if version = 'v4.5' then begin
        version := 'v4\Full';
        check45 := true;
    end else
        check45 := false;

    // installation key group for all .NET versions
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + version;
    
    //Debug version. Will fail the test ALWAY
    //key := 'SOFTWARE\B0RKEN\Microsoft\NET Framework Setup\NDP\' + version;

    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;

    // .NET 4.0/4.5 uses value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;

    // .NET 4.5 uses additional value Release
    if check45 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Release', release);
        success := success and (release >= 378389);
    end;

    result := success and (install = 1) and (serviceCount >= service);
end;

//Checks for a given powershell version
//See: blogs.technet.com/b/heyscriptingguy/archive/2009/01/05/how-do-i-check-which-version-of-windows-powershell-i-m-using.aspx
function IsPowerShellDetected(version: string): boolean;
var
    value:string;
begin
   result:=false;

   if (RegQueryStringValue(HKLM, 'SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine', 'PSCompatibleVersion', value)) then begin
      if (Pos(version,value)>0) then begin
          result:=true;
      end;
   end;

end;




procedure InitializeWizard();
begin 
  //Create custom "Component not found" page after welcome page
  pagComponentNotFound := 
     CreateOutputMsgMemoPage(wpWelcome,
         'System Requirements not met', 
         'A component that is required was not found',
         '',
         sComponentNotFoundWizard_Text);

  pagInstallModules :=
      CreateInputOptionPage(wpSelectComponents,
         'Install custom PowerShell modules', 
         '',
         'If you plan to create your own assets and tests, the custom PowerShell modules of Xteq5 should also be copied to your personal modules path.'+#13#10+#13#10+
         'This is NOT required if you just want to execute Xteq5.'+#13#10,
          False, False);

       pagInstallModules.Add('Install modules to %UserProfile%\Documents\WindowsPowerShell\Modules');
       pagInstallModules.Values[0] := False;
end;


function InitializeSetup(): Boolean;
var
 sTemp:string;
begin
  bComponentMissing:=false;
  sComponentNotFoundWizard_Text:='';

  bInstallModules:=false;

  
  //Check if .NET 4.5 (SP does not count) is available
  bDotNetAvailable:=IsDotNetDetected('v4.5', 0);

  log('System requirements check:');
  if bDotNetAvailable then begin
     log('   .NET 4.5 has been detected');
  end else begin
     log('   .NET 4.5 could not be found');
  end;

  //Check if PowerShell 4.0 is available
  bPowerShellAvailable:=IsPowerShellDetected('4.0');
  if bPowerShellAvailable then begin
     log('   PowerShell 4.0 has been detected');
  end else begin
     log('   PowerShell 4.0 could not be found');
  end;

  if (bDotNetAvailable=false) OR (bPowerShellAvailable=false) then begin
     bComponentMissing:=true;
 
     //Set the text of the custom page so the user knows what to download.

     //There is a bug with WMF 4.0 that, if .NET 4.5 is NOT Installed, it will report SUCCESS altough PowerShell wasn't installed at all. 
     //   See: http://blogs.msdn.com/b/powershell/archive/2013/10/29/wmf-4-0-known-issue-partial-installation-without-net-framework-4-5.aspx          
     //Therefore, prefer the display of the .NET download link if both components are missing. 

     if (bPowerShellAvailable=false) then begin  
        sTemp:='';      
        sTemp:=sTemp + 'PowerShell 4.0 (or a compatible version) was not found.'+#13#10+#13#10;
        sTemp:=sTemp + 'Please download and install Windows Management Framework 4.0, then re-run setup.'+#13#10+#13#10;
        sTemp:=sTemp + 'http://www.microsoft.com/en-us/download/details.aspx?id=40855';
        //This download link is also used in /README.md
     end;

     if (bDotNetAvailable=false) then begin
        sTemp:='';      
        sTemp:=sTemp + '.NET Framework 4.5 (or a compatible version) was not found.'+#13#10+#13#10;
        sTemp:=sTemp + 'Please download and install it, then re-run setup.'+#13#10+#13#10;
        sTemp:=sTemp + 'http://www.microsoft.com/en-us/download/details.aspx?id=40773'; //http://go.microsoft.com/?linkid=9831985' - Offline installer
        //This download link is also used in /README.md
     end;
     
     sTemp:=sTemp + #13#10 + #13#10 + #13#10 + #13#10 + 'Sorry for the trouble.';         
     sComponentNotFoundWizard_Text:=sTemp;   
  end;


  result:=true;
end;

function InstallModulesToUserModules(): Boolean;
begin
  result:=bInstallModules;
end;


function ShouldSkipPage(PageID: Integer): Boolean;
begin
  result := false;

  if PageID=pagComponentNotFound.ID then begin
     //Skip the "Component missing" if all components were found
     if (bComponentMissing=false) then begin
         result:=true;
     end;
  end;

end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  result:=true;

  //If the "component not found" page is displayed, do not allow to continue.
  if CurPageID=pagComponentNotFound.ID then begin     
     result:=false;
     WizardForm.Close;
  end;

  //Read the value the user has selected 
  if CurPageID=pagInstallModules.ID then begin
     result:=true;
     bInstallModules:=pagInstallModules.Values[0];   
  end;

end;

procedure CancelButtonClick(CurPageID: Integer; var Cancel, Confirm: Boolean);
begin
  //Allow exit from the "Component not found" page without asking "Are you sure to exit?"
  if CurPageID=pagComponentNotFound.ID then begin
     Cancel:=true;
     Confirm:=false;
  end;
end;



#expr SaveToFile(AddBackslash(SourcePath) + "zz_Temp_Preprocessed.iss")
