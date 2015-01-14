using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public enum RunnerAction
    {
        Starting = 1, //The runner is getting ready to be started
        ScriptRunning = 2, //A script is about to be executed
        Ended = 4 //The runner has completed
    }

    /// <summary>
    /// This class is used to report progress pack from the Xteq5Runner async method
    /// </summary>
    public class RunnerProgress
    {
        public RunnerProgress()
        {
            Action = RunnerAction.Starting;
            ScriptFilename = "";
            ScriptFilepath = "";
        }

        /// <summary>
        /// The action the runner reports
        /// </summary>
        public RunnerAction Action;

        /// <summary>
        /// Full file path including path of the script that is about to be executed
        /// </summary>
        public string ScriptFilepath { get; internal set; }

        /// <summary>
        /// Filename of the script that is about to be executed
        /// </summary>
        public string ScriptFilename { get; internal set; }


        public override string ToString()
        {
            switch (this.Action)
            {
                case RunnerAction.Starting:
                    return "Preparing...";

                case RunnerAction.ScriptRunning:
                    return "Executing \"" + this.ScriptFilename + "\"...";

                case RunnerAction.Ended:
                    return "Cleaning up...";

                default:
                    return "Unknown Action";
            }


        }

    }
}
