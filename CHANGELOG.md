## WORKING ON

IMPROVEMENTS:

 * Added assets and tests
 * Added functions to MPXmodule
 
BUG FIXES:

 * None
 
## [Version 2015.01.05](https://github.com/texhex/testutil/releases/tag/v2015.01.05) 

IMPROVEMENTS:

 * Changed script details text in HTML template
 * Added assets and tests
 * Changed all filenames in /src/scripts - files now start with "-" (minus), because "_" (underscore) did not work with markdown
 * Started to include non-TestUtil related functions to scripts/MPXModule/MPXModule.psm1 (My PowerShell eXtensions)
 * Changed HTML template
 * Added option in src/setup/Setup.iss to copy modules from TestUtil to personal PS modules path (unchecked by default)
 
BUG FIXES:

 * Returning n/a from an asset did not set status to DoesNotApply
 
## [Version 2015.01.03](https://github.com/texhex/testutil/releases/tag/v2015.01.03) 

IMPROVEMENTS:

 * Added example report link to README.MD
 * Changed README.MD
 * Changed button sizing TestUtilGUI
 * Changed html template
 * Finished (sort of) https://github.com/texhex/testutil/wiki/Custom-scripts
 * Added SharedAssemblyInfo to all release projects
 * Setup now creates VERSION.ini in install directory
 * Changed all files in /src/scripts so the filename starts with _ (underscore)
 * Updated README.TXT in /src/scripts/ 
 * Changed button placement in TestUtilGUI
 
BUG FIXES:

 * Fixed code analysis errors in GlobalAtom.cs

## [Version 2015.01.01](https://github.com/texhex/testutil/releases/tag/v2015.01.01) 

**BREAKING CHANGES:**

* CHANGELOG.MD and releases now use the publish date as version tag 
* Changed previous releases to this new format  

IMPROVEMENTS:

 * Reworked README.md
 * Reworked /src/README.MD 
 * Several small code changes
 * Changed About window to monospaced font
 * Exception handling in TestUtilGUI greatly improved     
 
BUG FIXES:

 * Strange message when PowerShell assembly can't be found - [#4](https://github.com/texhex/testutil/issues/4)

## [Version 2014-12-31](https://github.com/texhex/testutil/releases/tag/v1.20) 

IMPROVEMENTS:

 * Explained overwrite procedure in %PROGRAMDATA%\TestUtil\readme.txt (Repository source /scripts/readme.txt) better
 * Added "Fail" as alias for result "Major"
 * Added MarkdownPad to /licenses/license.txt
 * Reworked /README.md
 * Reworked Build Readme
 * Added test script #825 that uses write-warning (/src/ScriptsForTesting/Test1/tests/)
 
BUG FIXES:

 * Fixed wrong date for v1.19 in this file

## [Version 2014-12-30](https://github.com/texhex/testutil/releases/tag/v1.19)

IMPROVEMENTS:

 * Tests can now access asset values ($TestUtilAssets) without using the exact case for the name. A case-insensitive hash table is now used.
 * Setup now checks if PowerShell 4.0 (WMF 4.0) and .NET 4.5 are installed and show a download link if not. 
 
BUG FIXES:

 * Scripts did not run when the PowerShell ExecutionPolicy was undefined
 
## <a name="1.15">Version 2014-12-29</a>

IMPROVEMENTS:

 * Added TestUtilLauncher.exe that launches either the x86 or the x64 version of TestUtilGUI
 
BUG FIXES:

 * Changed several comments inside bootstrap HTML template because of errors reported by Internet Explorer
 
## <a name="1.14">Version 2014-12-28</a>

**BREAKING CHANGES:**

 * CHANGELOG.md section ordering is BREAKING, IMPROVMENTS, BUG FIXES
 * Format of this changelog.md taken from [Chocolatey CHANGELOG.md](https://github.com/chocolatey/chocolatey/blob/master/CHANGELOG.md)
 
IMPROVEMENTS:

 * D'oh! - [#1](https://github.com/texhex/testutil/issues/1)
  
BUG FIXES:

 * Added changelog.md - [#2](https://github.com/texhex/testutil/issues/2)
 
 