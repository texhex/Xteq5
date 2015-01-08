#Version 1.03
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


#Set .Data to "" - this means "Does not apply" $Result = @{Name="OS_Is64Bit"; Data = ""; Text = "Is the operation system 64 bit or not"}
if ([Environment]::Is64BitOperatingSystem) {   $Result.Data = $true} else {   $Result.Data= $false}$Result