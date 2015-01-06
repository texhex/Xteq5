#v1.02
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Because we need the module storage that is maybe not available, set to n/a by default$Result = @{Name="All disk drives are healthy (WSMP)"; Data = "n/a"; Text = "Windows Storage Management Provider is not supported on this computer"}#check if module Storage is available
if (Test-MPXModuleAvailable "Storage") {
   
   $Result.Data="OK"
   $Result.Text="All drives are healthy according to Windows Storage Management Provider (WSMP)"

   #Retrieve a list of disks where HealthStatus is not Healthy.
   #We will only use the first non-healthy drive      
   $phydisk=Get-PhysicalDisk | Where-Object { $_.HealthStatus -ne "Healthy"} | Select-Object -first 1


   #DEBUG!
   ##$phydisk=Get-PhysicalDisk | Where-Object { $_.HealthStatus -ne "xxxx"} | Select-Object -first 1


   if ($phydisk) {
      #we found a drive that is not healthy
      $Result.Data="Fail"

      [string]$diskname=$phydisk.FriendlyName
      [string]$serial=$phydisk.SerialNumber.Trim()
      [string]$status=$phydisk.HealthStatus

      write-output "Non healthy drive: $diskname"            
      write-output "Serial: $serial" 
      write-output "Status: $status" 

      #because the info isn't good at all, try to find a volume that uses this physical disk
      $vols=Get-MPXDrivePartionVolume 
      $vol= $vols | Where-Object { $_.DiskDrive_SerialNumber -eq $serial -and $_.Volume_FS -ne "" } | Select-Object -first 1

      if (-not $vol) {
         #We did not find a entry that contains a volume, great. So use just the drive and hope the user can read the data
         $vol=$vols| Where-Object { $_.DiskDrive_SerialNumber -eq $serial } | Select-Object -first 1
      }

      #Now hopefully we have some data
      if ($vol) {
         [string]$volname=$vol.DiskDrive_Caption 
         [string]$driveletter=$vol.Volume_DeviceID
         $Result.Text="Windows Storage Management Provider reports status [$status] for $volname ($driveletter) - S/N $serial"
      } else {
        #no volume or drive data found. Output what we have
        $Result.Text="Windows Storage Management Provider reports status [$status] for $diskname (S/N $serial)"
      }
   }
   
}$Result