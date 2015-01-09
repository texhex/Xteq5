#Version 1.03
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


$Result = @{Data = ""; Name = "Xteq5_Version"; Text = "Version of Xteq5 used"}
$Result.Data=$Xteq5EngineVersion$Result