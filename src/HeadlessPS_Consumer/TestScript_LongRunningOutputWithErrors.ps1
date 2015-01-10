$VerbosePreference='Continue'

$s1 = 'test1'
$s2 = 'test2'
$s1

write-error 'write error'
write-warning 'write warning'
write-warning 'second warning'

start-sleep -s 3

$s2
