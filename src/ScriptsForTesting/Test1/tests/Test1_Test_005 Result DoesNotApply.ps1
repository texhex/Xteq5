#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Shortest version possible. Just the data, no name and no description#Because Data is empty, this test will be displayed as "Does not apply"$myReturn = @{Data = ""}$myReturn