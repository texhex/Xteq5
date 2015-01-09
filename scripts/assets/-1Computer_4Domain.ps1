#Version 1.02
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Data = ""; Name = "Computer_Domain"} #Text = "Computer is not member of a domain"
$wmi = Get-WMIObject Win32_ComputerSystem -Property "PartOfDomain, Domain"#Only list the domain if the computer is member of a domain, not for workgroupsif ($wmi.PartOfDomain) {   $Result.Data=$wmi.Domain   $Result.Text="NetBIOS name of the domain the computer is member of"}$Result