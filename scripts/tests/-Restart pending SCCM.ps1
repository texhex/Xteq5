#v1.03
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "n/a"; Name="No SCCM restart pending"; Text= "No pending changes by SCCM, that require a restart, have been detected"}#$wmi = Get-WMIObject -Namespace 'ROOT\ccm\ClientSDK' -Class 'CCM_ClientUtilities' -list -ErrorAction Ignore#if ($wmi -ne $null) {if (Get-XQAssetValue "SCCM_Enabled" $false) {   $restart_required=$sccm.DetermineIfRebootPending().RebootPending   if ($restart_required) {      $Result.Data="Major"       $Result.Text = "A restart is required in order to finish an operation"   } else {      $Result.Data="OK"    }}
     


$Result