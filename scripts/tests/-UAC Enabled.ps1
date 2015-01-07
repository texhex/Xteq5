#v1.01
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "Fail"; Name="User Account Control enabled"; Text= "User Account Control (UAC) is not enabled"} #Note: PowerShell + Registry = DRAIN BAMAGED! #Get-ChildItem -> Returns the PATHS below the given path #Get-ItemProperty -> Returns the VALUES below the given path  
 $test = Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System" -Name "EnableLUA" -ErrorAction Ignore 
 
 if($test -ne $null) {
   if ($test.EnableLUA -eq 1) {
       $Result.Data="OK" 
       $Result.Text="User Account Control (UAC) is enabled"
   }
 }

 
$Result