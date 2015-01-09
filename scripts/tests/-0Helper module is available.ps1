#v1.06
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$Result = @{Data = "Major"; Name="Xteq5 helper module available"; Text= "Function Test-XQActive from module Xteq5Helpers is available"}if (Test-XQActive) {       $Result.Data="OK"; #As we are running in Xteq5, set the result to Success by using the alias "OK"}else {   #we are not running in Xteq5   $Result.Data="Fail"}$Result