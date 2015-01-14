#v1.02
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'
 $test = Get-ItemProperty -Path "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced" -Name "HideFileExt" -ErrorAction Ignore 
 
 if($test -ne $null) {
   if ($test.HideFileExt -eq 0) {
       $Result.Data="OK" 
       $Result.Text="Windows displays file extensions"
   }
 }

 
