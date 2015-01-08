#Script requires PowerShell 4.0 or higher - http://blogs.msdn.com/b/powershell/archive/2009/02/06/requires-your-scripts.aspx
#require -version 4.0

#Guard against common code errors - http://technet.microsoft.com/en-us/library/ff730970.aspx
Set-StrictMode -version 2.0

#Terminate on errors - http://blogs.technet.com/b/heyscriptingguy/archive/2010/03/08/hey-scripting-guy-march-8-2010.aspx
$ErrorActionPreference = 'Stop'


$s1="This output is ignored"write-output $s1$s2="As well as this"write-output $s2#Xteq5 will ignore the above two values because they are not hashtables $myReturn = @{Name = "Asset #020 Name"; Data = "Asset #020 Data"; Text = "Asset #020 Text"}
#This output will be processed because IT IS a hashtablewrite-output $myReturn