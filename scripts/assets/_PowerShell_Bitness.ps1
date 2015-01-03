#Version 1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#Set .Data to "" - this means "Does not apply" $Result = @{Data = ""; Name = "PowerShell_Bitness"; Text = "Bitness (x86 or x64) of PowerShell used"}
#Side note: [Environment]::Is64BitProcess might also be an optionSwitch ([System.IntPtr]::Size) {       4 {           $Result.Data = "32bit"        }                 8 {           $Result.Data = "64bit"        }}$Result