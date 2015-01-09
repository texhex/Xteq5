#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#Set .Data to "" - this means "Does not apply" $Result = @{Name = "PowerShell_Bitness"; Data = ""; Text = "Asset 505: Bitness (x86 or x64) of PowerShell used"}
#Side note: [Environment]::Is64BitProcess might also be an optionSwitch ([System.IntPtr]::Size) {       4 {           $Result.Data = "32-bit"        }                 8 {           $Result.Data = "64-bit"        }}$Result