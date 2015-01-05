#v1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is OK because we expect that Test-TUWoW returns false$Return = @{Data = "OK"; Name="Executed with correct bitness"; Text= "TestUtil is executed with the correct bitness (as 64 bit on a 64 bit OS or 32 bit on a 32 bit OS)"}if (Test-TUWoW) {       #We are affected by WoW64. Report this as Fail (MAJOR issue)   $Return.Data="Fail"   $Return.Text="TestUtil was not started with correct bitness! Please start it using TestUtilLauncher.exe."}$Return