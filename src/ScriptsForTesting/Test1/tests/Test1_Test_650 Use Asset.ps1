#Script requires PowerShell 4.0 or higher - http://blogs.msdn.com/b/powershell/archive/2009/02/06/requires-your-scripts.aspx
#require -version 4.0

#Guard against common code errors - http://technet.microsoft.com/en-us/library/ff730970.aspx
Set-StrictMode -version 2.0

#Terminate on errors - http://blogs.technet.com/b/heyscriptingguy/archive/2010/03/08/hey-scripting-guy-march-8-2010.aspx$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$myReturn = @{Name="Value from Asset Test"; Data = "Major"; Text= "Does an asset value named [FromAsset550] exist and is it TRUE"}if (test-path variable:TestUtilActive) {   if ($TestUtilAssets.ContainsKey("FromAsset550")) {      #value exists, check the value      if ($TestUtilAssets["FromAsset550"] -eq $true) {          #Value is TRUE, exactly what we expect          $myReturn.Data="OK"; #This means Success      }   }  }else { #we are not running in TestUtil, hence $TestUtilAssets does not exist $myReturn.Data="Not running in TestUtil..."}$myReturn