#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

#This will set this asset to "Does Not Apply" / "Not Applicable" by setting .Data to NULL$result = @{Name = "Asset #106 Name"; Data = $null; Text = "Asset #106 Text"}$result