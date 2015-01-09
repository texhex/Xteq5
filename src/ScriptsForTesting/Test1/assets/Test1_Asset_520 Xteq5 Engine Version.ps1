#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name = "Xteq5Engine_Version"; Data = ""; Text = "Asset #520: Version of Xteq5"}
$Result.Data=$Xteq5EngineVersion$Result