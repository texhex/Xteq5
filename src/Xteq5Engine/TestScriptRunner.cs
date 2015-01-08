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

        internal async Task<List<TestRecord>> RunAsync(PSScriptRunner ScriptRunner, string TestScriptPath)
        {
            _results = new List<TestRecord>();

            await RunInternalAsync(ScriptRunner, TestScriptPath);

            return _results;
        }


        protected override void ProcessFailure(BaseRecord Record)
        {
            //All basic values were set by BaseRunner already, so we simply add it to our list
            TestRecord rec = new TestRecord(Record);
            _results.Add(rec);
        }

        protected override void ProcessEmptyData(BaseRecord Record, Hashtable Table)
        {
            //Conclusion is already set to .DoesNotApply
            TestRecord rec = new TestRecord(Record);
            _results.Add(rec);
        }

        protected override void ProcessNonEmptyData(BaseRecord Record, Hashtable Table, string DataKeyValue)
        {
            //The basic values are all set by :base() and it was also validated that the hashtable contains a key ".Data". 
            TestRecord testRecord = new TestRecord(Record);

            //Data is not NULL and not "", hence we need to check which conclusion the author wanted to report
            ConclusionEnum conclusion = ConclusionEnumConverter.ParseConclusion(DataKeyValue);

            if (conclusion == ConclusionEnum.Fatal)
            {
                //We were not able to parse the value inside .Data
                testRecord.AddLineToProcessMessages(string.Format("Unable to parse result {0}", DataKeyValue));

            }
            testRecord.Conclusion = conclusion;

            _results.Add(testRecord);

        }



    }
}
