using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xteq5
{
    //The result json can be validated using http://jsonlint.com/

    //I'm still unsure if the result JSON should be enveloped or not. 
    //It seems we shouldn't: http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api, section "Don't use an envelope by default, but make it possible when needed"

    public class JSONGenerator : BaseGenerator
    {
        //Helper classes to generate the JSON
        private class ReportClass
        {
            public Guid ID;
            public string Compilation;
            public Version EngineVersion;
            public string EngineVersionString;
            public string Username;
            public string Computername;
            public string Text;
            public DateTime StartDateTimeUTC;
            public DateTime EndDateTimeUTC;
            public string RuntimeSeconds;

            public IssuesFoundClass IssuesFound;

            public RecordsStatistics AssetStatistics;
            public RecordsStatistics TestStatistics;

            public List<ItemDetail> Assets;
            public List<ItemDetail> Tests;
        }

        private class IssuesFoundClass
        {
            public bool Any;
            public bool Assets;
            public bool Tests;
        }

        private class ItemDetail
        {
            public string Name;
            public string Filename;
            public ConclusionEnum Conclusion;
            public string ConclusionString;
            public ItemResult Result;
        }

        private class ItemResult
        {
            public string Primary;
            public string Seconday;
        }


        /// <summary>
        /// Generates a JSON formated output report
        /// </summary>
        /// <param name="report">The report that should be transfered into JSON</param>
        /// <returns>A JSON formated string</returns>
        public override string Generate(Report report)
        {
            ReportClass rep = new ReportClass();
            rep.IssuesFound = new IssuesFoundClass();

            rep.ID = report.ID;
            rep.Compilation = report.CompilationFolder;
            rep.EngineVersion = report.EngineVersion;
            rep.EngineVersionString = report.EngineVersion.ToString();
            rep.Username = report.UserName;
            rep.Computername = report.ComputerName;
            rep.Text = report.UserText;
            rep.StartDateTimeUTC = report.StartedUTC;
            rep.EndDateTimeUTC = report.EndedUTC;
            rep.RuntimeSeconds = report.RuntimeSeconds;

            rep.IssuesFound.Any = report.IssuesFound;
            rep.IssuesFound.Assets = report.AssetIssuesFound;
            rep.IssuesFound.Tests = report.TestIssuesFound;

            rep.AssetStatistics = report.AssetStatiscs;
            rep.TestStatistics = report.TestStatiscs;

            rep.Assets = new List<ItemDetail>();
            rep.Tests = new List<ItemDetail>();

            //Loop over all assets
            foreach (AssetRecord asset in report.Assets)
            {
                BaseRecord baseRec = asset as BaseRecord;
                rep.Assets.Add(ConvertToItemDetail(baseRec));
            }

            //Loop over all tests
            foreach (TestRecord test in report.Tests)
            {
                BaseRecord baseRec = test as BaseRecord;
                rep.Tests.Add(ConvertToItemDetail(baseRec));
            }

            //MTH: I can't even imaging how much magic happens in this single call. 
            return JsonConvert.SerializeObject(rep);
        }

        private ItemDetail ConvertToItemDetail(BaseRecord baseRec)
        {
            ItemDetail detail = new ItemDetail();
            detail.Name = baseRec.Name;
            detail.Filename = baseRec.ScriptFilename;
            detail.Conclusion = baseRec.Conclusion;
            detail.ConclusionString = baseRec.Conclusion.ToString();

            ResultPrimarySecondary rps = new ResultPrimarySecondary(baseRec);

            ItemResult result = new ItemResult();
            result.Primary = rps.Primary;
            result.Seconday = rps.Secondary;

            detail.Result = result;

            return detail;
        }
    }
}
