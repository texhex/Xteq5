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
        string _userText = "";
        OutputFormatEnum _reportFormat = OutputFormatEnum.Unknown;
        string _destFilename = "";

        private void ClearRunVariables()
        {
            this.Report = null;
            this.ReportFilepath = "";

            this.ExecutionSuccessful = false;
            this.FailedMessage = "UNKNOWN";
            this.FailedException = new Exception("Unknown exception");
        }

        /// <summary>
        /// Executes Xteq5 without generating a report
        /// </summary>
        /// <param name="CompilationPath">Path to the compilation folder</param>
        public SimplifiedXteq5Runner(string CompilationPath)
        {
            _compilationPath = CompilationPath;
            _reportFormat = OutputFormatEnum.Unknown;
            _userText = "";
            _destFilename = "";

            ClearRunVariables();
        }


        /// <summary>
        /// Executes Xteq5 and generates a HTML report for it
        /// </summary>
        /// <param name="CompilationPath">Path to the compilation folder</param>        
        /// <param name="UserText">Optional text to be added to the generated HTML file</param>        
        public SimplifiedXteq5Runner(string CompilationPath, string UserText)
            : this(CompilationPath)
        {
            _userText = UserText;
            _reportFormat = OutputFormatEnum.HTML;
        }

        /// <summary>
        /// Executes Xteq5 and generates a report in the given format for it. 
        /// </summary>
        /// <param name="CompilationPath">Path to the compilation folder</param>        
        /// <param name="UserText">Optional text to be added to the generated HTML file</param>        
        /// <param name="ReportFormat">Format the report should have</param>
        public SimplifiedXteq5Runner(string CompilationPath, string UserText, OutputFormatEnum ReportFormat)
            : this(CompilationPath, UserText)
        {
            _reportFormat = ReportFormat; //This will later on checked by OutputGenerator anyway, so no need to check this here
        }

        /// <summary>
        /// Executes Xteq5, generates a report in the given format for it and save the result to Filename 
        /// </summary>
        /// <param name="CompilationPath">Path to the compilation folder</param>        
        /// <param name="UserText">Optional text to be added to the generated HTML file</param>        
        /// <param name="ReportFormat">Format the report should have</param>
        public SimplifiedXteq5Runner(string CompilationPath, string UserText, OutputFormatEnum ReportFormat, string Filename)
            : this(CompilationPath, UserText, ReportFormat)
        {
            _destFilename = Filename;
        }


        /// <summary>
        /// TRUE if the Execution (generally) was successful. This does NOT mean that all test or assets were executed successful.
        /// </summary>
        public bool ExecutionSuccessful { get; private set; }

        /// <summary>
        /// Reference to the generated Xteq5 report
        /// </summary>
        public Report Report { get; private set; }

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
        public bool Run(IProgress<RunnerProgressDetail> ProgressRunner = null, IProgress<ReportCreationProgress> ProgressReportCreation = null)
        {
            Task<bool> task = RunAsync(ProgressRunner);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Generates the report and (if set) the file report
        /// </summary>
        /// <param name="ProgressRunner">Report status using this Progress object</param>        
        /// <returns>TRUE if execution was successful</returns>
        public async Task<bool> RunAsync(IProgress<RunnerProgressDetail> ProgressRunner = null, IProgress<ReportCreationProgress> ProgressReportCreation = null)
        {
            ClearRunVariables();

            bool successful = false;

            try
            {
                //Let Xteq5Runner do it's thing...
                Xteq5Runner runner = new Xteq5Runner();                                
                this.Report = await runner.RunAsync(_compilationPath, ProgressRunner);

                this.Report.UserText = _userText;

                //MTH: That a second progress is used here is because I know at least one AV software that gets crazy when an unknown EXE saves an HTML file with <script> tags in it. 
                //I don't know why, but the report creation takes AGES / A VERY LONG TIME / F**** BAZILLION SECONDS with that POS software installed.                                
                if (_reportFormat != OutputFormatEnum.Unknown)
                {
                    ProgressReporter<ReportCreationProgress> reporterStart = new ProgressReporter<ReportCreationProgress>(ProgressReportCreation);
                    reporterStart.Content.Action = ReportAction.Starting;
                    reporterStart.Report();


                    //Generate the report
                    this.ReportFilepath = OutputGenerator.GenerateReportOutputFile(this.Report, _reportFormat, _destFilename);


                    ProgressReporter<ReportCreationProgress> reporterEnd = new ProgressReporter<ReportCreationProgress>(ProgressReportCreation);
                    reporterEnd.Content.Action = ReportAction.Ended;
                    reporterEnd.Report();

                }

                successful = true;
            }

            catch (FileNotFoundException fnfe)
            {
                //In most cases this means we couldn't load System.Management.Automation. 
                if (fnfe.Message.Contains("System.Management.Automation"))
                {
                    //We need to explain to the user that this means PowerShell is missing. 
                    this.FailedMessage = "PowerShell is required, but could not be loaded. Please re-run Setup and follow the provided link to install it.";
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
                //The given compilation folder was not found. 
                this.FailedMessage = "The selected compilation folder does not exist";
                this.FailedException = cfnfe;
            }
            catch (CompilationSubFolderException csfnfe)
            {
                //A sub folder was not found. 
                this.FailedMessage = "A required subfolder of the selected compilation folder does not exist";
                this.FailedException = csfnfe;
            }
            catch (PSTestFailedException pstfe)
            {
                //The powershell test failed
                this.FailedMessage = "Testing the PowerShell environment failed. ";
                this.FailedException = pstfe;
            }
            catch (TemplateFileNotFoundException tfnfe)
            {
                //Template file does not exist in folder 
                this.FailedMessage = "The template file for the report is missing. Please re-run Setup to install it.";
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
