
function Get-TestModuleTestString {
 param ([string]$inString)

 $ModPath=[Environment]::GetEnvironmentVariable("PSModulePath")
 $result="Get-TestModuleTestString - In: [$inString] - ModPath: $ModPath"
 return $result
}



