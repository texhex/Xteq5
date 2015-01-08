#v1.01
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#require -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'$Result = @{Data = "OK"; Name="%PATH% contains existing folder"; Text= "All paths listed in environment variable %PATH% do exist"}$path=$Env:Path#debug#$path="C:\ARG;$path"#$path="$path;C:\ARG;"#Split the string$options = [System.StringSplitOptions]::RemoveEmptyEntries$ary=$path.Split(";", $options) #split on ";" and ignore empty entriesForeach ($curdir in $ary)
{

  if (-Not (Test-Path $curdir)) {
     #we have found an invalid path
     $Result.Data="Minor"
     $Result.Text="Folder [$curdir] does not exist but is included in %PATH%"

     #push the registy path to output
     "The PATH for the computer can be changed using this registry value: [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment\Path]"

     break
  }
}#$path#$ary$Result