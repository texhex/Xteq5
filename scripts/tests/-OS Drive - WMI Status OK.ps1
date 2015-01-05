#v1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$wmi = Get-WMIObject Win32_OperatingSystem -Property "SystemDrive"$sysdrive=$wmi.SystemDrive#append some output"SysDrive is $sysdrive"$Return = @{Data = ""; Name="OS Drive Health"; }$wmidrive = Get-WmiObject Win32_LogicalDisk -Property "Status" -Filter "DeviceID='$sysdrive'"[string]$status= $wmidrive.Status#Add output"Status is [$status]"#debug test#$status="arg!"#if wmi reports an empty string, this drive can not be checkedif (-Not ([string]::IsNullOrEmpty($status))) {   #If it is not an empty string, we expect the value to be OK   if ($status -eq "OK") {      $Return.Data="OK"      $Return.Text="Windows Management Instrumentation (WMI) reports $sysdrive is healthy"   } else {           #Value is neither "" nor OK       $Return.Data="Fail"      $Return.Text="Windows Management Instrumentation (WMI) class Win32_LocgicalDisk reported status [$status] for $sysdrive"   }} else {   #Drive does not support health status using WMI   $Return.Data="n/a"   $Return.Text="Drive $sysdrive does not support health status"}$Return