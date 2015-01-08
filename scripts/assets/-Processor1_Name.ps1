#Version 1.03
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="Processor_Name"; Data = ""; Text = "Name of the processor (CPU)"}
$wmi = Get-WMIObject Win32_Processor -Property "Name"[string]$name = $wmi.Name##Replace "(R)" and "(TM)" inside the name. $name = $name.Replace("(R)", "")$name = $name.Replace("(TM)", "")$name = $name.Replace("  ", "")#replace any left over double white spaces$name = $name.Trim()$Result.Data=$name$Result