using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace Xteq5
{

    public static class Xteq5UIConstant
    {
        /// <summary>
        /// Default path to the compilation create by setup.exe.
        /// Example: C:\ProgramData\Xteq5
        /// </summary>
        public static string DefaultCompilationFolder
        {
            get
            {
                string programDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return Path.GetFullPath(programDataFolder) + @"\Xteq5";
            }
        }

        /// <summary>
        /// Returns the content of licenses\license.txt or the error text if the file was not found
        /// </summary>
        public static string LicenseTxtContent
        {
            get
            {
                string filePath = ApplicationInformation.Instance.ParentFolder + @"licenses\license.txt";

                return File.Exists(filePath) ? File.ReadAllText(filePath, Encoding.Default) : "Unable to load " + filePath;
            }
        }

        //MTH: These are static readonly fields BY PURPOSE (not CONST) to avoid that the compiler performs an copy+paste to the assembly of the consumer.
        //See [Const Strings - a very convenient way to shoot yourself in the foot](http://www.stum.de/2009/01/14/const-strings-a-very-convenient-way-to-shoot-yourself-in-the-foot/) by [Michael Stum](http://www.stum.de/)        


        public static readonly string DocumentationURL = "https://github.com/texhex/Xteq5/wiki/";

        public static readonly string HomepageURL = "http://www.xteq5.com/";

        public static readonly string HowtoUseURL = "https://github.com/texhex/Xteq5/wiki/How-to-use";


    }
}
