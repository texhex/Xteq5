# Xteq5Helpers module
# Version 1.11
#
# Copyright © 2010-2015 Michael Hex 
# Licensed under the Apache License, Version 2.0. 
# For details, please see licenses/LICENSE.txt.
#
#
# **THIS FILE WILL BE OVERWRITTEN WITHOUT QUESTION. DO NOT ADD YOUR OWN FUNCTIONS HERE.**
#
# Before adding a new function, please see
# [Approved Verbs for Windows PowerShell Commands](http://msdn.microsoft.com/en-us/library/ms714428%28v=vs.85%29.aspx)


#requires -version 4.0


function Test-XQActive {
<#
  .SYNOPSIS
  Return $true if the script is running within Xteq5

  .PARAMETER 
  None

  .OUTPUTS
  $true if the script is running within Xteq5, $false otherwise
#>
 
 #Better set strict mode on function scope than on module level
 Set-StrictMode -version 2.0

 if (test-path variable:Xteq5Active) {
    return $true 
 } else {
    return $false
 }
}



function Get-XQAssetValue {
<#
  .SYNOPSIS
  Return the value of an asset or the provided default value 

  .DESCRIPTION
  Return the value of an asset or the provided default value.

  The default value is returned when the script is not executed within Xteq5 or the asset does not exist.

  .PARAMETER AssetName
  The name of the asset that should be returned 

  .PARAMETER DefaultValue
  The value that should be returned if the asset could not be found or Xteq5 is not active

  .OUTPUTS
  Value of the asset or the default value

  .EXAMPLE
  $myValue = Get-XQAssetValue "IsOfficeInstalled" $false
  
  $myValue = Get-XQAssetValue "TotalSuperInstallPath" "C:\TotalSuper"
#>
 param (
 [Parameter(Mandatory=$True,Position=1)]
 [ValidateNotNullOrEmpty()]
 [string]$AssetName, 
 
 [Parameter(Mandatory=$True,Position=2)]
 $DefaultValue
 )

 #Better set strict mode on function scope than on module level to avoid side effects
 Set-StrictMode -version 2.0

  if (Test-XQActive) {
     if ($Xteq5Assets.ContainsKey($AssetName)) {
        #return the value of the key
        return $Xteq5Assets[$AssetName]
     }
     else {
        #hastable does not contain a key with the given name
        return $DefaultValue
     } 
 } 
 else {
    #Xteq5 is not active
    return $DefaultValue
 }

 #Done
}

