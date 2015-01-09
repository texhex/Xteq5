#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


#This will set this asset to "Does Not Apply" / "Not Applicable" as .Data is an empty string $result = @{Name = "Asset #105 Name"; Data = ""; Text = "Asset #105 Text"}$result