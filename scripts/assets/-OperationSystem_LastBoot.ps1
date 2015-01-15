#Version 1.0
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="OS_LastBoot"; Data = ""; Text = "Time since last boot of the operation system"}
$wmi = Get-WMIObject Win32_OperatingSystem -Property "LastBootupTime"$lastbootdate=$wmi.ConvertToDateTime($wmi.lastbootuptime)write-output "Last boot time $lastbootdate"[timespan]$uptime = (Get-Date) - $lastbootdatewrite-output "Time since last boot is is $uptime"#long format#$display="{0:N0} days, {1:D2} hours, {2:D2} minutes" -f $uptime.Days, $uptime.Hours, $uptime.Minutes$display="{0:N0}d - {1:D2}h - {2:D2}m" -f $uptime.Days, $uptime.Hours, $uptime.Minutes$Result.Data=$display$Result