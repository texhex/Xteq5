#Version 1.01
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "Computer_Model"; Text = "Model of this computer, set by the vendor"}
$wmi = Get-WMIObject Win32_ComputerSystem -Property "Model"$Result.Data=$wmi.Model$Result