#v1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Make it a minor test$Result = @{Data = "Minor"; Name="File extensions displayed"; Text= "Windows currently hides several file extensions. This could be used to trick you into opening an unwanted application."}#Add some explainationwrite-output "This setting can be changed by opening Windows Explorer, Options -> Folder Options -> Tab 'View' -> Disable 'Hide extensions for known file types'" #Note: PowerShell + Registry = DRAIN BAMAGED! #Get-ChildItem -> Returns the PATHS below the given path #Get-ItemProperty -> Returns the VALUES below the given path  
 $test = Get-ItemProperty -Path "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced" -Name "HideFileExt" -ErrorAction Ignore 
 
 if($test -ne $null) {
   if ($test.HideFileExt -eq 0) {
       $Result.Data="OK" 
       $Result.Text="Windows displays file extensions"
   }
 }

 
$Result