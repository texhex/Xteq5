#Version 1.01
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'

$Result = @{Name="IE_Version"; Data = "n/a"; Text = "Version of Internet Explorer installed"}
 # IE Version in the registry, see https://support.microsoft.com/kb/969393
 # First check svcVersion, then Version. If svcVersion exists its the property we need to use
 $ver_string=""
 $regkey=Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Internet Explorer" -Name "svcVersion" -ErrorAction Ignore  
 
 if ($regkey -ne $null) {
     $ver_string = $regkey.svcVersion  
 } else {
     
     #No svcVersion found. Use the old version path  
     $regkey=Get-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Internet Explorer" -Name "Version" -ErrorAction Ignore 
     if ($regkey -ne $null) {
         $ver_string = $regkey.Version
     }

 }

 
 #Check if we have something we can work with
 if ($ver_string) {        [version]$ver=$ver_string    $Result.Data=$ver }$Result