using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace Xteq5
{
    /// <summary>
    /// A base class for class that reads a template file and then replaces the content with the actual values.
    /// </summary>
    public abstract class BaseTemplateReplaceGenerator
    {
        protected StringBuilder _content = new StringBuilder();

        protected void ReadTemplate(string TemplateFilepath)
        {
            if (string.IsNullOrWhiteSpace(TemplateFilepath))
                throw new ArgumentException("TemplateFilepath can not be empty");

            if (PathExtension.FileExists(TemplateFilepath) == false)
            {
                throw new TemplateFileNotFoundException(TemplateFilepath);
            }

            //If there is a load error, this will throw an exception
            _content = new StringBuilder(File.ReadAllText(TemplateFilepath, Encoding.UTF8));
        }

        //Base function that is called by a consumer
        public abstract string Generate(Report Report, string TemplateFilepath);


        //This needs to be called by the implementation to start the replacement process
        protected void StartGenerating(Report Report)
        {
            ReplaceHeaderValuesInternal(Report);

        }

        private void ReplaceHeaderValuesInternal(Report Report)
        {
            //Replace all known string values
            ReplaceHeaderValueInternal("ReportID", Report.ID.ToString());
            ReplaceHeaderValueInternal("UserName", Report.UserName);
            ReplaceHeaderValueInternal("Computername", Report.ComputerName);
            ReplaceHeaderValueInternal("UserText", Report.UserText);
            ReplaceHeaderValueInternal("SourceFolder", Report.CompilationFolder);
            ReplaceHeaderValueInternal("VersionString", Report.Xteq5Version.ToString());

            //Datetime in UTC and ISO 8601 format without fraction of second
            ReplaceHeaderValueInternal("StartDateTimeUTC", Report.StartedUTC.ToString("s") + "Z");
            ReplaceHeaderValueInternal("EndDateTimeUTC", Report.EndedUTC.ToString("s") + "Z");
            ReplaceHeaderValueInternal("RuntimeSeconds", Report.RuntimeSeconds);
        }

        private void ReplaceHeaderValueInternal(string ValueName, string Value)
        {
            ReplaceHeaderValue(ValueName.ToUpper(), Value);
        }

        //Called from this class to replace typical "Header" items. Data that is only found once in the report. 
        protected abstract void ReplaceHeaderValue(string ValueName, string Value);

    }
}
