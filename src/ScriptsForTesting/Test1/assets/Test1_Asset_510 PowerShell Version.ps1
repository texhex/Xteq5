#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name = "PowerShell_Version"; Data = ""; Text = "Asset #510 Version of PowerShell used"}
$Result.Data=$PSVersionTable.PSVersion$Result