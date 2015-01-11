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
        /// <param name="Report">The report that should be transfered into JSON</param>
        /// <returns>A JSON formated string</returns>
        public override string Generate(Xteq5.Report Report)
        {
            ReportClass rep = new ReportClass();
            rep.IssuesFound = new IssuesFoundClass();

            rep.ID = Report.ID;
            rep.Compilation = Report.CompilationFolder;
            rep.EngineVersion = Report.EngineVersion;
            rep.Username = Report.UserName;
            rep.Computername = Report.ComputerName;
            rep.Text = Report.UserText;
            rep.StartDateTimeUTC = Report.StartedUTC;
            rep.EndDateTimeUTC = Report.EndedUTC;
            rep.RuntimeSeconds = Report.RuntimeSeconds;

            rep.IssuesFound.Any = Report.IssuesFound;
            rep.IssuesFound.Assets = Report.AssetIssuesFound;
            rep.IssuesFound.Tests = Report.TestIssuesFound;

            rep.AssetStatistics = Report.AssetStatiscs;
            rep.TestStatistics = Report.TestStatiscs;

            rep.Assets = new List<ItemDetail>();
            rep.Tests = new List<ItemDetail>();

            //Loop over all assets
            foreach (AssetRecord asset in Report.Assets)
            {
                BaseRecord baseRec = asset as BaseRecord;
                rep.Assets.Add(ConvertToItemDetail(baseRec));
            }

            //Loop over all tests
            foreach (TestRecord test in Report.Tests)
            {
                BaseRecord baseRec = test as BaseRecord;
                rep.Tests.Add(ConvertToItemDetail(baseRec));
            }

            //MTH: I can't even imaging how much magic happens in this single call. 
            return JsonConvert.SerializeObject(rep);
        }

        private ItemDetail ConvertToItemDetail(BaseRecord BaseRec)
        {
            ItemDetail detail = new ItemDetail();
            detail.Name = BaseRec.Name;
            detail.Filename = BaseRec.ScriptFilename;
            detail.Conclusion = BaseRec.Conclusion;
            detail.ConclusionString = BaseRec.Conclusion.ToString();

            ResultPrimarySecondary rps = new ResultPrimarySecondary(BaseRec);

            ItemResult result = new ItemResult();
            result.Primary = rps.Primary;
            result.Seconday = rps.Secondary;

            detail.Result = result;

            return detail;
        }
    }
}
