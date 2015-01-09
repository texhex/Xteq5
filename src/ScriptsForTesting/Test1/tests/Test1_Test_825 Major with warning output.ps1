#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$myReturn = @{Data = "Major"; Text = "Test #825 This test should include some warning output"}Write-Warning "This text was generated using write-warning"Write-Warning "Another warning message"$myReturn