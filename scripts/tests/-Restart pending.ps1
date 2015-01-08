#v1.10
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'
     write-output "Component Based Servicing has value RebootPending set"
     $cbsrebootpending = $true
 }


 #Check Windows Update for updates that have triggered a reboot
 #This is a single value 
 $wsusbootpending=$false
 $test = Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\WindowsUpdate\Auto Update" -Name "RebootRequired" -ErrorAction Ignore 
 if($test -ne $null) {
    Write-Output "WindowsUpdate has RebootRequired set"
    $wsusbootpending=$true
 }


  #Check PendingFileRenameOperations
  #WARNING: We need to check if Norton/Symantec still misuses this update as described here: https://discussions.nessus.org/thread/2413
  #         If so, this check might have to go. 
  #Single value, REG_MULTI_SZ
  $pendingfilerename=$false
  $test = Get-ItemProperty "HKLM:SYSTEM\CurrentControlSet\Control\Session Manager" -Name "PendingFileRenameOperations" -ErrorAction Ignore
  if($test -ne $null) {
    write-output "Windows Session Manager has PendingFileRenameOperations set"
    $pendingfilerename=$true
  } 
