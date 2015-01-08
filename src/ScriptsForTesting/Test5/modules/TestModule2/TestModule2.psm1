
function Get-TestModule2String {
 param ([string]$inString)

 $ModPath=[Environment]::GetEnvironmentVariable("PSModulePath")
 $result="Get-TestModule2String - In: [$inString] - ModPath: $ModPath"
 return $result
}



