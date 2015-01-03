#Version 1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "Memory_Free"; Text = "Free system memory in gigabyte (GB)"}
$wmi = Get-WMIObject Win32_OperatingSystem -Property "FreePhysicalMemory"[int]$mem = $wmi.FreePhysicalMemory / 1mb #as the value is in kilo bytes$Result.Data=$mem$Result