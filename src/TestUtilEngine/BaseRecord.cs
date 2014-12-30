using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtil
{
    /// <summary>
    /// Base class for AssetRecord and TestRecord
    /// </summary>
    public class BaseRecord
    {
        internal BaseRecord()
        {
            //Pre set the values to a faulty state
            Name = "No name";
            Conclusion = ConclusionEnum.Fatal;
            Description = "";

            ScriptFilePath = "No file";
            ProcessMessages = "No output";
            ScriptSuccessful = false;
        }

        public BaseRecord(BaseRecord copyFrom)
        {
            this.Name = copyFrom.Name;
            this.Conclusion = copyFrom.Conclusion;
            this.Description = copyFrom.Description;

            this.ScriptFilePath = copyFrom.ScriptFilePath;
            this.ProcessMessages = copyFrom.ProcessMessages;
            this.ScriptSuccessful = copyFrom.ScriptSuccessful;
        }

        /// <summary>
        /// Name of this record
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Conclusion (Fatal, Success, Error etc.) for this record
        /// </summary>
        public ConclusionEnum Conclusion { get; internal set; }

        /// <summary>
        /// Description text returned by the script
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Full filename of the script that generated this record
        /// </summary>
        public string ScriptFilePath { get; internal set; }

        /// <summary>
        /// Returns only the Script file, without path
        /// </summary>
        public string ScriptFilename
        {
            get
            {
                return Path.GetFileName(ScriptFilePath);
            }
        }

        /// <summary>
        /// Output of the script
        /// </summary>
        public string ProcessMessages { get; internal set; }         

        /// <summary>
        /// Appends a line at the top of ProcessMessages
        /// </summary>
        /// <param name="message"></param>
        internal void AddLineToProcessMessages(string message)
        {
            ProcessMessages = "TestUtil: " + message + "\r\n\r\n" + ProcessMessages;
        }

        /// <summary>
        /// TRUE if this script was executed correctly 
        /// </summary>
        public bool ScriptSuccessful { get; internal set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Name: ").Append(Name).Append("; Conclusion: ").Append(Conclusion).Append("; ScriptSuccessful: ").Append(ScriptSuccessful).Append("; ScriptFilename: ").Append(ScriptFilePath).Append("; ").Append(base.ToString());

            return sb.ToString();
        }


    }
}
