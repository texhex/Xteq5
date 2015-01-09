#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


#Returns a hashtable but with with a misspelled ".Data" - this will result in an error$wrongReturn = @{Name = "Asset #910 Name";  Daataa = "Asset #910 Data"; Text = "Asset #910 Additional Text"}
$wrongReturn