$s="My Output"
$s

write-error 'An error message';

$WarningPreference = 'Continue'
write-warning 'A warning message'

$VerbosePreference = 'Continue'
write-verbose 'A verbose message'

$DebugPreference = 'Continue'
write-debug 'A debug message'

