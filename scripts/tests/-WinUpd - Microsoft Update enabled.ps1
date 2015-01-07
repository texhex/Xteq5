#v1.04
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "n/a"; Name="Microsoft Update enabled"; }
#References:
#http://blogs.technet.com/b/danbuche/archive/2010/01/07/enabling-and-disabling-microsoft-update-in-windows-7-via-script.aspx
#http://morgansimonsen.wordpress.com/2013/01/15/how-to-opt-in-to-microsoft-update-with-powershell/
#http://msdn.microsoft.com/en-us/library/windows/desktop/aa826676%28v=vs.85%29
#https://github.com/mwrock/boxstarter/blob/master/BoxStarter.Common/Get-IsMicrosoftUpdateEnabled.ps1


$serviceManager = New-Object -ComObject Microsoft.Update.ServiceManager -Strict 

#A managed machine will have the entry "Windows Server Update Service" with IsManaged=true in it.
#In this case, Microsoft Update might or might not be enabled.

$WSUS=$serviceManager.Services | ? Name -eq "Windows Server Update Service"
$WSUSmanaged=$false

if ($WSUS -ne $null) {
   write-output "Windows Server Update Service detected"
   $WSUSmanaged=$WSUS.IsManaged
}

if ($WSUSmanaged -eq $true) {
   #computer is managed. Leave to n/a
   $Result.Data="n/a"
   $Result.Text="Windows Update is managed by your administrator"
} else {
   #search for Microsoft Update
   $msupd=$serviceManager.Services | ? Name -eq "Microsoft Update"

   if ($msupd -ne $null) {
      $Result.Data="OK"
      $Result.Text="Your computer receives updates for other Microsoft products, not just for Windows"
   } else {
      $Result.Data="Fail"
      $Result.Text= "Microsoft Update is not enabled, you only receive update for Windows"
   }
  
}


$Result