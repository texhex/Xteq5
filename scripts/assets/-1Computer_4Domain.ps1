#Version 1.01
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "Computer_Domain"; Text = "NetBIOS name of the domain the computer is member of"}
$wmi = Get-WMIObject Win32_ComputerSystem -Property "PartOfDomain, Domain"#Only list the domain if the computer is member of a domain, not for workgroupsif ($wmi.PartOfDomain) {   $Result.Data=$wmi.Domain}$Result