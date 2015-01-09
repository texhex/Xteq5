#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


#Create the result hashtable$Result = @{Name="FromAsset550"; Data= ""; Text="Bool value that will used by Test #650"}
#Create an bool
[boolean]$b=$true#Set .Data to this object
$Result.Data=$b


$Result
