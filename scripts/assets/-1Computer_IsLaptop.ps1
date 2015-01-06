#Version 1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="IsLaptop"; Data = $false; Text = "Is this computer a laptop"}


$isLaptop = $false


#Orginal code from http://blogs.technet.com/b/heyscriptingguy/archive/2010/05/15/hey-scripting-guy-weekend-scripter-how-can-i-use-wmi-to-detect-laptops.aspx
if(Get-WMIObject -Class Win32_SystemEnclosure -Property "ChassisTypes" | Where-Object { $_.chassistypes -eq 9 -or $_.chassistypes -eq 10 -or $_.chassistypes -eq 14})  { 
   $isLaptop = $true    
} else {
   if(Get-WMIObject -Class Win32_Battery -Property "BatteryStatus" ) { 
      $isLaptop = $true 
   }
 }

$Result.Data = $isLaptop$Result