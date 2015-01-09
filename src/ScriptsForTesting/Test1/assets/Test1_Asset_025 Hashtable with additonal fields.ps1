#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


$result = @{Name = "Asset #025 Name"; Data = "Asset #025 Data"; Text = "Asset #025 Text"}#Now add some fields to this hashtable$result.MyAdditonalField1 = "Some data"$result.VersionOfPSObject = [PSObject].Assembly.GetName().Version$result.___FirstField ="First!"
#Xteq5 will process this result without problems because it ignores the additional fields$result