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

        public UniversalStreamRecord(string Message, InvocationInfo InvocationInfo)
        {
            this.Message = Message;
            this.InvocationInfo = InvocationInfo;
        }

        public string Message { get; set; }

        public InvocationInfo InvocationInfo { get; set; }






        public static UniversalStreamRecord FromWarningRecord(WarningRecord Record)
        {
            return FromInformationalRecord(Record);
        }

        public static UniversalStreamRecord FromVerboseRecord(VerboseRecord Record)
        {
            return FromInformationalRecord(Record);
        }

        public static UniversalStreamRecord FromDebugRecord(DebugRecord Record)
        {
            return FromInformationalRecord(Record);
        }

        public static UniversalStreamRecord FromInformationalRecord(InformationalRecord Record)
        {
            return new UniversalStreamRecord(Record.Message, Record.InvocationInfo);
        }

        public static UniversalStreamRecord FromErrorRecord(ErrorRecord Record)
        {
            return new UniversalStreamRecord(Record.Exception.ToString(), Record.InvocationInfo);
        }

        public static UniversalStreamRecord FromPSObject(PSObject PSObj)
        {
            return UniversalStreamRecord.FromObject(PSObj.BaseObject);
        }

        public static UniversalStreamRecord FromVariablePlain(VariablePlain Variable)
        {
            UniversalStreamRecord usr = new UniversalStreamRecord();
            usr.Message = "$" + Variable.Name + ": " + ObjectToString(Variable.Value);
            return usr;
        }

        public static UniversalStreamRecord FromObject(Object Obj)
        {
            UniversalStreamRecord usr = new UniversalStreamRecord();
            usr.Message = ObjectToString(Obj);
            return usr;
        }

        private static string ObjectToString(Object Obj)
        {
            if (Obj != null)
            {
                //Check if the result is an IDictionary (e.g. Hashtable). In this case we will return the items within the dictionary                
                if (Obj is IDictionary)
                {
                    IDictionary idc = Obj as IDictionary;

                    //See [IDictionary to string](http://stackoverflow.com/questions/12197089/idictionary-to-string) by [Daniel Hilgarth](http://blog.fire-development.com/)
                    string entries = idc.Cast<DictionaryEntry>()
                                .Aggregate(new StringBuilder(),
                                (sb, x) => sb.Append(" ").Append(x.Key.ToString()).Append(": ").Append(x.Value).Append(";"),
                                sb => sb.ToString());

                    return Obj.GetType().ToString() + " =>" + entries;
                }
                else
                {
                    //"No idea how to display it better" format...
                    return string.Format("{0} ({1})", Obj.ToString(), Obj.GetType().ToString());
                }
            }
            else
            {
                return "NULL";
            }
        }

        public static Collection<UniversalStreamRecord> FromInformationalRecordPSCollection<T>(PSDataCollection<T> Collection) where T : InformationalRecord
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (Collection != null && Collection.Count > 0)
            {
                foreach (InformationalRecord ir in Collection)
                {
                    UniversalStreamRecord record = UniversalStreamRecord.FromInformationalRecord(ir);
                    collectionUniversalStreamRecord.Add(record);
                }
            }

            return collectionUniversalStreamRecord;
        }

        public static Collection<UniversalStreamRecord> FromErrorRecordPSCollection(PSDataCollection<ErrorRecord> Collection)
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (Collection != null && Collection.Count > 0)
            {
                foreach (ErrorRecord er in Collection)
                {
                    UniversalStreamRecord record = UniversalStreamRecord.FromErrorRecord(er);
                    collectionUniversalStreamRecord.Add(record);
                }
            }

            return collectionUniversalStreamRecord;
        }


        public static Collection<UniversalStreamRecord> FromPSObjectPSCollection(PSDataCollection<PSObject> Collection)
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (Collection != null && Collection.Count > 0)
            {
                foreach (PSObject psobj in Collection)
                {
                    UniversalStreamRecord record = UniversalStreamRecord.FromPSObject(psobj);
                    collectionUniversalStreamRecord.Add(record);
                }
            }

            return collectionUniversalStreamRecord;
        }


        public static Collection<UniversalStreamRecord> FromVariablePlainHashSet(HashSet<VariablePlain> Collection, bool IncludeReadOnly=true)
        {
            Collection<UniversalStreamRecord> collectionUniversalStreamRecord = new Collection<UniversalStreamRecord>();

            if (Collection != null && Collection.Count > 0)
            {
                foreach (VariablePlain var in Collection)
                {
                    if (var.ReadOnly & IncludeReadOnly==false)
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
