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
    /// Internal class that is used to run a script and process the output of it. 
    /// </summary>
    internal abstract class BaseScriptRunner
    {

        protected async Task RunInternalAsync(PSScriptRunner ScriptRunner, string ScriptDirectory)
        {

            string[] allScripts = Directory.GetFiles(ScriptDirectory, TestUtilConstant.ScriptFilePattern);
            NaturalSort.Sort(allScripts);

            foreach (string scriptFilename in allScripts)
            {
                //Set the values we already have
                BaseRecord record = new BaseRecord();
                record.ScriptFilePath = scriptFilename;
                record.Name = Path.GetFileNameWithoutExtension(scriptFilename);

                try
                {
                    Debug.WriteLine("Executing script " + scriptFilename);

                    using (ExecutionResult execResult = await ScriptRunner.RunScriptFileAsync(scriptFilename))
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
        }


        void ProcessHashtableOutputInternal(BaseRecord Record, Hashtable Table)
        {
            //We need a a key "Data" or this is considered to be fatal. The value of this key can still be NULL. 
            if (Table.ContainsKey(TestUtilConstant.ReturnedHashtableKeyData) == false)
            {
                Record.Conclusion = ConclusionEnum.Fatal;
                Record.AddLineToProcessMessages("Data key missing from returned hashtable");

                ProcessFailure(Record);
            }
            else
            {
                string name = GetStringFromHashtable(Table, TestUtilConstant.ReturnedHashtableKeyName);
                if (string.IsNullOrEmpty(name) == false) 
                {
                    Record.Name = name;
                }

                string text = GetStringFromHashtable(Table, TestUtilConstant.ReturnedHashtableKeyText);
                if (string.IsNullOrEmpty(text) == false)
                {
                    Record.Description = text;
                }

                string data = GetStringFromHashtable(Table, TestUtilConstant.ReturnedHashtableKeyData);
                if (string.IsNullOrEmpty(data))
                {
                    //Empty data means DoesNotApply
                    Record.Conclusion = ConclusionEnum.DoesNotApply;
                    
                    ProcessEmptyData(Record, Table);
                }
                else
                {
                    //The data key contains something. First try if the result is maybe the string "n/a"
                    ConclusionEnum conclusion = ConclusionEnumConverter.ParseConclusion(data);
                    if (conclusion == ConclusionEnum.DoesNotApply)
                    {
                        //The script returned n/a (DoesNotApply), so it can be processed as empty record
                        Record.Conclusion = ConclusionEnum.DoesNotApply;
                        ProcessEmptyData(Record, Table);
                    }
                    else
                    {
                        //The result was something else. Let the implementation decide.
                        ProcessNonEmptyData(Record, Table, data);
                    }
                }


            }
        }


        protected Object GetObjectFromHashtable(Hashtable Table, string KeyName)
        {
            if (Table.ContainsKey(KeyName))
            {
                return Table[KeyName];
            }
            else
            {
                return null;
            }
        }

        protected string GetStringFromHashtable(Hashtable Table, string KeyName)
        {
            Object obj = GetObjectFromHashtable(Table, KeyName);

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


        protected abstract void ProcessFailure(BaseRecord Record);

        protected abstract void ProcessEmptyData(BaseRecord Record, Hashtable Table);

        protected abstract void ProcessNonEmptyData(BaseRecord Record, Hashtable Table, string DataKeyValue);


    }
}
