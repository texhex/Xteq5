#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'


$s1="This output is ignored"write-output $s1$s2="As well as this"write-output $s2#Xteq5 will ignore the above two values because they are not hashtables $myReturn = @{Name = "Asset #020 Name"; Data = "Asset #020 Data"; Text = "Asset #020 Text"}
#This output will be processed because IT IS a hashtablewrite-output $myReturn