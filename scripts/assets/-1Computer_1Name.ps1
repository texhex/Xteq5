#Version 1.01
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "Computer_Name"; Text = "System name"}
$wmi = Get-WMIObject Win32_ComputerSystem -Property "Name"$Result.Data=$wmi.Name$Result