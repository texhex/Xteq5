#v1.02

#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$Return = @{Data = "Major"; Name="TestUtil module available"; Text= "Function Test-TUActive from TestUtilHelpers is available"}if (Test-TUActive) {       $Return.Data="OK"; #As we are running in TestUtil, set the result to Success by using the alias "OK"}else {   #we are not running in TestUtil   $Return.Data="Not running in TestUtil"}$Return