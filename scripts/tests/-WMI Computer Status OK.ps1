#v1.04
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "OK"; Name="WMI Computer status is healthy"; Text= "Windows Management Instrumentation (WMI) reports this system is healthy"}$wmi = Get-WMIObject Win32_ComputerSystem -Property "Status"[string]$status= $wmi.Status#$status="arg!"if ($status -ne "OK") {   $Result.Data="Fail"   $Result.Text="Windows Management Instrumentation (WMI) class Win32_ComputerSystem reported status [$status]"}
$Result