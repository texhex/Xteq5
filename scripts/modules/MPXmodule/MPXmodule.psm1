# Michael PowerShell eXtension module
# Version 1.08
#
# Copyright © 2010-2015 Michael 'Tex' Hex 
# Licensed under the Apache License, Version 2.0. 
# For details, please see licenses/LICENSE.txt.
#
#
# **THIS FILE WILL BE OVERWRITTEN WITHOUT QUESTION. DO NOT ADD YOUR OWN FUNCTIONS HERE.**
#
# Before adding a new function, please see
# [Approved Verbs for Windows PowerShell Commands](http://msdn.microsoft.com/en-us/library/ms714428%28v=vs.85%29.aspx)


#require -version 4.0


function Test-MPXWoW {
<#
  .SYNOPSIS
  Returns $true if the script is affected by WoW64

  .PARAMETER 
  None

  .OUTPUTS
  $true if the script is virtualized with WoW64, $false otherwise
#>
 
 #Better set strict mode on function scope than on module level
 Set-StrictMode -version 2.0

 if ([Environment]::Is64BitOperatingSystem) {
    if ([Environment]::Is64BitProcess) {
       #We are running as 64 bit process on a 64 bit OS. No WOW.
       return $false
    } else {
       #We are running as a 32 bit process on a 64 bit OS - WoW is automatically enabled
       return $true 
    }    
 } else {
   #WoW64 is only active on a 64 bit OS 
   return $false
 }
}


function Test-OS32bit {
<#
  .SYNOPSIS
  Returns $true if Windows is a 32 bit version

  .PARAMETER 
  None

  .OUTPUTS
  Returns $true if Windows is 32 bit, $false otherwise
#>
 
 #Better set strict mode on function scope than on module level
 Set-StrictMode -version 2.0

 if ([Environment]::Is64BitOperatingSystem) {
    return $false
 } else {
   return $true
 }
}


function Test-OS64bit {
<#
  .SYNOPSIS
  Returns $true if Windows is a 64 bit version

  .PARAMETER 
  None

  .OUTPUTS
  Returns $true if Windows is 64 bit, $false otherwise
#>
 
 return [Environment]::Is64BitOperatingSystem
}


#Helper function for [string]::IsNullOrWhiteSpace - http://msdn.microsoft.com/en-us/library/system.string.isnullorwhitespace%28v=vs.110%29.aspx
<#
 write-host "Tests for Test-MPXStringIsNullOrWhiteSpace"
 Test-MPXStringIsNullOrWhiteSpace "a" #False
 Test-MPXStringIsNullOrWhiteSpace "" #true
 Test-MPXStringIsNullOrWhiteSpace " " #true
 Test-MPXStringIsNullOrWhiteSpace "     " #true
 Test-MPXStringIsNullOrWhiteSpace $null #true
#> 
function Test-MPXStringIsNullOrWhiteSpace {
<#
  .SYNOPSIS
  Returns $true if the string is either $null, empty, or consists only of white-space characters.

   .PARAMETER string
  The string value to be checked

  .OUTPUTS
  $true if the string is either $null, empty, or consists only of white-space characters, $false otherwise
#>
  param (
 [Parameter(Mandatory=$True,Position=1)]
 [AllowEmptyString()] #we need this or PowerShell will complain "Cannot bind argument to parameter 'string' because it is an empty string." 
 [string]$string
 )


 if ([string]::IsNullOrWhiteSpace($string)) {
    return $true
 } else {
    return $false
 }
 

}


<#
 write-host "Tests for Test-MPXStringHasData"
 Test-MPXStringHasData "a" #true
 Test-MPXStringHasData "" #false
 Test-MPXStringHasData " " #false
 Test-MPXStringHasData "   " #false
 Test-MPXStringHasData $null #false
#>
function Test-MPXStringHasData {
<#
  .SYNOPSIS
  Returns $true if the string contains data

   .PARAMETER string
  The string value to be checked

  .OUTPUTS
  $true if the string is not $null, empty, or consists only of white-space characters, $false otherwise
#>
  param (
 [Parameter(Mandatory=$True,Position=1)]
 [AllowEmptyString()] #we need this or PowerShell will complain "Cannot bind argument to parameter 'string' because it is an empty string." 
 [string]$string
 )


 if (-Not (Test-MPXStringIsNullOrWhiteSpace $string)) {
    return $true
 } else {
    return $false
 }
 

}


#The verb "Humanize" is taken from this great project: [Humanizer](https://github.com/MehdiK/Humanizer)
#Idea from [Which Disk is that volume on](http://www.uvm.edu/~gcd/2013/01/which-disk-is-that-volume-on/) by Geoff Duke 
function Get-MPXHumanizeBytes {
<#
  .SYNOPSIS
  Returns a string optimized for readability 

   .PARAMETER bytes
  The value of bytes that should be returned as humanized string

  .OUTPUTS
  A humanized string that is rounded (no decimal points) and optimized for readability. 1024 becomes 1kb, 179111387136 will be 167 GB etc. 
#>
  param (
 [Parameter(Mandatory=$True,Position=1)]
 [AllowEmptyString()] #we need this or PowerShell will complain "Cannot bind argument to parameter 'string' because it is an empty string." 
 [int64]$bytes
 )

 #Better set strict mode on function scope than on module level
 Set-StrictMode -version 2.0

 #Original code was :N2 which means "two decimal points"
 if     ( $bytes -gt 1pb ) { return "{0:N0} PB" -f ($bytes / 1pb) }
 elseif ( $bytes -gt 1tb ) { return "{0:N0} TB" -f ($bytes / 1tb) }
 elseif ( $bytes -gt 1gb ) { return "{0:N0} GB" -f ($bytes / 1gb) }
 elseif ( $bytes -gt 1mb ) { return "{0:N0} MB" -f ($bytes / 1mb) }
 elseif ( $bytes -gt 1kb ) { return "{0:N0} KB" -f ($bytes / 1kb) } 
 else   { return "{0:N0} Bytes" -f $bytes } 

}



#$Test_MPXModuleAvailable_Cache=$null

Function Test-MPXModuleAvailable {
<#
  .SYNOPSIS
  Returns $true if the module exist

   .PARAMETER name
  The name of the module to be checked

  .OUTPUTS
  $true if the module is available, $false if not
#>
 param(
  [Parameter(Mandatory=$True,Position=1)]
  [ValidateNotNullOrEmpty()]
  [string]$name
 )

 Set-StrictMode -version 2.0

 #First check if the requested module is already available in this session
 if(-Not (Get-Module -name $name))
 {
     #The correct way would be to now use Get-Module -ListAvailable like this:

     #Creating the list of available modules takes some seconds. 
     #Therfore use a cache on module level.
     #if ($script:Test_MPXModuleAvailable_Cache -eq $null) {
     #   $script:Test_MPXModuleAvailable_Cache=Get-Module -ListAvailable | ForEach-Object {$_.Name}
     #}

     ##if(Get-Module -ListAvailable | Where-Object { $_.name -eq $name })
     #if ($Test_MPXModuleAvailable_Cache -contains $name)     
     #{
     #  #module is available and will be loaded by PowerShell when requested
     #  return $true
     #} else { 
     #  #this module is not available
     # return $false      
     #}

     #However, this function is a performance killer as it reads every cmdlet, dll, and whatever
     #from any module that is available. 
     #
     #Therefore we will simply try to import the module using import-module on local level 
     #and then return if this has worked. This way, only the module requested is fully loaded.
     #Since we only load it to the local level, we make sure not to change the calling level
     #if the caller does not want that module to be loaded. 
     #
     #Given that the script (that has called us) will the use a cmdlet from the module,
     #the module is already loaded in the runspace and the next call to this function will be
     #a lot faster since get-module will then return $TRUE.

     $mod=Import-Module $name -PassThru -ErrorAction SilentlyContinue -Scope Local
     if ($mod -ne $null) {
        return $true
     } else {
        return $false
     }


 } else { 
   #module is already available in this runspace
   return $true 
 }

} 


#See http://msdn.microsoft.com/en-us/library/aa394592%28v=vs.85%29.aspx#and http://www.uvm.edu/~gcd/2013/01/which-disk-is-that-volume-on/Function Get-MPXDrivePartionVolume {<#
  .SYNOPSIS
  Returns a collection of objects about drive/partition/volume information

  .PARAMETER 
  None

  .OUTPUTS
  A flat collection of object containing the relationsship of disk drives, partitions and volumes
#>        Set-StrictMode -version 2.0    #generate the template object    $oTemplate = New-Object System.Object     $oTemplate | Add-Member -MemberType NoteProperty -Name "DiskDrive_DeviceID" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "DiskDrive_Caption" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "DiskDrive_Interface" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "DiskDrive_SerialNumber" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "DiskDrive_Status" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "DiskDrive_Size" -Value 0     $oTemplate | Add-Member -MemberType NoteProperty -Name "Partition_DeviceID" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "Partition_IsPrimary" -Value $false    $oTemplate | Add-Member -MemberType NoteProperty -Name "Partition_Size" -Value 0
    $oTemplate | Add-Member -MemberType NoteProperty -Name "Volume_DeviceID" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "Volume_Name" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "Volume_FS" -Value ""     $oTemplate | Add-Member -MemberType NoteProperty -Name "Volume_Size" -Value 0            $diskdrives = Get-WMIobject Win32_DiskDrive | Sort-Object DeviceID    foreach ($disk in $diskdrives) {     #debug output    #write-host "Working on disk " $disk.DeviceID    #$disk | fl *    #$oDisk = New-Object System.Object     $oDisk = $oTemplate | select *     $oDisk.DiskDrive_DeviceID = $disk.DeviceID    $oDisk.DiskDrive_Caption = $disk.Caption.Trim()    $oDisk.DiskDrive_Interface = $disk.InterfaceType    $oDisk.DiskDrive_SerialNumber = $disk.SerialNumber.Trim()    $oDisk.DiskDrive_Status = $disk.Status.Trim()    $oDisk.DiskDrive_Size = $disk.Size     #Serial $disk.SerialNumber.Trim()      #BytesPerSector $disk.BytesPerSector "     #get the partions on the physical drive     $partions_query = 'ASSOCIATORS OF {Win32_DiskDrive.DeviceID="' + $disk.DeviceID.replace('\','\\') + '"} WHERE AssocClass=Win32_DiskDriveToDiskPartition'     $partitions = @( Get-WMIObject -query $partions_query | Sort-Object StartingOffset )     $bPartitionsFound=$false     #Partions on disks     foreach ($partition in $partitions) {       #debug output           #write-host "Working on partition" $partition.DeviceID       #$partition | fl *       #we found at least one partition       $bPartitionsFound=$true             #Clone the data from the current oDisk to the new oPartition object       $oPartition = $oDisk | select *        $oPartition.Partition_DeviceID = $partition.DeviceID       $oPartition.Partition_IsPrimary = $partition.PrimaryPartition       $oPartition.Partition_Size = $partition.Size       #get the volumes on the partion        $volumes_query = 'ASSOCIATORS OF {Win32_DiskPartition.DeviceID="' + $partition.DeviceID +  '"} WHERE AssocClass=Win32_LogicalDiskToPartition'          $volumes = @(Get-WMIObject -query $volumes_query | Sort-Object Name)       $volumesfound=$false   
       foreach ($volume in $volumes) {
           
          #debug output
          #write-host "Working on volume" $volume.Name
          #$volume | fl *

          #we found a volume
          $volumesfound=$true

          #create a clone from the current partion object to add our own properties
          $oVolume = $oPartition | select *       

          $oVolume.Volume_DeviceID = $volume.DeviceID          $oVolume.Volume_Name = $volume.VolumeName          $oVolume.Volume_FS = $volume.FileSystem          $oVolume.Volume_Size = $volume.Size      
          write-output $oVolume
       }        if (-Not ($volumesfound)) {          # If no volumes were found, add the current partion object                    write-output $oPartition       }     }     if (-Not ($bPartitionsFound)) {        #No partions were found, add the current disk object        write-output $oDisk     }     }        }