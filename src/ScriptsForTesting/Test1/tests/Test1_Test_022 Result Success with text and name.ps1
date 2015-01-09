#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$myReturn = @{Name ="Test 022"; Data = "Success"; Text = "Test #022 - Success with text and name"}$myReturn