#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$myReturn = @{Name="Value from Asset Test"; Data = ""; Text= "Does an asset value named [FromAsset550] exist and is it TRUE"}if (test-path variable:Xteq5Active) {   if ($Xteq5Assets.ContainsKey("FromAsset550")) {      #value exists, check the value      if ($Xteq5Assets["FromAsset550"] -eq $true) {          #Value is TRUE, exactly what we expect          $myReturn.Data="OK"; #This means Success      }   }  }else { #we are not running in Xteq5, hence $Xteq5Assets does not exist $myReturn.Data="Fail"}$myReturn