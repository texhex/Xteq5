using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public sealed class UserInterface
    {
        private UserInterface()
        {
        }

        #region Singelton code
        public static UserInterface Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly UserInterface instance = new UserInterface();
        }
        #endregion


        /// <summary>
        /// Default path to the compilation create by setup.exe.
        /// Example: C:\ProgramData\Xteq5
        /// </summary>
        public string DefaultCompilationFolder
        {
            get
            {
                string programDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                return Path.GetFullPath(programDataFolder) + @"\Xteq5";
            }
        }
    }
}
