#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#Create the result hashtable$Result = @{Data= ""; Text="Data is int"}
#Create an int
[int]$i=42042#Set .Data to this object
$Result.Data=$i



$Result
