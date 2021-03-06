#v1.12
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "OK"; Name="No changes pending"; Text= "No pending changes that require a restart have been detected"} #Original Code [Determine Pending Reboot Status�PowerShell Style! Part 2](http://blogs.technet.com/b/heyscriptingguy/archive/2013/06/11/determine-pending-reboot-status-powershell-style-part-2.aspx) #by [Brian Wilhite](https://social.technet.microsoft.com/profile/brian%20wilhite/) #Note: PowerShell + Registry = DRAIN BAMAGED! #Get-ChildItem -> Returns the PATHS below the given path #Get-ItemProperty -> Returns the VALUES below the given path  #Check [CBS](http://technet.microsoft.com/en-us/library/cc756291%28v=ws.10%29.aspx) for Windows service packs etc. #This is a single value $cbsrebootpending=$false $test=Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing" -Name "RebootPending" -ErrorAction Ignore  if ($test -ne $null) {
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
     write-output "Windows Session Manager has PendingFileRenameOperations set."
     write-output "Files to be renamed or deleted upon reboot:" 
     write-output $test.PendingFileRenameOperations
     $pendingfilerename=$true
  }     if ($cbsrebootpending -eq $true -or $wsusbootpending -eq $true -or $pendingfilerename -eq $true) {     $Result.Data ="Fail"     $Result.Text = "A restart is required in order to finish an operation"  }
$Result