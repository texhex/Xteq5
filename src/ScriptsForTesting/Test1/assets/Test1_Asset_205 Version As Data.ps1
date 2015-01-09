#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#Create the result hashtable$Result = @{Data= ""; Text="Data is System.Version"}
#Create a version object
[version]$v="1.2.3.4"#Set .Data to this version 
$Result.Data=$v


$Result
