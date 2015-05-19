using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xteq5Launcher
{
    static class Program
    {
        /// <summary>
        /// Checks if the system is x86 (32 bit) or x64 (64 bit) and then tries 
        /// to execute Xteq5.exe from either the subfolder \32bit or \64bit.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string exeName = @"\Xteq5.exe";
            string folderNameX86= "32bit";
            string folderNameX64="64bit";

            //DO NOT use ApplicationInformation because we do not want to have any dependencies. 
            //Contains \ at the end
            string launchEXE = AppDomain.CurrentDomain.BaseDirectory.ToString();             
            
            launchEXE += Environment.Is64BitOperatingSystem ? folderNameX64 : folderNameX86;
            launchEXE += exeName;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.ErrorDialog = true;
            psi.FileName = launchEXE;

            try
            {
                Process.Start(psi);
            }
            catch
            {
            }



        }
    }
}
