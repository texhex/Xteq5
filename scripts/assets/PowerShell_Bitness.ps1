#Version 1.02

#Script requires PowerShell 4.0 or higher - http://blogs.msdn.com/b/powershell/archive/2009/02/06/requires-your-scripts.aspx
#require -version 4.0

#Guard against common code errors - http://technet.microsoft.com/en-us/library/ff730970.aspx
Set-StrictMode -version 2.0

#Terminate on errors - http://blogs.technet.com/b/heyscriptingguy/archive/2010/03/08/hey-scripting-guy-march-8-2010.aspx
$ErrorActionPreference = 'Stop'


#Set .Data to "" - this means "Does not apply" $Result = @{Data = ""; Text = "Bitness (x86 or x64) of PowerShell used"}
#Side note: [Environment]::Is64BitProcess might also be an optionSwitch ([System.IntPtr]::Size) {       4 {           $Result.Data = "32bit"        }                 8 {           $Result.Data = "64bit"        }}$Result