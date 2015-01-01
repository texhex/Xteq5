## TestUtil Source code (/src) README


## General

 - The scripts (Assets, tests and modules) that will be installed during setup are in _/scripts_.
 - Scripts for testing are located in _/src/ScriptsForTesting/Test1_
 - Any project that ends in **Consumer** is a debug project that uses the project it contains in the name (_TestUtilEngine-**Consumer**_ uses _TestUtilEngine_).
 - Projects that end in **xUnitTest** are test assemblies for [xUnit](https://github.com/xunit/xunit)
 - If you get the error __The type or namespace name 'Xunit' could not be found (are you missing a using directive or an assembly reference?)__, xUnit needs to be downloaded using NuGet.
 - Also download _Xunit runners_ using NuGet. This allow to run all test with the build in Visual Studio's Test Explorer (_Test_ > _Windows_ > _Test Explorer_)


## Projects
 - **HeadlessPS** is a library to run scripts using PowerShell.
 - **TestUtilEngine** is the main library and uses HeadlessPS to run assets and tests.
 - **TestUtilOutputGenerator** is used to generate the resulting report.
 *** 
 - **TestUtilGUI** (TestUtil.exe) is the GUI used to operate TestUtil, it uses all three libraries above.
 - **TestUtilLauncher** launches TestUtilGUI in the matching bitness (32 or 64 bit) for the computer.
 ***
 - **TestUtilSetup** create _TestUtilSetup.exe_ and is located in **/src/Setup/_TestUtilSetup.iss**. Inside the solution the link to this file is in _Solution items_. 
 - You need to download and install [Inno Setup Unicode](http://www.jrsoftware.org/isdl.php) in order to open *.ISS files.
  

## Build procedure

  - If you make a change to the code, update _TestUtilEngine\Properties\AssemblyInfo.cs_ and increment _AssemblyVersion_. This information is the main indicator for the version of the binary files. 
  
  - Update CHANGELOG.md

  - Commit to GitHub
  
  - Verifiy everything looks good. If not: Repeat.  
  
  - Use _Build_ > _Batch Build_ > _Build_ to create all files. _TestUtilLauncher.exe_ will build for **Any CPU** only. This is on purpose. 

  - Run Inno Setup from `src\Setup` to generate `TestUtilSetup.exe`.

  - Verify that the setup is working correctly
   
  - If the file looks good, go to https://github.com/texhex/testutil/releases/new and attach it  
     
     - Tag is the current date in [ISO 8601](http://xkcd.com/1179/) format with "." instead of "-". Hence, if you release on 2014-12-31 than the correct tag is **v2014.12.31**.  
     - Second field is what the user sees, so please use **Version 2014.12.31**
   
    
    
##Further readinging about multi build
 
  * [How to: Configure Projects to Target Multiple Platforms](http://msdn.microsoft.com/en-us/library/ms165408.aspx)

  * [How to: Build Multiple Configurations Simultaneously](http://msdn.microsoft.com/en-us/library/jj651644.aspx)

  * [Batch Build Dialog Box](http://msdn.microsoft.com/en-us/library/169az28z%28v=vs.90%29.aspx)
 

  
  > Michael Hex,  December 2014
