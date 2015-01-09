#Version 1.05
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="OS_Name"; Data = ""; Text = "Product name of the operationg system"}
#Hey, if you want to retrieve this value by the most complicated way possible, use this code!#selffacepalm#$wmi = Get-WMIObject Win32_OperatingSystem -Property "Name"#[string]$rawname = $wmi.Name#$options = [System.StringSplitOptions]::RemoveEmptyEntries#$ary=$rawname.Split([char]0X007C, $options) #split on "|" and ignore empty entries#[string]$name = $ary[0]##Replace "Microsoft" inside the name. I guess everybody knows who made it#$name = $name -replace "Microsoft ", ""#$name = $name -replace "Microsoft", ""$wmi = Get-WMIObject Win32_OperatingSystem -Property "Caption"[string]$name = $wmi.Caption$name=$name.Trim()$Result.Data=$name$Result