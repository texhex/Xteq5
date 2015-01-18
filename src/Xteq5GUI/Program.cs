using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace Xteq5GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool issueDetectedAndFixed = FixUserSettingsFile();

            if (issueDetectedAndFixed == false)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                string message = "It was detected that your user settings file has become corrupted.\r\n\r\n" +
                                 "We are sorry about that. \r\n\r\n" +
                                 "The file was deleted, please restart the program.";

                MessageBox.Show(message, "Corrupt user settings file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                Application.Exit();
            }
        }

        
        /// <summary>
        /// Tries to load the user settings file. If an error is detected, it will try to fix the problem. 
        /// </summary>
        /// <returns>TRUE if there was an error and it was fixed, FALSE if no error was detected</returns>
        public static bool FixUserSettingsFile()
        {
            //Code taken from this answer http://stackoverflow.com/a/18905791/612954 by [tofutim](http://stackoverflow.com/users/339843/tofutim)
            var wasFixed = false;

            try
            {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            }
            catch (ConfigurationErrorsException ex)
            {
                string filename = string.Empty;

                if (string.IsNullOrEmpty(ex.Filename) == false)
                {
                    filename = ex.Filename;
                }
                else
                {
                    var innerEx = ex.InnerException as ConfigurationErrorsException;

                    if (innerEx != null && string.IsNullOrEmpty(innerEx.Filename) == false)
                    {
                        filename = innerEx.Filename;
                    }
                }

                if (string.IsNullOrEmpty(filename) == false)
                {
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                        wasFixed = true;
                    }
                }
            }

            return wasFixed;
        }
    }
}
