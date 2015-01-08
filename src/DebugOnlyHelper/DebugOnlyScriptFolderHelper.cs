using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugOnlyHelper
{
    /// <summary>
    /// DEBUG ONLY! This can be used to find out the path of the TestingScripts folder
    /// </summary>
    public static class DebugOnlyScriptFolderHelper
    {

        public static string TestingScriptDirectory(int TestFolderID = 1)
        {
            //This function should return the path of "TestingScripts\TestX", e.g. 
            //   C:\dev\xteq5\current\src\TestingScripts\Test1)
            //based from the path of this assembly, e.g. 
            //   C:\dev\xteq5\current\src\Xteq5Engine_Consumer\bin\x86\Debug\
            //   or
            //   C:\dev\xteq5\current\src\Xteq5Engine_Consumer\bin\Debug\

            string concat = "";
            string dir = "";
            
            
            concat = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\..\..\..\ScriptsForTesting\"; 
            dir = Path.GetFullPath(concat);

            if (Directory.Exists(dir) == false)
            {
                //First try didn't worked, seems we have to go one folder more 
                concat = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"\..\..\..\..\ScriptsForTesting\";
                dir = Path.GetFullPath(concat);
            }

            dir += "Test" + TestFolderID.ToString();

            //If it still does not work, I'm giving up
            if (Directory.Exists(dir)==false)
                throw new DirectoryNotFoundException(string.Format("TestingScripts folder {0} not found",dir));

            return dir;
        }
    }
}
