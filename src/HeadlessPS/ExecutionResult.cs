using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Yamua;
using System.Collections.ObjectModel;
using System.IO;
using System.CodeDom.Compiler;

namespace HeadlessPS
{
    /// <summary>
    /// This class encapsulates the results of a PowerShell script that was executed using PSScriptRunner. 
    /// You are free to use this object until the PSScriptRunner instance, that has create an instance of this class, runs another script. At that moment, the values of this object might be gone, broken or corrupt. 
    /// </summary>
    public class ExecutionResult : DisposableObject
    {
        private ExecutionResult()
        {

        }

        /// <summary>
        /// Returns TRUE if the execution was successful. Please note that successful means: "From the view of PowerShell". For example, a script with the command "Xsdhjfsjk23" but $ErrorActionPreference='Continue' will also be reported as successful.
        /// Check SuccessfulNoError if reported errors should be considered as an unsuccessful execution.
        /// </summary>
        public bool Successful { get; private set; }

        /// <summary>
        /// Returns TRUE if the execution was suceessful and no error was found. 
        /// </summary>
        public bool SuccessfulNoError { get; private set; }

        /// <summary>
        /// Returns the fatal exception that caused the execution to fail. Can be NULL. 
        /// </summary>
        public Exception FailedException { get; private set; }

        PSInvocationState _pipelineState;

        /// <summary>
        /// Returns the PSObjects in the OUTPUT stream (#1)
        /// </summary>
        public PSDataCollection<PSObject> StreamOutput { get; private set; }

        /// <summary>
        /// Returns the ErrorRecord objects in the ERROR stream (#2)
        /// </summary>
        public PSDataCollection<ErrorRecord> StreamError { get; private set; }

        /// <summary>
        /// Returns the WarningRecord objects in the WARNING stream (#3)
        /// </summary>
        public PSDataCollection<WarningRecord> StreamWarning { get; private set; }

        /// <summary>
        /// Returns the VerboseRecord objects in the VERBOSE stream (#4)
        /// </summary>
        public PSDataCollection<VerboseRecord> StreamVerbose { get; private set; }

        /// <summary>
        /// Returns the DebugRecord objects in the DEBUG stream (#5)
        /// </summary>
        public PSDataCollection<DebugRecord> StreamDebug { get; private set; }

        /// <summary>
        /// Returns the variables that were passed in and value
        /// </summary>
        public HashSet<VariablePlain> Variables { get; private set; }


        protected internal ExecutionResult(PowerShell psInstance, HashSet<VariablePlain> vars, PSDataCollection<PSObject> psOutput)
            : this(psInstance, vars, psOutput, null)
        {

        }

        protected internal ExecutionResult(PowerShell psInstance, HashSet<VariablePlain> vars, Exception additionalException)
            : this(psInstance, vars, null, additionalException)
        {

        }


        //MTH: This is done in a constructor and not a function to make sure the consumer either gets a working instance or no instance (null) at all. 
        protected internal ExecutionResult(PowerShell psInstance, HashSet<VariablePlain> vars, PSDataCollection<PSObject> psOutput, Exception additionalException)
        {
            Successful = false;
            SuccessfulNoError = false;

            //It can happen that PSInstance is NULL 
            if (psInstance != null)
            {
                _pipelineState = psInstance.InvocationStateInfo.State;

                //Check if we have an inner or outer exception
                if (additionalException == null && psInstance.InvocationStateInfo.Reason == null)
                {
                    FailedException = null;
                }
                else
                {
                    //At least one excpetion has been detected. The additonal exception is raised by the the call of .EndInvoke() and is therefore considered to be more important.
                    if (additionalException != null)
                    {
                        FailedException = additionalException;
                    }
                    else
                    {
                        FailedException = psInstance.InvocationStateInfo.Reason;
                    }
                }

            }
            else
            {
                //PSInstance is null - execution can not be successful in this case
                _pipelineState = PSInvocationState.NotStarted;

                if (additionalException != null)
                {
                    FailedException = additionalException;
                }
                else
                {
                    //OK, this is strange. We do not have a PSInstance but also no AdditonalException. Set a default value. 
                    FailedException = new Exception("No detailed exception available");
                }
            }


            //If FailedException is not set, we can check if the execution was successful 
            if (FailedException == null)
            {
                //No exception detected, if State is Complete we have a succesful execution.
                if (psInstance.InvocationStateInfo.State == PSInvocationState.Completed)
                {
                    Successful = true;

                    //Now also check the ERROR stream. If there are no error reported, we also have a SuccessfulNoError status
                    if (psInstance.Streams.Error != null)
                    {
                        if (psInstance.Streams.Error.Count == 0)
                        {
                            SuccessfulNoError = true;
                        }

                    }
                    else
                    {
                        //Error stream is NULL, hence no problem
                        SuccessfulNoError = true;
                    }

                }
            }

            //MTH: Assign the stream collections to our internal properties or create empty one if they are null.
            //This will make using this object easier since the consumer does not need to check if the collections are null.                 
            StreamOutput = psOutput ?? new PSDataCollection<PSObject>();

            //MTH: I didn't created a default value for all the StreamXXX properties here because PSDataCollection implements IDisposable. Therefore this odd looking code. 
            if (psInstance == null)
            {
                //When PS Instance is NULL, set all values to default 
                StreamError = new PSDataCollection<ErrorRecord>(); //http://msdn.microsoft.com/en-us/library/System.Management.Automation.ErrorRecord%28v=vs.85%29.aspx
                StreamWarning = new PSDataCollection<WarningRecord>();  //http://msdn.microsoft.com/en-us/library/system.management.automation.warningrecord%28v=vs.85%29.aspx
                StreamVerbose = new PSDataCollection<VerboseRecord>(); //http://msdn.microsoft.com/en-us/library/system.management.automation.verboserecord%28v=vs.85%29.aspx
                StreamDebug = new PSDataCollection<DebugRecord>();  //http://msdn.microsoft.com/en-us/library/system.management.automation.debugrecord%28v=vs.85%29.aspx

                //Set the variables to the same collection which was passed in
                Variables = vars;
            }
            else
            {
                //PSInstance is not null, so we can check the streams
                StreamError = psInstance.Streams.Error ?? new PSDataCollection<ErrorRecord>();
                StreamWarning = psInstance.Streams.Warning ?? new PSDataCollection<WarningRecord>();
                StreamVerbose = psInstance.Streams.Verbose ?? new PSDataCollection<VerboseRecord>();
                StreamDebug = psInstance.Streams.Debug ?? new PSDataCollection<DebugRecord>();  

                //Return the current value of the Variables that were passed in. 
                Variables = new HashSet<VariablePlain>();

                if (vars.Count > 0)
                {
                    foreach (VariablePlain var in vars)
                    {
                        //If the value was marked as ReadOnly, we simply take it from the original hashset because PowerShell can't change it anyway
                        if (var.ReadOnly)
                        {
                            Variables.Add(var);
                        }
                        else
                        {
                            //The variable could have been changed by the script, so retrieve it from PowerShell

                            /* MTH: There are two functions to read variables from PowerShell:
                             * PSVariable psv = PSInstance.Runspace.SessionStateProxy.PSVariable.Get("aTestVar"); //Docs: http://msdn.microsoft.com/en-us/library/system.management.automation.runspaces.sessionstateproxy.psvariable%28v=vs.85%29.aspx
                             * Object obj = PSInstance.Runspace.SessionStateProxy.GetVariable("aTestVar"); //Docs: http://msdn.microsoft.com/en-us/library/system.management.automation.runspaces.sessionstateproxy.getvariable%28v=vs.85%29.aspx
                             * I assume that the second one is the short-circuit version of the first one. Since we only want the value, we use it.
                             */

                            Object value = psInstance.Runspace.SessionStateProxy.GetVariable(var.Name);
                            VariablePlain new_var = new VariablePlain(var.Name, value);
                            Variables.Add(new_var);
                        }

                    }
                }

            }



        }


        protected override void DisposeManagedResources()
        {
            if (StreamOutput != null)
            {
                StreamOutput.Dispose();
                StreamOutput = null;
            }

            if (StreamError != null)
            {
                StreamError.Dispose();
                StreamError = null;
            }

            if (StreamWarning != null)
            {
                StreamWarning.Dispose();
                StreamWarning = null;
            }

            if (StreamVerbose != null)
            {
                StreamVerbose.Dispose();
                StreamVerbose = null;
            }

            if (StreamDebug != null)
            {
                StreamDebug.Dispose();
                StreamDebug = null;
            }

            base.DisposeManagedResources();
        }

        public override string ToString()
        {
            StringBuilder sbuilder = new StringBuilder();

            sbuilder.Append("Script status: ");
            if (Successful)
            {
                sbuilder.Append("Executed successfully");

                //IF SuccessfulNoError is FALSE, we need to report this
                if (SuccessfulNoError == false)
                {
                    sbuilder.Append(", but errors were reported");
                }

                sbuilder.AppendLine();

            }
            else
            {
                sbuilder.Append("Failed");
                //If _pipelineState is FAILED, we ignore this. Else the result is "Failed [Pipeline state FAILED]"
                if (_pipelineState != PSInvocationState.Failed)
                {
                    sbuilder.Append(string.Format(" [Pipeline state {0}]", _pipelineState));
                }
                sbuilder.AppendLine();

                sbuilder.AppendLine("-EXCEPTION------------------------");
                if (FailedException != null)
                {
                    sbuilder.AppendLine(FailedException.ToString());
                }
                else
                {
                    sbuilder.AppendLine("No exception found");
                }
                sbuilder.AppendLine("----------------------------------");
            }
            sbuilder.AppendLine();

            Collection<UniversalStreamRecord> universalRecordCollection;

            //MTH: We are moving down the streams in this order: Output(1), Error(2), Warning(3), Verbose(4), Debug(5) and finally the Variables
            //See [Understanding Streams, Redirection, and Write-Host in PowerShell](blogs.technet.com/b/heyscriptingguy/archive/2014/03/30/understanding-streams-redirection-and-write-host-in-powershell.aspx)                               
            universalRecordCollection = UniversalStreamRecord.FromPSObjectPSCollection(StreamOutput);
            sbuilder.AppendLine(UniversalStreamRecordCollectionToString(universalRecordCollection, PSStreamNameEnum.Output));

            //MTH: This will list all non-terminating errors. In fact, these ARE real errors but normally $ErrorActionPreference is set to "Continue" - which is the default.
            //If set to $ErrorActionPreference = "Stop", any error will become terminating.         
            //See: [Managing non-terminating errors](http://blogs.msdn.com/b/powershell/archive/2006/04/25/583241.aspx)
            universalRecordCollection = UniversalStreamRecord.FromErrorRecordPSCollection(StreamError);
            sbuilder.AppendLine(UniversalStreamRecordCollectionToString(universalRecordCollection, PSStreamNameEnum.Error));

            //Only contains entires when $WarningPreference = 'Continue' - this is the default in PowerShell 
            universalRecordCollection = UniversalStreamRecord.FromInformationalRecordPSCollection(StreamWarning);
            sbuilder.AppendLine(UniversalStreamRecordCollectionToString(universalRecordCollection, PSStreamNameEnum.Warning));

            //Only contains entries when $VerbosePreference = 'Continue'
            universalRecordCollection = UniversalStreamRecord.FromInformationalRecordPSCollection(StreamVerbose);
            sbuilder.AppendLine(UniversalStreamRecordCollectionToString(universalRecordCollection, PSStreamNameEnum.Verbose));

            //Only contains entries when $DebugPreference = 'Continue'
            universalRecordCollection = UniversalStreamRecord.FromInformationalRecordPSCollection(StreamDebug);
            sbuilder.AppendLine(UniversalStreamRecordCollectionToString(universalRecordCollection, PSStreamNameEnum.Debug));

            //Add the variables but only if read/write variables were passed in (second paramter FALSE)
            universalRecordCollection = UniversalStreamRecord.FromVariablePlainHashSet(Variables, false);
            sbuilder.AppendLine(UniversalStreamRecordCollectionToString(universalRecordCollection, "Writeable variables:", "No writeable variable exist"));

            return sbuilder.ToString();
        }


        string UniversalStreamRecordCollectionToString(Collection<UniversalStreamRecord> collection, PSStreamNameEnum streamNameEnum)
        {
            string streamName = streamNameEnum.ToString().ToUpper();
            string streamID = ((int)streamNameEnum).ToString();

            string objectsFoundMessage = string.Format("Objects in stream {0} (Stream ID {1}):", streamName, streamID);
            string noObjectsFoundMessage = string.Format("Nothing in stream {0} (Stream ID {1})", streamName, streamID);

            return UniversalStreamRecordCollectionToString(collection, objectsFoundMessage, noObjectsFoundMessage);
        }


        string UniversalStreamRecordCollectionToString(Collection<UniversalStreamRecord> collection, string objectsFoundMessage, string noObjectsFoundMessage)
        {
            /* MTH: The old code was:
             * using (StringWriter baseBuffer = new System.IO.StringWriter()) {
             *    using (IndentedTextWriter indentWriter = new IndentedTextWriter(baseBuffer, "    ")) 
             * but this resulted in Code Review message 
             *    CA2202: Do not dispose objects multiple time.
             * Therefore I changed it. 
             */
            using (IndentedTextWriter indentWriter = new IndentedTextWriter(new System.IO.StringWriter(), "    ")) //Use four whitespaces as indent, this is the same amount the PowerShell help uses
            {
                indentWriter.Indent = 0;

                if (collection != null && collection.Count > 0)
                {
                    indentWriter.WriteLine(objectsFoundMessage);
                    indentWriter.Indent++;

                    foreach (UniversalStreamRecord unirecord in collection)
                    {
                        indentWriter.WriteLine(unirecord.Message);
                        indentWriter.Indent++;

                        InvocationInfo invocation = unirecord.InvocationInfo;
                        if (invocation == null)
                        {
                            //Do not display anything as InvocationInfo is NULL so we do not have any information to display
                        }
                        else
                        {
                            indentWriter.Write("from: ");

                            //It seems that InvocationInfo.Line normally contains better informationthan than InvocationInfo.MyCommand.Definition
                            if (string.IsNullOrWhiteSpace(invocation.Line))
                            {
                                if (invocation.MyCommand != null)
                                {
                                    indentWriter.WriteLine(ReturnLiteralNoneInAngleBracketsIfEmpty(invocation.MyCommand.Definition));
                                }
                                else
                                {
                                    indentWriter.WriteLine(LiteralNoneInAngleBrackets);
                                }

                            }
                            else
                            {
                                indentWriter.WriteLine(invocation.Line);
                            }

                            //If we have a PositionMessage, we will display it. Else we do not display it at all
                            if (string.IsNullOrWhiteSpace(invocation.PositionMessage) == false)
                            {
                                indentWriter.WriteLine(invocation.PositionMessage);
                            }

                            if (invocation.MyCommand != null)
                            {
                                indentWriter.Write("Source: ");
                                indentWriter.Write(ReturnLiteralNoneInAngleBracketsIfEmpty(invocation.MyCommand.Name));
                                indentWriter.Write("; ");

                                //Check if the we have a module name. If not, ignore it 
                                if (string.IsNullOrWhiteSpace(invocation.MyCommand.ModuleName) == false)
                                {
                                    indentWriter.Write("Module: ");
                                    indentWriter.Write(invocation.MyCommand.ModuleName);
                                    indentWriter.Write("; ");
                                }

                                //For Power Shell each script starts at line 1, so this is ignored if the value is 0
                                if (invocation.ScriptLineNumber > 0)
                                {
                                    indentWriter.Write("Line: ");
                                    indentWriter.Write(invocation.ScriptLineNumber);
                                    indentWriter.Write("; ");
                                }
                            }

                            //If PowerShell has a script name, we will display it 
                            if (string.IsNullOrWhiteSpace(invocation.ScriptName) == false)
                            {
                                indentWriter.Write("Script: ");
                                indentWriter.Write(ReturnLiteralNoneInAngleBracketsIfEmpty(invocation.ScriptName));
                                indentWriter.WriteLine("; ");
                            }
                        }

                        indentWriter.Indent--;
                    }

                }
                else
                {
                    indentWriter.Write(noObjectsFoundMessage);

                }

                indentWriter.Flush();
                return indentWriter.InnerWriter.ToString();
            }
        }


        string ReturnLiteralNoneInAngleBracketsIfEmpty(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? LiteralNoneInAngleBrackets : input;
        }

        readonly string LiteralNoneInAngleBrackets = "<None>";

    }
}
