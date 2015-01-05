#v1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Return = @{Data = "OK"; Name="Minimum free physical memory"; Text= "More than 256 MB of physical memory is available"}$wmi = Get-WMIObject Win32_OperatingSystem -Property "FreePhysicalMemory","FreeVirtualMemory"[int]$memFreePhyscial = $wmi.FreePhysicalMemory #in KB[int]$memFreeVirtual = $wmi.FreeVirtualMemory #in KB$memFreePhyscial=$memFreePhyscial/1024 #now in MB$memFreeVirtual=$memFreeVirtual/1024 #now in MB#Debug#$memFreePhyscial=10#$memFreeVirtual#If less than 256MB are available, report Majorif ($memFreePhyscial -lt 256) {   $Return.Data="Fail"   $Return.Text="There are only $memFreePhyscial MB of free physical memory available"}$Return