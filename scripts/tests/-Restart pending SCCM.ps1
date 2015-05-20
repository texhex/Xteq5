#v1.04
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors $ErrorActionPreference = 'Stop'$Result = @{Data = "n/a"; Name="No SCCM restart pending"; Text= "No pending changes by SCCM, that require a restart, have been detected"}#Code from this location: https://gallery.technet.microsoft.com/scriptcenter/Get-PendingReboot-Query-bdb79542# Determine SCCM 2012 Client Reboot Pending Status
# To avoid nested 'if' statements and unneeded WMI calls to determine if the CCM_ClientUtilities class exist, setting EA = 0
$CCMClientSDK = $null

$wmiOptions = @{
    NameSpace    = 'ROOT\ccm\ClientSDK'
    Class        = 'CCM_ClientUtilities'
    Name         = 'DetermineIfRebootPending'
    ErrorAction  = 'SilentlyContinue'
}

$CCMClientSDK = Invoke-WmiMethod @wmiOptions

if ($CCMClientSDK) 
{
    if ($CCMClientSDK.ReturnValue -ne 0) 
    {
        #If we have found the WMI class, the call should be OK
        "SCCM DetermineIfRebootPending method returned error code $($CCMClientSDK.ReturnValue)"

    } else {
      
      if ($CCMClientSDK.IsHardRebootPending) {
         $Result.Data="Major"          $Result.Text = "A restart (hard reboot) is required in order to finish an operation"
      }

      if ($CCMClientSDK.RebootPending) {
         $Result.Data="Major"          $Result.Text = "A restart is required in order to finish an operation"
      }


    }

}
else 
{
    #SCCM Not found
}    


$Result