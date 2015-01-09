#v1.07
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$wmi = Get-WMIObject Win32_OperatingSystem -Property "SystemDrive"$sysdrive=$wmi.SystemDrive#append some output"SysDrive is $sysdrive"$Result = @{Data = ""; Name="Operating System drive is healthy"; Text="Drive $sysdrive does not support health status using Windows Management Instrumentation (WMI)"}$wmidrive = Get-WmiObject Win32_LogicalDisk -Property "Status" -Filter "DeviceID='$sysdrive'"[string]$status= $wmidrive.Status#Add output"Status is [$status]"#debug test#$status="arg!"#if wmi reports an empty string, this drive can not be checkedif (-Not (Test-MPXStringIsNullOrWhiteSpace $status)) {   #If it is not an empty string, we expect the value to be OK   if ($status -eq "OK") {      $Result.Data="OK"      $Result.Text="Windows Management Instrumentation (WMI) reports $sysdrive is healthy"   } else {           #Value is neither "" nor OK       $Result.Data="Fail"      $Result.Text="Windows Management Instrumentation (WMI) class Win32_LocgicalDisk reports status [$status] for $sysdrive"   }} $Result