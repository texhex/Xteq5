#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$myReturn = @{Name="Value from Asset using Helper function"; Data = "Major"; Text= "An asset named FromAsset550 exists and could be retrieved with a different upper/lower case name"}$valueasset=Get-XQAssetValue "fromASSet550" $false #should return TRUEif ($valueasset -eq $true) {   $myReturn.Data="OK"; #This means Success}else { #Something didn't work out $myReturn.Data="fail" $myReturn.Text="Result was not TRUE!"}$myReturn