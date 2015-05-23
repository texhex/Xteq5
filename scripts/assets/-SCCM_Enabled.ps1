#Version 1.02
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "SCCM_Enabled"; Text = "Has this computer the System Center Configuration Manager (SCCM) agent installed"}
$wmi = Get-WMIObject -Namespace 'ROOT\ccm\ClientSDK' -Class 'CCM_ClientUtilities' -list -ErrorAction Ignoreif ($wmi -ne $null) {   $Result.Data=$true} else {   $Result.Data="n/a"}  $Result