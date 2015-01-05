#Version 1.01
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "Computer_Vendor"; Text = "Vendor (Manufacturer) that build this computer"}
$wmi = Get-WMIObject Win32_ComputerSystem -Property "Manufacturer"$Result.Data=$wmi.Manufacturer$Result