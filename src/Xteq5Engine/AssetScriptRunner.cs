using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessPS;

namespace Xteq5
{
    /// <summary>
    /// Executes scripts and interpret the return as assets
    /// </summary>
    internal class AssetScriptRunner : BaseScriptRunner
    {
        List<AssetRecord> _results;

        internal async Task<List<AssetRecord>> Run(PSScriptRunner ScriptRunner, string AssetScriptPath, IProgress<RunnerProgress> Progress = null)
        {
            _results = new List<AssetRecord>();

            await RunInternalAsync(ScriptRunner, AssetScriptPath, Progress);

            return _results;
        }

        protected override void ProcessFailure(BaseRecord Record)
        {
            //All basic values were set by BaseRunner already, so we simply add it to our list
            AssetRecord rec = new AssetRecord(Record);
            _results.Add(rec);
        }

        protected override void ProcessEmptyData(BaseRecord Record, Hashtable Table)
        {
            //All basic values were set by BaseRunner already, so we simply add it to our list
            AssetRecord rec = new AssetRecord(Record);
            _results.Add(rec);
        }


        protected override void ProcessNonEmptyData(BaseRecord Record, Hashtable Table, string DataKeyValue)
        {
            AssetRecord assetRecord = new AssetRecord(Record);

            //Data is set = Conclusion.Success
            assetRecord.Conclusion = ConclusionEnum.Success;
            assetRecord.Data = DataKeyValue;

            //Check the object that was returned and copy it to .DataNative (if we support the type)
            object dataObjectFromHashtable = GetObjectFromHashtable(Table, Xteq5EngineConstant.ReturnedHashtableKeyData);

            if (dataObjectFromHashtable is string)
            {
                //String is the default of .Data. Therefore no action is required
            }
            else
            {
                if (dataObjectFromHashtable is bool)
                {
                    assetRecord.DataNative = (Boolean)dataObjectFromHashtable;
                }
                else
                {
                    if (dataObjectFromHashtable is int)
                    {
                        assetRecord.DataNative = (int)dataObjectFromHashtable;
                    }

                    else
                    {
                        if (dataObjectFromHashtable is System.Version)
                        {
                            assetRecord.DataNative = dataObjectFromHashtable as System.Version;
                        }
                    }
                }
            }


            _results.Add(assetRecord);
        }





    }
}
