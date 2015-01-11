


REM First step is to find out if this is a x64 or a x86 machine.
REM This happens in order to start the correct version (bitness match) of Xteq5CLI.


SET BITNESS=64bit
IF "%PROCESSOR_ARCHITECTURE%" == "x86" ( 
	if not defined PROCESSOR_ARCHITEW6432 SET BITNESS=32bit
)

REM Clear errorlevel variable, just to be sure
SET ERRORLEVEL=

REM Since Xteq5 is always installed into the native ProgramFiles folder, we can use the path directly
"C:\Program Files\Xteq5\%BITNESS%\Xteq5CLI.exe" -Run -Path "C:\dev\git\Xteq5\scripts" -Format XML -Filename "C:\Temp\result.xml" -Text "From BatchTestBAT"
SET RETURN_CODE=%ERRORLEVEL%

   REM Execute Xteq5CLI (Debug Version)
   REM "%~dp0bin\x64\Debug\Xteq5CLI.exe" -Run -Path "C:\dev\git\Xteq5\scripts" -Format XML -Filename "C:\Temp\result.xml" -Text "From BatchTestBAT"

REM Write found RETURN CODE
echo Error level is %RETURN_CODE%

REM Check the result with a STRING operator to avoid side effects
IF "%RETURN_CODE%"=="0" GOTO good
IF "%RETURN_CODE%"=="13" GOTO issuesfound
IF "%RETURN_CODE%"=="22" GOTO hell

REM We expected that this line will not be executed because of the GOTO commands.
REM If we are here, it means an unknown return code has been returned (e.g. 9009 for file not found). 
REM Go to hell in this case.
goto hell


:good
echo.
echo All is good.
echo.
goto ENDE


:issuesfound
echo.
echo Some issues were found
echo.
goto ENDE


:hell
echo.
echo Fatal error during run!
echo.
goto ENDE



:ENDE
pause
