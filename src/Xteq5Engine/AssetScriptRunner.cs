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

        internal async Task<List<AssetRecord>> Run(PSScriptRunner scriptRunner, string assetScriptPath, IProgress<RunnerProgressDetail> progress = null)
        {
            _results = new List<AssetRecord>();

            await RunInternalAsync(scriptRunner, assetScriptPath, progress);

            return _results;
        }

        protected override void ProcessFailure(BaseRecord record)
        {
            //All basic values were set by BaseRunner already, so we simply add it to our list
            AssetRecord rec = new AssetRecord(record);
            _results.Add(rec);
        }

        protected override void ProcessEmptyData(BaseRecord record, Hashtable table)
        {
            //All basic values were set by BaseRunner already, so we simply add it to our list
            AssetRecord rec = new AssetRecord(record);
            _results.Add(rec);
        }


        protected override void ProcessNonEmptyData(BaseRecord record, Hashtable table, string dataKeyValue)
        {
            AssetRecord assetRecord = new AssetRecord(record);

            //Data is set = Conclusion.Success
            assetRecord.Conclusion = ConclusionEnum.Success;
            assetRecord.Data = dataKeyValue;

            //Check the object that was returned and copy it to .DataNative (if we support the type)
            object dataObjectFromHashtable = GetObjectFromHashtable(table, Xteq5EngineConstant.ReturnedHashtableKeyData);

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
