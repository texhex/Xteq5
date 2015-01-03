## TestUtil Source code (/src) README


## General

 - The scripts (Assets, tests and modules) that will be installed during setup are in _/scripts_.
 - Scripts for testing are located in _/src/ScriptsForTesting/Test1_
 - Any project that ends in **Consumer** is a debug project that uses the project it contains in the name (`TestUtilEngine-Consumer` uses `TestUtilEngine`).
 - Projects that end in **xUnitTest** are test assemblies for [xUnit](https://github.com/xunit/xunit)
 - If you get the error __The type or namespace name 'Xunit' could not be found (are you missing a using directive or an assembly reference?)__, xUnit needs to be downloaded using NuGet.
 - Also download _Xunit runners_ using NuGet. 

## Projects
 - `HeadlessPS` is a library to run scripts using PowerShell.
 - `TestUtilEngine` is the main library and uses HeadlessPS to run assets and tests.
 - `TestUtilOutputGenerator` is used to generate the resulting report.
 - `TestUtilGUI` (TestUtil.exe) is the GUI used to operate TestUtil, it uses all three libraries above.
 - `TestUtilLauncher` launches TestUtilGUI in the matching bitness (32 or 64 bit) for the computer. 
 - `src/setup/_TestUtilSetup.iss` is used to create **TestUtilSetup.exe**. Inside the solution the link to this file is in _Solution items_. 
 - You need to download and install [Inno Setup Unicode](http://www.jrsoftware.org/isdl.php) in order to open *.ISS files.
  

## Build procedure

  - Run all xUnit tests with the build-in Visual Studio Test Explorer (_Test_ > _Windows_ > _Test Explorer_)
  
  - Run Code Analyizes (_Analyze_ > _Run Code Analysis on Solution_)
  
  - If you make a change to the code, update _Solution items_ > _SharedAssemblyInfo.cs_ and increment _AssemblyVersion_. This information is uses as version details in all TestUtil projects.
     
  - Update CHANGELOG.md

  - Commit to GitHub
  
  - Verifiy everything looks good. If not: Repeat.  
  
  - Use _Build_ > _Batch Build_ > _Build_ to create all files. _TestUtilLauncher.exe_ will build for **Any CPU** only. This is on purpose. 

  - Run Inno Setup and run `src\Setup\_TestUtilSetup.iss` to generate `TestUtilSetup.exe`.

  - Verify that the setup is working correctly
   
  - If the file looks good, go to https://github.com/texhex/testutil/releases/new and attach it  
     
     - Tag is the current date in **ISO 8601** format with "." instead of "-". Hence, if you release on _2014-12-31_ than the correct tag is **v2014.12.31**.  
     - Second field is what the user sees, so please use **Version 2014.12.31**
     - Add `**Please see [CHANGELOG.md](CHANGELOG.md) for details**`. This will automatically be linked to the changelog for that release using the tag.  
   
    
    
##Further readings
 
  * [How to: Configure Projects to Target Multiple Platforms](http://msdn.microsoft.com/en-us/library/ms165408.aspx)
  
  * [How to: Build Multiple Configurations Simultaneously](http://msdn.microsoft.com/en-us/library/jj651644.aspx)

  * [Batch Build Dialog Box](http://msdn.microsoft.com/en-us/library/169az28z%28v=vs.90%29.aspx)
 
  * [ISO 8601](http://xkcd.com/1179/)
  
  * [ISO 8601](http://en.wikipedia.org/wiki/ISO_8601) 

