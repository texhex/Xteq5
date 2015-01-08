#Version 1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


$Result = @{Data = ""; Name = "Xteq5_Version"; Text = "Version of Xteq5 engine used"}
$Result.Data=$Xteq5EngineVersion$Result