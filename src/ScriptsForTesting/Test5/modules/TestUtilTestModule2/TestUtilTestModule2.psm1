
function Get-TestUtilTest2String {
 param ([string]$inString)

 $ModPath=[Environment]::GetEnvironmentVariable("PSModulePath")
 $result="Get-TestUtilTest2String - In: [$inString] - ModPath: $ModPath"
 return $result
}



