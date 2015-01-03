#Version 1.01
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="OS_Version"; Data = ""; Text = "Internal version of Windows"}
$wmi = Get-WMIObject Win32_OperatingSystem -Property "Version"[version]$ver=[version]$wmi.Version$Result.Data=$ver$Result