using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessPS
{
    /// <summary>
    /// Preferences for PSScriptRunner
    /// </summary>
    public class PSScriptRunnerPreferences
    {
        public PSScriptRunnerPreferences()
        {
            _modulePath = "";
            _requiredPSVersion = 4;
            Variables = new HashSet<VariablePlain>();
        }

        string _modulePath;
        /// <summary>
        /// Path to load all modules from. If empty, no additional modules will be loaded. Standard modules for PowerShell are always available. 
        /// </summary>
        public string ModulePath {
            get
            {
                return _modulePath;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                string directory = value;

                if (directory != string.Empty)
                {
                    //MTH: Make sure that we have an absolute and not a relative path
                    directory = Path.GetFullPath(directory);

                    if (Directory.Exists(directory) == false)
                    {
                        throw new DirectoryNotFoundException(string.Format("Module path {0} not found", directory));
                    }
                }

                _modulePath = directory;
            }
        }

        int _requiredPSVersion;
        /// <summary>
        /// Major version of the PowerShell version that is required to run the scripts. 4 means "PowerShell 4.0 or higher"
        /// </summary>
        public int RequiredPSVersion {
            get
            {
                return _requiredPSVersion;
            }

            set
            {
                if (value < 3)
                    throw new ArgumentOutOfRangeException();

                _requiredPSVersion = value;
            }
        }

        /// <summary>
        /// Variables that can be accessed within the PowerShell script using $(name)
        /// </summary>
        public HashSet<VariablePlain> Variables { get; set; }

    }
}
