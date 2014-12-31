
## TestUtil Build README
  
  - Update CHANGELOG.md

  - Update TestUtilEngine\Properties\AssemblyInfo.cs and increment _AssemblyVersion_

  - Commit to GitHub
  
  - Verifiy everything look good. If not: Repeat.  
  
  - Use _Build_ - _Batch Build_ - _Build_ to create all files. _TestUtilLauncher.exe_ will build for **Any CPU** only. This is on purpose. 

  - Run Inno Setup from `src\Setup` to generate `TestUtilSetup.exe`.

  - Test the setup
   
  - If the file looks good, go to https://github.com/texhex/testutil/releases/new and attach it  
     
     - Tag version should include "v", e.g. "v1.16"
     - Second field is what the user sees, so use "Version 1.16"

   
    
    
  **More information about using multi build:**
 
  * [How to: Configure Projects to Target Multiple Platforms](http://msdn.microsoft.com/en-us/library/ms165408.aspx)

  * [How to: Build Multiple Configurations Simultaneously](http://msdn.microsoft.com/en-us/library/jj651644.aspx)

  * [Batch Build Dialog Box](http://msdn.microsoft.com/en-us/library/169az28z%28v=vs.90%29.aspx)
 
  
  > Michael Hex,  December 2014
