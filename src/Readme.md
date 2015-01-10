## Xteq5 source code (/src) README


## General

 - The scripts (Assets, tests and modules) that will be installed during setup are in _/scripts_.
 - Scripts for testing are located in _/src/ScriptsForTesting/Test1_
 - Any project that ends in **Consumer** is a debug project that uses the project it contains in the name (`Xteq5Engine_Consumer` uses `Xteq5Engine`).
 - Projects that end in **xUnitTest** are test assemblies for [xUnit](https://github.com/xunit/xunit)
 - If you get the error __The type or namespace name 'Xunit' could not be found (are you missing a using directive or an assembly reference?)__, xUnit needs to be downloaded using NuGet.
 - Also download _xunit.runner.visualstudio_ using NuGet to be able to use the _Test Explorer_. **IMPORTANT**: You need to select "Include prereleases" to find this package.
  

## Projects
 - `HeadlessPS` is a library to run scripts using PowerShell.
 - `Xteq5Engine` is the main library and uses HeadlessPS to run assets and tests.
 - `Xteq5OutputGenerator` is used to generate the resulting report.
 - `Xteq5UserInterface` contains helper functions when creating a user interface, e.g. default path for compilation etc. 

 - `Xteq5GUI` (Xteq5.exe) is the GUI used to operate Xteq5, it uses all four libraries above.
 - `Xteq5CLI` (Xteq5Cli.exe) is a command line interface for Xteq5, it uses all four libraries above.

 - `Xteq5Launcher` launches Xteq5GUI in the matching bitness (32 or 64 bit) for the computer. 

 - `src/setup/Setup.iss` is used to create **Xteq5Setup.exe**. Inside the solution the link to this file is in _Solution items_. 
 - You need to download and install [Inno Setup Unicode](http://www.jrsoftware.org/isdl.php) in order to open *.ISS files.
  

## Build procedure

  - Switch the project configuration to _Debug_ / _Any CPU_ then **run all xUnit tests** with the build-in Visual Studio Test Explorer (_Test_ > _Windows_ > _Test Explorer_)
  
  - Run **Code Analyizes** (_Analyze_ > _Run Code Analysis on Solution_)
  
  - If you make a change to the code, update _Solution items_ > _SharedAssemblyInfo.cs_ and increment _AssemblyVersion_. This information is uses as version details in all Xteq5 projects.
     
  - Update CHANGELOG.md

  - Commit to GitHub
  
  - Verifiy everything looks good. If not: Repeat.  
  
  - Use _Build_ > _Batch Build_ > _Build_ to create all files. _Xteq5Launcher.exe_ will build for **Any CPU** only. This is on purpose. 

  - Run Inno Setup and run `src/Setup/Setup.iss` to generate `Xteq5Setup.exe`.

  - Verify that the setup is working correctly
   
  - If the file looks good, go to https://github.com/texhex/xteq5/releases/new and attach it  
     
     - Tag is the current date in **ISO 8601** format with "." instead of "-". Hence, if you release on _2014-12-31_ than the correct tag is 
	    `v2014.12.31`.  
     - Second field is what the user sees, so please use 
	    `Version 2014.12.31`
     - Add 
	    `**Please see [CHANGELOG.md](CHANGELOG.md) for details**`
	 - This will automatically be linked to the changelog for that release using the tag.  
   
    
    
##Further readings
 
  * [How to: Configure Projects to Target Multiple Platforms](http://msdn.microsoft.com/en-us/library/ms165408.aspx)
  
  * [How to: Build Multiple Configurations Simultaneously](http://msdn.microsoft.com/en-us/library/jj651644.aspx)

  * [Batch Build Dialog Box](http://msdn.microsoft.com/en-us/library/169az28z%28v=vs.90%29.aspx)
 
  * [ISO 8601](http://xkcd.com/1179/)
  
  * [ISO 8601](http://en.wikipedia.org/wiki/ISO_8601) 

