#v1.05
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is OK because we expect that Test-TUWoW returns false$Result = @{Data = "OK"; Name="Executed with correct bitness"; Text= "Xteq5 is executed with the correct bitness (as 64-bit on a 64-bit OS, or as 32-bit on a 32-bit OS)"}if (Test-MPXWoW) {       #We are affected by WoW64. Report this as Fail (MAJOR issue)   $Result.Data="Fail"   $Result.Text="Xteq5 was not started with correct bitness! Please start it using Xteq5Launcher.exe."}$Result