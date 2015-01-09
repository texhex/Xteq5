#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#another possibility to use a hashtable[hashtable]$Return = @{} $Return.Data="Asset #008 Data"$Return.Text="Asset #008 Text"#send it to outputwrite-output $Return 