#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$myReturn = @{Data = "OK"; Text = "Test #023 - Success. This is a very long text because I needed this for testing. Lorem ipsum dolor sit amet, consectetur adipiscing elit. ullam faucibus odio ac arcu finibus pellentesque. Donec odio lectus, varius a semper vel, aliquet at leo. "}$myReturn