#v1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$wmi = Get-WMIObject Win32_OperatingSystem -Property "SystemDrive"$sysdrive=$wmi.SystemDrive"SysDrive is $sysdrive"#Because we need the module storage that is maybe not available, set to n/a by default$Result = @{Name="Operating System drive is healthy (WSMP)"; Data = "n/a"; Text = "Windows Storage Management Provider is not supported on this computer"}#check if module Storage is available
if (Test-MPXModuleAvailable "Storage") {
   
   $Result.Text="Drive ($sysdrive) is, according to Windows Storage Management Provider, healthy"

   #Get a list of drive to partition to volume to know on which physical drive the sysdrive is located
   $info=Get-MPXDrivePartionVolume | where-object {$_.Volume_DeviceID -eq $sysdrive}   

   if ($info) {
      Write-Output "Physical drive for $sysdrive is: " $info.DiskDrive_DeviceID
      write-output "Disk name: " $info.DiskDrive_Caption
      
      #Now we need Get-PhysicalDisk, but because the output of WMI and CIM are different (PhyiscalDrive0 vs. PhysicalDisk)
      #the only property we can use is SerialNumber
      $serial=$info.DiskDrive_SerialNumber

      #try to find a phyiscal disk with this s/N
      $physicaldisk=Get-PhysicalDisk | Where-Object {$_.SerialNumber.Trim() -eq $serial}

      if ($physicaldisk) {
         write-output "Found matching physical disk as: " $physicaldisk.FriendlyName

         $status=$physicaldisk.HealthStatus

         #Finally! 
         if ($status -eq "Healthy") {
            $Result.Data="OK"
         } else {
            $Result.Data="Major"
            $Result.Text = "Drive $sysdrive has status [$status] (reported by Windows Storage Management Provider)" 

         }
      }
   }


} $Result