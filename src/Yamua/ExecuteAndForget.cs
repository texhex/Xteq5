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
        /// <param name="FilenamePath">Filename and path to the file to be executed, a WWW link will work also</param>
        /// <returns>TRUE if the file was started successfully</returns>
        public static bool Execute(string FilenamePath)
        {
            return Execute(FilenamePath, string.Empty);
        }

        /// <summary>
        /// Execute a file (exe, link etc.). Windows will display an error message when something goes wrong.
        /// </summary>
        /// <param name="FilenamePath">Filename and path to the file to be executed, a WWW link will work also</param>
        /// <param name="Arguments">Arguments to be used</param>
        /// <returns>TRUE if the file was started successfully</returns>
        public static bool Execute(string FilenamePath, string Arguments)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.ErrorDialog = true;
            psi.FileName = FilenamePath;
            psi.Arguments = Arguments;

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
