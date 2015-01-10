using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessPS;
using Xteq5;
using Yamua;

namespace Xteq5
{
    /// <summary>
    /// Intend for providing an UI-friendly method to execute Xteq5Runner. This class will perform all the necessary
    /// tasks and also tries to convert any exception that might appear to a user friendly message. 
    /// 
    /// Create the class with the necessary options, then start Run()/RunAsync(). 
    /// 
    /// If a fatal error happens (e.g. PowerShell has failed), ExecutionSuccessful will be FALSE. 
    /// FailedMessage will contain a (hopefully) user-friendly description what went wrong, 
    /// FailedException will contain the exception that caused the failure.
    /// </summary>
    public class SimplifiedXteq5Runner
    {
        private SimplifiedXteq5Runner()
        {
        }

        string _compilationPath = "";


        private void ClearAllVariables()
        {
            _compilationPath = "";
            
            _reportFormat = OutputFormatEnum.Unknown;
            _userText = "";

            ClearRunVariables();
        }

        private void ClearRunVariables()
        {
            this.Report = null;
            this.ReportFilepath = "";

            this.ExecutionSuccessful = false;
            this.IssuesFound = false;
            this.FailedMessage = "UNKNOWN";
            this.FailedException = new Exception("Unknown exception");
        }

        /// <summary>
        /// Executes Xteq5 without generating a report
        /// </summary>
        /// <param name="CompilationPath">Path to the compilation folder</param>
        public SimplifiedXteq5Runner(string CompilationPath)
        {
            ClearAllVariables();

            _compilationPath = CompilationPath;
        }

        string _userText = "";
        OutputFormatEnum _reportFormat = OutputFormatEnum.Unknown;

        /// <param name="UserText">Optional text that should appear on the report</param> //ReportFormatEnum ReportFormat
        //_reportFormat = ReportFormat; //This will later on checked by the object, so no need to check this here


        /// <summary>
        /// Executes Xteq5 and generates a HTML report for it
        /// </summary>
        /// <param name="CompilationPath">Path to the compilation folder</param>        
        /// <param name="UserText">Additopnal text to be added to the generated HTML file</param>        
        public SimplifiedXteq5Runner(string CompilationPath, string UserText)
            : this(CompilationPath)
        {
            _userText = UserText;
            _reportFormat = OutputFormatEnum.HTML;        
        }


        /// <summary>
        /// TRUE if the Execution (generally) was successful. This does NOT mean that all test were executed successful.
        /// </summary>
        public bool ExecutionSuccessful { get; private set; }

        /// <summary>
        /// Reference to the generated Xteq5 report
        /// </summary>
        public Report Report { get; private set; }

        /// <summary>
        /// Returns TRUE if at least one asset or test has the starus MINOR, MAJOR or FATAL
        /// </summary>
        public bool IssuesFound { get; private set; }

        /// <summary>
        /// Returns a user-friendly description what went wrong when ExecutionSuccessful=FALSE
        /// </summary>
        public string FailedMessage { get; private set; }

        /// <summary>
        /// Returns the Exception that caused the execution to fail.
        /// </summary>
        public Exception FailedException { get; private set; }

        /// <summary>
        /// Contains the path to the generated report (if any)
        /// </summary>
        public string ReportFilepath { get; private set; }


        /// <summary>
        /// Generates the report and (if set) the file report
        /// </summary>
        /// <returns>TRUE if execution was successful</returns>
        public bool Run()
        {
            Task<bool> task = RunAsync();
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Generates the report and (if set) the file report
        /// </summary>
        /// <returns>TRUE if execution was successful</returns>
        public async Task<bool> RunAsync()
        {
            ClearRunVariables();  

            bool successful = false;            
                                                
            try
            {
                Xteq5Runner runner = new Xteq5Runner();
                this.Report = await runner.RunAsync(_compilationPath);

                this.Report.UserText = _userText;

                //TODO: !!!!FIX ME!!!!
                this.ReportFilepath = OutputGenerator.GenerateReportOutputFile(this.Report, OutputFormatEnum.HTML, "");

                /*
                string htmlTemplatePath = Path.Combine(_compilationPath, "BootstrapTemplate1.html");
                BootstrapHTMLGenerator generator = new BootstrapHTMLGenerator(htmlTemplatePath);
                string tempFile = generator.GenerateAndSaveFile(this.Report);

                this.ReportFilepath = tempFile;
                 */

                //Set IssuesFound
                IssuesFound = false;

                int itemp = 0;
                itemp += this.Report.AssetStatiscs.FatalCount + this.Report.AssetStatiscs.MajorCount + this.Report.AssetStatiscs.MinorCount;
                itemp += this.Report.TestStatiscs.FatalCount + this.Report.TestStatiscs.MajorCount + this.Report.TestStatiscs.MinorCount;
                if (itemp > 0)
                {
                    IssuesFound=true;
                }

                successful = true;
            }

            catch (FileNotFoundException fnfe)
            {
                //In most cases this means we couldn't load System.Management.Automation. 
                if (fnfe.Message.Contains("System.Management.Automation"))
                {
                    //We need to explain to the user that this means PowerShell is missing. 
                    this.FailedMessage = "PowerShell is required, but could not be loaded. Please re-run Setup.exe and follow the provided link to install it.";
                }
                else
                {
                    //In any other case we have no idea what is missing, so use the message from the exception.
                    this.FailedMessage = fnfe.Message;
                }

                this.FailedException = fnfe;
            }
            catch (CompilationFolderException cfnfe)
            {
                //The given compilation folder was not found. Message is as good as it is.
                this.FailedMessage = "The selected compilation folder does not exist";
                this.FailedException = cfnfe;
            }
            catch (CompilationSubFolderException csfnfe)
            {
                //A sub folder was not found. Message is as good as it is.
                this.FailedMessage = "A required subfolder of the selected compilation folder does not exist";
                this.FailedException = csfnfe;
            }
            catch (PowerShellTestFailedException pstfe)
            {
                //The powershell test failed
                this.FailedMessage = "Testing the PowerShell environment failed. ";
                this.FailedException = pstfe;
            }
            catch (TemplateFileNotFoundException tfnfe)
            {
                //Template file does not exist in folder 
                this.FailedMessage = "The template file for the report is missing. Please re-run Setup.exe to install it.";
                this.FailedException = tfnfe;
            }
            catch (Exception exc)
            {
                //No idea what happened. Use the message from the exception.
                this.FailedMessage = exc.Message;
                this.FailedException = exc;
            }


            if (successful)
            {
                this.ExecutionSuccessful = true;
                this.FailedMessage = "";
                this.FailedException = null;
                return true;

            }
            else
            {
                this.ExecutionSuccessful = false;
                return false;

            }


        }
    }
}
