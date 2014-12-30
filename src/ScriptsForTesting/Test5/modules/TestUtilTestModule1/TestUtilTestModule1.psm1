
function Get-TestUtilTestString {
 param ([string]$inString)

 $ModPath=[Environment]::GetEnvironmentVariable("PSModulePath")
 $result="Get-TestUtilTestString - In: [$inString] - ModPath: $ModPath"
 return $result
}



#Get-TestUtilTest1

