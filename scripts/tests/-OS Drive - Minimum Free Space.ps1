#v1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$wmi = Get-WMIObject Win32_OperatingSystem -Property "SystemDrive"$sysdrive=$wmi.SystemDrive#append some output"SysDrive is $sysdrive"$Return = @{Data = "OK"; Name="Minimum free disk space on system drive"; Text="More than 512 MB of free disk space is available on $sysdrive"}#We now search for information about this in Win32_LogicalDisk$wmidrive = Get-WmiObject Win32_LogicalDisk -Property "FreeSpace" -Filter "DeviceID='$sysdrive'"[int64]$freespace=$wmidrive.FreeSpace$freespace=$freespace/1mb#append some output again. Just because we can. "Free space in MB $freespace"#Debug#$freespace=344#If less than 512MB are available, report Majorif ($freespace -lt 512) {   $Return.Data="Fail"   $Return.Text="$sysdrive has only $freespace MB of free disk space"}$Return