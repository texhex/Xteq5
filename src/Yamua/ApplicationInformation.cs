using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    public sealed class ApplicationInformation
    {
        private ApplicationInformation()
        {
            Folder = AppDomain.CurrentDomain.BaseDirectory.ToString();
            ParentFolder = Path.GetFullPath(Folder + @"..\");
        }

        /// <summary>
        /// Returns the folder where this application is executed from
        /// </summary>
        public string Folder { get; private set; }


        /// <summary>
        /// Returns the parent folder where the application is executed from. If the application is C:\test\test.exe, this will return C:\
        /// </summary>
        public string ParentFolder { get; private set; }


        #region Singelton code
        public static ApplicationInformation Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly ApplicationInformation instance = new ApplicationInformation();
        }
        #endregion

    }

}
