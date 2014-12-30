
**TestUtilSolution Readme**
  
  - Update CHANGELOG.md

  - Commit the latest changes 
  
  - Use Build -> Batch Build to create all files. 
    
	- Note: TestUtilLauncher.exe will ALWAYS build for *Any CPU* only. This is on purpose. 

  - After that, run Inno Setup from src\Setup to generate "TestUtilSetup.exe".

  - The resulting file then needs to be tested 
   
  - If the file is OK, go to https://github.com/texhex/testutil/releases/new and attach it
     - Tag version should include "v", e.g. "v1.16"
     - Second field is what the user sees, so it's "Version 1.16")

 
  
    
    
  **More information about using multi build:**
 
  [How to: Configure Projects to Target Multiple Platforms](http://msdn.microsoft.com/en-us/library/ms165408.aspx)

  [How to: Build Multiple Configurations Simultaneously](http://msdn.microsoft.com/en-us/library/jj651644.aspx)

  [Batch Build Dialog Box](http://msdn.microsoft.com/en-us/library/169az28z%28v=vs.90%29.aspx)
 
  
  Michael Hex,  December 2014
