#Script requires PowerShell 4.0 or higher - http://blogs.msdn.com/b/powershell/archive/2009/02/06/requires-your-scripts.aspx
#require -version 4.0

#Guard against common code errors - http://technet.microsoft.com/en-us/library/ff730970.aspx
Set-StrictMode -version 2.0

#Terminate on errors - http://blogs.technet.com/b/heyscriptingguy/archive/2010/03/08/hey-scripting-guy-march-8-2010.aspx
$ErrorActionPreference = 'Stop'


$result = @{Name = "Asset #025 Name"; Data = "Asset #025 Data"; Text = "Asset #025 Text"}#Now add some fields to this hashtable$result.MyAdditonalField1 = "Some data"$result.VersionOfPSObject = [PSObject].Assembly.GetName().Version$result.___FirstField ="First!"
#TestUtil will process this result without problems because it ignores the additional fields$result