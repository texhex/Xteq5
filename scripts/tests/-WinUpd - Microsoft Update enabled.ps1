#v1.03
#https://github.com/texhex/testutil/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "Fail"; Name="Microsoft Update enabled"; Text= "Microsoft Update is not enabled, you only receive update for Windows"}
#References:
#http://blogs.technet.com/b/danbuche/archive/2010/01/07/enabling-and-disabling-microsoft-update-in-windows-7-via-script.aspx
#http://morgansimonsen.wordpress.com/2013/01/15/how-to-opt-in-to-microsoft-update-with-powershell/
#http://msdn.microsoft.com/en-us/library/windows/desktop/aa826676%28v=vs.85%29
#https://github.com/mwrock/boxstarter/blob/master/BoxStarter.Common/Get-IsMicrosoftUpdateEnabled.ps1


$serviceManager = New-Object -ComObject Microsoft.Update.ServiceManager -Strict 

foreach ($service in $serviceManager.Services) {
   if($service.Name -eq "Microsoft Update") {
      $Result.Data="OK"
      $Result.Text="Your computer receives updates for other Microsoft products, not just for Windows"
      break;
   }
}

 
$Result