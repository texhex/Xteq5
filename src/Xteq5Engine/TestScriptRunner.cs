using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeadlessPS;
using Yamua;

namespace Xteq5
{
    /// <summary>
    /// Executes scripts and interpret the return as tests
    /// </summary>
    internal class TestScriptRunner : BaseScriptRunner
    {
        List<TestRecord> _results;

        internal async Task<List<TestRecord>> RunAsync(PSScriptRunner scriptRunner, string testScriptsPath, IProgress<RunnerProgressDetail> progress = null)
        {
            _results = new List<TestRecord>();

            await RunInternalAsync(scriptRunner, testScriptsPath, progress);

            return _results;
        }


        protected override void ProcessFailure(BaseRecord record)
        {
            //All basic values were set by BaseRunner already, so we simply add it to our list
            TestRecord rec = new TestRecord(record);
            _results.Add(rec);
        }

        protected override void ProcessEmptyData(BaseRecord record, Hashtable table)
        {
            //Conclusion is already set to .DoesNotApply
            TestRecord rec = new TestRecord(record);
            _results.Add(rec);
        }

        protected override void ProcessNonEmptyData(BaseRecord record, Hashtable table, string dataKeyValue)
        {
            //The basic values are all set by :base() and it was also validated that the hashtable contains a key ".Data". 
            TestRecord testRecord = new TestRecord(record);

            //Data is not NULL and not "", hence we need to check which conclusion the author wanted to report
            ConclusionEnum conclusion = ConclusionEnumConverter.ParseConclusion(dataKeyValue);

            if (conclusion == ConclusionEnum.Fatal)
            {
                //We were not able to parse the value inside .Data
                testRecord.AddLineToProcessMessages(string.Format("Unable to parse result {0}", dataKeyValue));

            }
            testRecord.Conclusion = conclusion;

            _results.Add(testRecord);

        }



    }
}
