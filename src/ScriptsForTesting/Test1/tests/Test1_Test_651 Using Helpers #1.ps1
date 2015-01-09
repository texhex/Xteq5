#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$myReturn = @{Name="Value from Asset using Helper function"; Data = "Major"; Text= "Running in Xteq5 checked using Test-XQActive"}if (Test-XQActive) {        $myReturn.Data="OK"; #This means Success}else { #we are not running in Xteq5, hence $Xteq5Assets does not exist $myReturn.Data="Fail"}$myReturn