using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessPS
{
    /// <summary>
    /// This class is used to unifiy the access to the different stream records PowerShell uses.
    /// DebugRecord, VerboseRecord and WarningRecord have the base type InformationalRecord which contains .Message and .InvocationInfo. 
    /// ErrorRecord also has .InvocationInfo, but no .Message. Therefore this classes uses ErrorRecord.Exception.ToString() for .Message
    /// There is no OutputRecord as the output depeneds on the commands used within PowerShell. To allow access also for any output, the static function FromPSObject() can be used. .InvocationInfo will be NULL in this case. 
    /// </summary>
    public class UniversalStreamRecord
    {
        private UniversalStreamRecord()
        {
            Message = "";
            InvocationInfo = null;

        }

        public UniversalStreamRecord(string message, InvocationInfo invocationInfo)
        {
            this.Message = message;
            this.InvocationInfo = invocationInfo;
        }

        public string Message { get; set; }

        public InvocationInfo InvocationInfo { get; set; }






        public static UniversalStreamRecord FromWarningRecord(WarningRecord record)
        {
            return FromInformationalRecord(record);
        }

        public static UniversalStreamRecord FromVerboseRecord(VerboseRecord record)
        {
            return FromInformationalRecord(record);
        }

        public static UniversalStreamRecord FromDebugRecord(DebugRecord record)
        {
            return FromInformationalRecord(record);
        }

        public static UniversalStreamRecord FromInformationalRecord(InformationalRecord record)
        {
            return new UniversalStreamRecord(record.Message, record.InvocationInfo);
        }

        public static UniversalStreamRecord FromErrorRecord(ErrorRecord record)
        {
            return new UniversalStreamRecord(record.Exception.ToString(), record.InvocationInfo);
        }

        public static UniversalStreamRecord FromPSObject(PSObject psObj)
        {
            return UniversalStreamRecord.FromObject(psObj.BaseObject);
        }

        public static UniversalStreamRecord FromVariablePlain(VariablePlain variable)
        {
            UniversalStreamRecord usr = new UniversalStreamRecord();
            usr.Message = "$" + variable.Name + ": " + ObjectToString(variable.Value);
            return usr;
        }

        public static UniversalStreamRecord FromObject(Object obj)
        {
            UniversalStreamRecord usr = new UniversalStreamRecord();
            usr.Message = ObjectToString(obj);
            return usr;
        }

        private static string ObjectToString(Object obj)
        {
            if (obj != null)
            {
                //Check if the result is an IDictionary (e.g. Hashtable). In this case we will return the items within the dictionary                
                if (obj is IDictionary)
                {
                    IDictionary idc = obj as IDictionary;

                    //See [IDictionary to string](http://stackoverflow.com/questions/12197089/idictionary-to-string) by [Daniel Hilgarth](http://blog.fire-development.com/)
                    string entries = idc.Cast<DictionaryEntry>()
                                .Aggregate(new StringBuilder(),
                                (sb, x) => sb.Append(" ").Append(x.Key.ToString()).Append(": ").Append(x.Value).Append(";"),
                                sb => sb.ToString());

                    return "(" + obj.GetType().ToString() + ") =>" + entries;
                }
                else
                {
                    if (obj is string)
                    {
                        //If it's a string, display as is without type description
                        return obj.ToString();
                    }
                    else
                    {
                        //"No idea how to display it better" format...
                        return string.Format("{0} ({1})", obj.ToString(), obj.GetType().ToString());
                    }
                }
            }
            else
            {
                return "NULL";
            }
        }

        public static Collection<UniversalStreamRecord> FromInformationalRecordPSCollection<T>(PSDataCollection<T> collection) where T : InformationalRecord
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (collection != null && collection.Count > 0)
            {
                foreach (InformationalRecord ir in collection)
                {
                    UniversalStreamRecord record = UniversalStreamRecord.FromInformationalRecord(ir);
                    collectionUniversalStreamRecord.Add(record);
                }
            }

            return collectionUniversalStreamRecord;
        }

        public static Collection<UniversalStreamRecord> FromErrorRecordPSCollection(PSDataCollection<ErrorRecord> collection)
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (collection != null && collection.Count > 0)
            {
                foreach (ErrorRecord er in collection)
                {
                    UniversalStreamRecord record = UniversalStreamRecord.FromErrorRecord(er);
                    collectionUniversalStreamRecord.Add(record);
                }
            }

            return collectionUniversalStreamRecord;
        }


        public static Collection<UniversalStreamRecord> FromPSObjectPSCollection(PSDataCollection<PSObject> collection)
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (collection != null && collection.Count > 0)
            {
                foreach (PSObject psobj in collection)
                {
                    UniversalStreamRecord record = UniversalStreamRecord.FromPSObject(psobj);
                    collectionUniversalStreamRecord.Add(record);
                }
            }

            return collectionUniversalStreamRecord;
        }


        public static Collection<UniversalStreamRecord> FromVariablePlainHashSet(HashSet<VariablePlain> collection, bool includeReadOnly=true)
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (collection != null && collection.Count > 0)
            {
                foreach (VariablePlain var in collection)
                {
                    if (var.ReadOnly & includeReadOnly==false)
                    {
                        //Do nothing as ReadOnly entries should be ignored
                    }
                    else
                    {
                        UniversalStreamRecord record = UniversalStreamRecord.FromVariablePlain(var);
                        collectionUniversalStreamRecord.Add(record);
                    }
                }
            }

            return collectionUniversalStreamRecord;
        }

    }
}
