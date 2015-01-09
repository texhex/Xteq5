#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


$Result = @{Name = "ExecutedFromXteq5"; Data = ""; Text = "Asset #525: Is this script executed with Xteq5 or not"}
if (test-path variable:Xteq5Active) {   #Executed within Xteq5   $Result.Data="Running in Xteq5" } else {   #Not running within Xteq5   $Result.Data= "Xteq5 is nowhere to be seen"}$Result