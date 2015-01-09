#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$daReturn = @{Data = ""; Text = "Asset #605 Using a module"}

$daReturn.Data=Get-StringFromAModule "And this is a string from the asset"
$daReturn