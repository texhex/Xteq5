using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    /// <summary>
    /// Fire-and-Forget execution of files, processes etc. 
    /// </summary>
    public static class ExecuteAndForget
    {
        /// <summary>
        /// Execute a file (exe, link etc.). Windows will display an error message when something goes wrong.
        /// </summary>
        /// <param name="fileNamePath">Filename and path to the file to be executed, a WWW link will work also</param>
        /// <returns>TRUE if the file was started successfully</returns>
        public static bool Execute(string fileNamePath)
        {
            return Execute(fileNamePath, string.Empty);
        }

        /// <summary>
        /// Execute a file (exe, link etc.). Windows will display an error message when something goes wrong.
        /// </summary>
        /// <param name="fileNamePath">Filename and path to the file to be executed, a WWW link will work also</param>
        /// <param name="arguments">Arguments to be used</param>
        /// <returns>TRUE if the file was started successfully</returns>
        public static bool Execute(string fileNamePath, string arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.ErrorDialog = true;
            psi.FileName = fileNamePath;
            psi.Arguments = arguments;

            try
            {
                Process.Start(psi);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
