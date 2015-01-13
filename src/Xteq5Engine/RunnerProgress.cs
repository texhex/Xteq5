using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    /// <summary>
    /// This class is used to report progress pack from the Xteq5Runner async method
    /// </summary>
    public class RunnerProgress
    {
        public RunnerProgress()
        {
            Starting = false;
            Ended = false;
            ScriptFilename = "";
            ScriptFilepath = "";
        }

        /// <summary>
        /// TRUE if the runner is getting ready to be started
        /// </summary>
        public bool Starting { get; internal set; }

        /// <summary>
        /// Full file path including path of the script that is about to be executed
        /// </summary>
        public string ScriptFilepath { get; internal set; }

        /// <summary>
        /// Filename of the script that is about to be executed
        /// </summary>
        public string ScriptFilename { get; internal set; }

        /// <summary>
        /// TRUE if the runner has completed the entire execution 
        /// </summary>
        public bool Ended { get; internal set; }

    }
}
