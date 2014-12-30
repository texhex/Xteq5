$s1 = 'test1'
$s2 = 'test2'


$MyTestVarWrite="Data set by script"


#Enabling the line below will result the script to become failed when you try to write to the read-only variable $MyTestVarReadOnly (Message "Cannot overwrite variable MyTestVarReadOnly because it is read-only or constant.")
#$ErrorActionPreference = "Stop"

#This will not work because the variable is read-only
#$MyTestVarReadOnly="Should result in an error"



$myReturn = @{ MyTestVarWrite=$MyTestVarWrite; MyTestVarReadOnly = $MyTestVarReadOnly; MyTestVarVersion = $MyTestVarVersion }

$myReturn

