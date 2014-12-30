
function Get-StringFromAModule {
 param ([string]$inString)

 $result="This is a message from the JustTestingModule from the /modules folder. You passed in: [$inString]"
 return $result
}

