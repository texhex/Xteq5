#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


#Returns the keys of the hashtable with mixed case - but this works also$myReturn = @{name = "Asset #015 Name"; daTA = "Asset #015 Data"; teXt = "Asset #015 Text"}
$myReturn