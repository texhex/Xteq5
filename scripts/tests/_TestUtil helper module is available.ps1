#Version 1.01

#Script requires PowerShell 4.0 or higher - http://blogs.msdn.com/b/powershell/archive/2009/02/06/requires-your-scripts.aspx
#require -version 4.0

#Guard against common code errors - http://technet.microsoft.com/en-us/library/ff730970.aspx
Set-StrictMode -version 2.0

#Terminate on errors - http://blogs.technet.com/b/heyscriptingguy/archive/2010/03/08/hey-scripting-guy-march-8-2010.aspx$ErrorActionPreference = 'Stop'#Default is MAJOR because we expect the value to exist$myReturn = @{Data = "Major"; Name="TestUtil module available"; Text= "Function Test-TUActive from TestUtilHelpers is available"}if (Test-TUActive) {        $myReturn.Data="OK"; #As we are running in TestUtil, set the result to Success by using the alias "OK"}else {    #we are not running in TestUtil    $myReturn.Data="Not running in TestUtil"}$myReturn