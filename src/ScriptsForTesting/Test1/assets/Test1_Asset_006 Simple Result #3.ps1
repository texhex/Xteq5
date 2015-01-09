#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


#This will set .Name which sets the name of this asset directly (instead of using the *.ps1 file name)$myReturn = @{Name = "Asset Name #006 from script"; Data = "Asset #006 Data"; Text = "Asset #006 Text"}
$myReturn