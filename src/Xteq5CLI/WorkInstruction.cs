using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMDLine;
using Xteq5;
using Yamua;

namespace Xteq5CLI
{
    /// <summary>
    /// A representation what the CLI should do by parsing the arguments passed to it
    /// </summary>
    public class WorkInstruction
    {
        private WorkInstruction()
        {
        }

        public WorkInstruction(string[] CommandlineArguments)
        {
            //Set default values
            this.Execute = false;
            this.GenerateReport = false;
            this.DestinationFilepath = "";            
            this.DestinationFormat = OutputFormatEnum.Unknown;


            CMDLineParser parser = new CMDLineParser();
            parser.throwInvalidOptionsException = true;

            //Until further notice, no aliases are supported except for HELP. 

            //Add -Help option
            CMDLineParser.Option HelpOption = parser.AddBoolSwitch("-Help", "Displays help");
            HelpOption.AddAlias("/?");

            //Add -Run option
            CMDLineParser.Option RunOption = parser.AddBoolSwitch("-Run", "Required. Execute all files in compilation path");
            //RunOption.AddAlias("/Run");

            //Add -Path parameter
            CompilationPathOption PathParameter = new CompilationPathOption("-Path", "Compilation path to load scripts from", false);
            //PathParameter.AddAlias("/Path");
            parser.AddOption(PathParameter);

            //Add -Format parameter
            ReportFormatOption FormatParameter = new ReportFormatOption("-Format", "Format of the report that should be generated (HTML, XML ...)", false);
            //FormatParameter.AddAlias("/Format");
            parser.AddOption(FormatParameter);

            //Add -Filename parameter
            FilenameOption FilenameParameter = new FilenameOption("-Filename", "Filename of the generated report", false);
            //FilenameParameter.AddAlias("/Filename");
            parser.AddOption(FilenameParameter);

            //Add -Text parameter
            CMDLineParser.Option TextParameter = parser.AddStringParameter("-Text", "Additional text to be included in generated report", false);
            //FilenameParameter.AddAlias("/Text");

            bool commandLineParsed = false;

            try
            {
                parser.Parse(CommandlineArguments);
                commandLineParsed = true;
            }
            catch (CMDLineParser.CMDLineParserException ex)
            {
                //That didn't worked...                
                Console.WriteLine(ex.Message);
                Console.WriteLine("Use /? for help");
                Console.WriteLine();
            }

            if (commandLineParsed)
            {
                if (HelpOption.isMatched)
                {
                    Console.WriteLine(parser.HelpMessage());
                }
                else
                {
                    if (RunOption.isMatched == false)
                    {
                        //No -Run command, nothing to do. 
                        Console.WriteLine("Missing -RUN option");
                        Console.WriteLine(parser.HelpMessage());
                    }
                    else
                    {
                        this.Execute = true;

                        //Check for PATH parameter is set and use default path if not
                        this.CompilationPath = OptionIsMatchedAndNotEmpty(PathParameter) ? PathParameter.Value.ToString() : Xteq5UIConstant.DefaultCompilationFolder;

                        //Check for FILENAME parameter if we should generate a report. Only if this is set, check the additonal parameters for the report
                        if (OptionIsMatchedAndNotEmpty(FilenameParameter))
                        {
                            this.GenerateReport = true;
                            this.DestinationFilepath = FilenameParameter.Value.ToString();

                            //Check for the FORMAT parameter and use HTML if not set
                            string reportFormatString = OptionIsMatchedAndNotEmpty(FormatParameter) ? FormatParameter.Value.ToString() : "HTML";
                            
                            //This direct cast without any error checking is OK because FORMATPARAMETER already tried to parse it and will only be set if the value is OK
                            this.DestinationFormat = OutputFormatConverter.ParseReportFormat(reportFormatString);

                            this.UserText = OptionIsMatchedAndNotEmpty(TextParameter) ? TextParameter.Value.ToString() : "";                           

                        }

                        //All done!

                    }
                }
            }

        }

        /// <summary>
        /// Return TRUE if an option is both matched and is not null or empty
        /// </summary>
        /// <param name="Option">Option to check</param>
        /// <returns>TRUE if an option is both matched and is not null or empty</returns>
        private bool OptionIsMatchedAndNotEmpty(CMDLineParser.Option Option)
        {
            bool returnvalue = false;

            if (Option.isMatched)
            {
                if (Option.Value != null)
                {
                    if (string.IsNullOrWhiteSpace(Option.Value.ToString()) == false)
                    {
                        returnvalue = true;
                    }
                }
            }

            return returnvalue;
        }

        /// <summary>
        /// TRUE if we should run the scripts
        /// </summary>
        public bool Execute { get; private set; }

        /// <summary>
        /// Compilation path to be used
        /// </summary>
        public string CompilationPath { get; private set; }

        /// <summary>
        /// TRUE if a report should be generated, FALSE otherwise
        /// </summary>
        public bool GenerateReport { get; private set; }

        /// <summary>
        /// Filename of the report that should be generated
        /// </summary>
        public string DestinationFilepath { get; private set; }

        /// <summary>
        /// Type of format that should be generated
        /// </summary>
        public OutputFormatEnum DestinationFormat { get; private set; }

        /// <summary>
        /// Additional text to be added to the report
        /// </summary>
        public string UserText { get; private set; }

    }

    //Custom implementation for the ReportFormat option
    class ReportFormatOption : CMDLineParser.Option
    {
        //constuctor
        public ReportFormatOption(string name, string description, bool required)
            : base(name, description, typeof(string), true, required) { }


        //implementation of parseValue
        public override object parseValue(string Parameter)
        {
            OutputFormatEnum format = OutputFormatConverter.ParseReportFormat(Parameter);            

            if (format == OutputFormatEnum.Unknown)
            {
                throw new System.ArgumentException(string.Format("Report format [{0}] is not valid", Parameter));
            }
            else
            {
                return format;
            }

        }
    }

    class CompilationPathOption : CMDLineParser.Option
    {
        //constuctor
        public CompilationPathOption(string name, string description, bool required)
            : base(name, description, typeof(string), true, required) { }


        //implementation of parseValue
        public override object parseValue(string Parameter)
        {
            if (PathExtension.DirectoryExists(Parameter))
            {
                return Parameter;
            }
            else
            {
                throw new System.ArgumentException(string.Format("Directory [{0}] does not exist", Parameter));
            }
        }
    }

    class FilenameOption : CMDLineParser.Option
    {
        //constuctor
        public FilenameOption(string name, string description, bool required)
            : base(name, description, typeof(string), true, required) { }


        //implementation of parseValue
        public override object parseValue(string Parameter)
        {
            if (PathExtension.IsFilenameValid(Parameter))
            {
                return Parameter;
            }
            else
            {
                throw new System.ArgumentException(string.Format("Filename [{0}] invalid", Parameter));
            }

        }
    }
}
