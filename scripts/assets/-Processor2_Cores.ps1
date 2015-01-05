#Version 1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="Processor_Cores"; Data = ""; Text = "Number of logical cores the processor (CPU) offers"}
$wmi = Get-WMIObject Win32_Processor -Property "NumberOfLogicalProcessors"[int]$cores= $wmi.NumberOfLogicalProcessors$Result.Data=$cores$Result