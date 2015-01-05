#Version 1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$wmi = Get-WMIObject Win32_OperatingSystem -Property "SystemDrive"$sysdrive=$wmi.SystemDrive"SysDrive is $sysdrive"#Because we need the module storage that is maybe not available, set to n/a by default$Result = @{Name="OSDrive_IsSSD"; Data = "n/a"; Text = "Windows Storage Management Provider is unfortunately not supported on this computer"}
#check if module Storage is available
if (Test-MPXModuleAvailable "Storage") {
   
   $Result.Text = "Is the operating system drive ($sysdrive) a Solid State Drive (SSD)"

   #Get a list of drive to partition to volume to know on which physical drive the sysdrive is located
   $info=Get-MPXDrivePartionVolume | where-object {$_.Volume_DeviceID -eq $sysdrive}   

   if ($info) {
      Write-Output "Physical drive for $sysdrive is: " $info.DiskDrive_DeviceID
      write-output "Disk name: " $info.DiskDrive_Caption
      
      #Now we need Get-PhysicalDisk, but because the output of WMI and CIM are different (PhyiscalDrive0 vs. PhsicalDisk)
      #the only property we can use is SerialNumber
      $serial=$info.DiskDrive_SerialNumber

      #try to find a phyiscal disk with this s/N
      $physicaldisk=Get-PhysicalDisk | Where-Object {$_.SerialNumber.Trim() -eq $serial}

      if ($physicaldisk) {
         write-output "Found matching physical disk as: " $physicaldisk.FriendlyName

         #Finally! 
         if ($physicaldisk.SpindleSpeed -eq 0) {
            $Result.Data=$true
         } else {
            $Result.Data=$false
         }
      }
   }


} 
$Result