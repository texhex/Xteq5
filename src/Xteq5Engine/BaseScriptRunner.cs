using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using HeadlessPS;
using Yamua;

namespace Xteq5
{
    /// <summary>
    /// Internal class that is used to run the scripts in a folder and process the output of those scripts
    /// </summary>
    internal abstract class BaseScriptRunner
    {

        ProgressReporter<RunnerProgressDetail> _reporter;

        void ReportProgressScript(string filepath)
        {
            _reporter.Content.Action = RunnerAction.ScriptRunning;
            _reporter.Content.ScriptFilepath = filepath;
            _reporter.Content.ScriptFilename = PathExtension.Filename(filepath);
            _reporter.Report();
        }
        
        protected async Task RunInternalAsync(PSScriptRunner scriptRunner, string scriptDirectory, IProgress<RunnerProgressDetail> progress = null)
        {
            //Assign progress reporter and set it that it can be used after calling Report()
            _reporter = new ProgressReporter<RunnerProgressDetail>(progress, createNewInstanceAfterReport:true);

            //Report that we are about to start
            _reporter.Content.Action = RunnerAction.Starting;
            _reporter.Report();


            string[] allScripts = Directory.GetFiles(scriptDirectory, Xteq5EngineConstant.ScriptFilePattern);
            NaturalSort.Sort(allScripts);

            foreach (string scriptFilename in allScripts)
            {
                //Set the values we already have
                BaseRecord record = new BaseRecord();
                record.ScriptFilePath = scriptFilename;
                record.Name = Path.GetFileNameWithoutExtension(scriptFilename);

                try
                {
                    //Report back status
                    ReportProgressScript(scriptFilename);

                    using (ExecutionResult execResult = await scriptRunner.RunScriptFileAsync(scriptFilename))
                    {
                        record.ProcessMessages = execResult.ToString();

                        if (execResult.Successful == false)
                        {
                            ProcessFailure(record);
                        }
                        else
                        {
                            //The script was run OK
                            record.ScriptSuccessful = true;


                            //Search the output 
                            Hashtable table = new Hashtable(); //new() is required or the compiler will complain about an unassigned local variable
                            bool foundHashtable = false;

                            //Search the output collection for a hashtable
                            foreach (PSObject psobj in execResult.StreamOutput)
                            {
                                if (psobj.BaseObject != null && psobj.BaseObject is Hashtable)
                                {
                                    foundHashtable = true;
                                    table = psobj.BaseObject as Hashtable;
                                    break;
                                }
                            }

                            if (foundHashtable == false)
                            {
                                //No hashtable was found within output stream. Add this to messages
                                record.AddLineToProcessMessages("No Hashtable was returned");

                                ProcessFailure(record);
                            }
                            else
                            {
                                ProcessHashtableOutputInternal(record, table);
                            }

                        }

                    }

                }
                catch (Exception exc)
                {
                    //That didn't go well                   
                    record.ProcessMessages = exc.ToString();

                    //Let the actual implementation decide what to do next
                    ProcessFailure(record);
                }


                //MTH: End of processing with this file. Next one please!
            }


            //Report status that this entire run has finished
            _reporter.Content.Action = RunnerAction.Ended;
            _reporter.Report();
        }


        void ProcessHashtableOutputInternal(BaseRecord record, Hashtable table)
        {
            //We need a a key "Data" or this is considered to be fatal. The value of this key can still be NULL. 
            if (table.ContainsKey(Xteq5EngineConstant.ReturnedHashtableKeyData) == false)
            {
                record.Conclusion = ConclusionEnum.Fatal;
                record.AddLineToProcessMessages("Data key missing from returned hashtable");

                ProcessFailure(record);
            }
            else
            {
                string name = GetStringFromHashtable(table, Xteq5EngineConstant.ReturnedHashtableKeyName);
                if (string.IsNullOrEmpty(name) == false)
                {
                    record.Name = name;
                }

                string text = GetStringFromHashtable(table, Xteq5EngineConstant.ReturnedHashtableKeyText);
                if (string.IsNullOrEmpty(text) == false)
                {
                    record.Description = text;
                }

                string data = GetStringFromHashtable(table, Xteq5EngineConstant.ReturnedHashtableKeyData);
                if (string.IsNullOrEmpty(data))
                {
                    //Empty data means DoesNotApply
                    record.Conclusion = ConclusionEnum.DoesNotApply;

                    ProcessEmptyData(record, table);
                }
                else
                {
                    //The data key contains something. First try if the result is maybe the string "n/a"
                    ConclusionEnum conclusion = ConclusionEnumConverter.ParseConclusion(data);
                    if (conclusion == ConclusionEnum.DoesNotApply)
                    {
                        //The script returned n/a (DoesNotApply), so it can be processed as an empty record
                        record.Conclusion = ConclusionEnum.DoesNotApply;
                        ProcessEmptyData(record, table);
                    }
                    else
                    {
                        //The result was something else. Let the implementation decide.
                        ProcessNonEmptyData(record, table, data);
                    }
                }


            }
        }


        protected Object GetObjectFromHashtable(Hashtable table, string keyName)
        {
            if (table.ContainsKey(keyName))
            {
                return table[keyName];
            }
            else
            {
                return null;
            }
        }

        protected string GetStringFromHashtable(Hashtable table, string keyName)
        {
            Object obj = GetObjectFromHashtable(table, keyName);

            if (obj != null)
            {
                string s = obj.ToString().Trim(); //Better be safe than sorry 
                
                if (string.IsNullOrWhiteSpace(s))
                {
                    return string.Empty;
                }
                else
                {
                    return s;
                }
            }
            else
            {
                return string.Empty;
            }
        }


        protected abstract void ProcessFailure(BaseRecord record);

        protected abstract void ProcessEmptyData(BaseRecord record, Hashtable table);

        protected abstract void ProcessNonEmptyData(BaseRecord record, Hashtable table, string dataKeyValue);


    }
}
