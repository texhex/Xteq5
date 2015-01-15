#v1.02
#https://github.com/texhex/xteq5/wiki/_fwLinkScript


#This script requires PowerShell 4.0 or higher 
#requires -version 4.0

#Guard against common code errors
Set-StrictMode -version 2.0

#Terminate script on errors 
$ErrorActionPreference = 'Stop'#The value of 5 is based on the average amount of system errors I have seen. #Sometimes event perfectly good machine report some errors. #Machines that have issued have WAY MORE than five entries$Result = @{Data = "OK"; Name="Less than five errors in system log"; Text= "There are less than five errors in the system event log (since the last boot)"}$boottime=Get-MPXLastBootupTime$events=Get-EventLog -LogName System -After $boottime -EntryType Error -Newest 6 -ErrorAction SilentlyContinue##debug##$events=Get-EventLog -LogName System -After $boottime -EntryType Information -Newest 6 -ErrorAction SilentlyContinueif ($events -ne $null) {

   if ($events -is [array]) { #if the return is no array, there was only one single event found
       
       if ($events.Count -gt 5) { 
          $Result.Data="Fail"
          $Result.Text="More than five errors were reported since the last boot"

          #Add the last entry to outoput
          write-output ($Result.Text)
          
          $event=Get-EventLog -LogName System -After $boottime -EntryType Error -Newest 1
          
          write-output "Newest error entry:"
          write-output ($event.Source + ": " + $event.Message)
       }

    }
   
} else {   #Not a single event found   Write-Output "Not a single error found"}
$Result