#v1.03
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "OK"; Name="More than 256 MB of free physical memory";}$wmi = Get-WMIObject Win32_OperatingSystem -Property "FreePhysicalMemory","FreeVirtualMemory"[int]$memFreePhyscial = $wmi.FreePhysicalMemory #in KB[int]$memFreeVirtual = $wmi.FreeVirtualMemory #in KB"Free memory in KB: $memFreePhyscial"$memFreePhyscial_human=Get-MPXHumanizeBytes ($memFreePhyscial*1024) #because the value is in KB#append the freespace to the text of this test$Result.Text="$memFreePhyscial_human of free phyisical memory available"$memFreePhyscial=$memFreePhyscial/1024 #now in MB$memFreeVirtual=$memFreeVirtual/1024 #now in MB"Free memory in MB: $memFreePhyscial"#Debug#$memFreePhyscial=10#$memFreeVirtual#If less than 256MB are available, report Majorif ($memFreePhyscial -lt 256) {   $Result.Data="Fail"}$Result