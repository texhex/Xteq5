#Version 1.03
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#Set .Data to "" - this means "Does not apply" $Result = @{Name="OS_Bitness"; Data = ""; Text = "Bitness of the operationg system"}
if ([Environment]::Is64BitOperatingSystem) {   $Result.Data = "64bit"} else {   $Result.Data= "32bit"}$Result