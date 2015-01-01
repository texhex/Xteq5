using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace TestUtil
{
    //TODO: Maybe switch to HtmlBuilders? https://github.com/amoerie/htmlbuilders
    //TODO: Create a base class to generate other format, e.g. Markdown, XML, Text etc.?
    public class BootstrapHTMLGenerator
    {
        private BootstrapHTMLGenerator()
        {

        }

        StringBuilder _templateHTML = new StringBuilder();
        HtmlDocument _doc = new HtmlDocument();

        public BootstrapHTMLGenerator(string TemplateHTMLFile)
        {
            //If the file is not found, this will raise an exception
            _templateHTML = new StringBuilder(File.ReadAllText(TemplateHTMLFile, Encoding.UTF8));
        }

        /// <summary>
        /// Generates a Bootstrap HTML report from the template 
        /// </summary>
        /// <param name="Report">Finished report to create report for</param>
        /// <returns>HTML source code</returns>
        public string Generate(Report Report)
        {
            //Replace all known string values
            Replace("ReportID", Report.ID.ToString());
            Replace("UserName", Report.UserName);
            Replace("Computername", Report.ComputerName);
            Replace("UserText", Report.UserText);
            Replace("SourceFolder", Report.SourceFolder);
            Replace("VersionString", Report.TestUtilVersion.ToString());            

            //Datetime in UTC and ISO 8601 format without fraction of second
            Replace("StartDateTimeUTC", Report.StartedUTC.ToString("s") + "Z");
            Replace("EndDateTimeUTC", Report.EndedUTC.ToString("s") + "Z");
            Replace("RuntimeSeconds", Report.RuntimeSeconds);

            //Add statistics for assets
            Replace("Assets", Report.AssetStatiscs);
            Replace("Tests", Report.TestStatiscs);

            //Replace Result texts (e.g. Success = "OK", Fatal = "Crashed" etc.)
            Replace("ResultText.DoesNotApply", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.DoesNotApply));
            Replace("ResultText.Success", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Success));
            Replace("ResultText.Fatal", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Fatal));
            Replace("ResultText.Inconclusive", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Inconclusive));
            Replace("ResultText.Major", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Major));
            Replace("ResultText.Minor", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Minor));

            //Replace text values for assets
            Replace("AssetText.DoesNotApply", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.DoesNotApply));
            Replace("AssetText.Success", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.Success));
            Replace("AssetText.Fatal", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.Fatal));

            //Replace text values for tests
            Replace("TestText.DoesNotApply", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.DoesNotApply));
            Replace("TestText.Success", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Success));
            Replace("TestText.Fatal", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Fatal));
            Replace("TestText.Inconclusive", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Inconclusive));
            Replace("TestText.Major", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Major));
            Replace("TestText.Minor", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Minor));

            //Recplace recommended action for tests
            Replace("TestActionText.DoesNotApply", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.DoesNotApply));
            Replace("TestActionText.Success", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Success));
            Replace("TestActionText.Fatal", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Fatal));
            Replace("TestActionText.Inconclusive", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Inconclusive));
            Replace("TestActionText.Major", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Major));
            Replace("TestActionText.Minor", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Minor));


            //Add entries to tables
            ReplaceHTMLComment("AssetRows", Report.Assets);
            ReplaceHTMLComment("TestRows", Report.Tests);

            string result=_templateHTML.ToString();
            return result;
        }

        /// <summary>
        /// Generates an HTML file and save it as a temporary file 
        /// </summary>
        /// <param name="report">Report that should be converted to HTML</param>
        /// <returns>Filename of the file generated</returns>
        public string GenerateAndSaveFile(Report report)
        {
            string html = Generate(report);

            string tempPath = Path.GetTempPath();

            string datepart = string.Format("{0:yyyy-MM-dd_HHmm}", report.EndedUTC); //2014-12-02_1458

            string guidpart = report.ID.ToString("N"); //00000000000000000000000000000000

            string fileName = "TestUtil_Report_" + datepart + "_" + guidpart + ".html"; 

            string fullFilename = Path.Combine(tempPath, fileName);

            File.WriteAllText(fullFilename, html);

            return fullFilename;
        }



        void ReplaceInternal(string SearchFor, string ReplaceWith)
        {
            _templateHTML.Replace(SearchFor.ToUpper(), ReplaceWith);
        }

        void Replace(string SearchFor, string ReplaceWith)
        {
            ReplaceInternal("@@" + SearchFor + "@@", ReplaceWith);
        }

        void ReplaceHTMLComment(string SearchFor, string ReplaceWith)
        {
            ReplaceInternal("<!--@@" + SearchFor.ToUpper() + "@@-->", ReplaceWith);
        }


        void Replace(string SearchFor, int ReplaceWith)
        {
            Replace(SearchFor, ReplaceWith.ToString());
        }

        void Replace(string BaseName, RecordsStatistics statistics)
        {
            Replace(BaseName + ".DoesNotApply", statistics.DoesNotApplyCount);
            Replace(BaseName + ".Fatal", statistics.FatalCount);
            Replace(BaseName + ".Inconclusive", statistics.InconclusiveCount);
            Replace(BaseName + ".Major", statistics.MajorCount);
            Replace(BaseName + ".Minor", statistics.MinorCount);
            Replace(BaseName + ".Success", statistics.SuccessCount);
            Replace(BaseName + ".Total", statistics.Total);
        }

        void ReplaceHTMLComment(string SearchFor, List<AssetRecord> Assets)
        {
            StringBuilder sb = new StringBuilder();
            foreach (AssetRecord asset in Assets)
            {
                BaseRecord baseRec = asset as BaseRecord;

                //If we have a value, use this a primary data
                if (asset.Conclusion == ConclusionEnum.Success)
                {
                    sb.AppendLine(CreateTableRow(baseRec, asset.Data));
                }
                else
                {
                    sb.AppendLine(CreateTableRow(baseRec, ""));
                }
            }
            ReplaceHTMLComment(SearchFor, sb.ToString());
        }


        void ReplaceHTMLComment(string SearchFor, List<TestRecord> Tests)
        {
            StringBuilder sb = new StringBuilder();
            foreach (TestRecord test in Tests)
            {
                BaseRecord baseRec = test as BaseRecord;
                sb.AppendLine(CreateTableRow(baseRec, ""));
            }
            ReplaceHTMLComment(SearchFor, sb.ToString());
        }


        string CreateTableRow(BaseRecord Record, string PrimaryData)
        {
            HtmlNode tr = _doc.CreateElement("tr");

            //If the conclusion is MAJOR or FATAL, add a class to the TR so the entire row is colored
            if ((Record.Conclusion == ConclusionEnum.Major) || (Record.Conclusion == ConclusionEnum.Fatal))
            {
                tr.Attributes.Add("class", ConclusionToCSSModifier(Record.Conclusion));
            }

            //Add parameter for script details modal
            tr.Attributes.Add("data-toggle", "modal");
            tr.Attributes.Add("data-target", "#modalDetails");
            tr.Attributes.Add("data-modal-title", HtmlDocument.HtmlEncode(Record.ScriptFilename));
            tr.Attributes.Add("data-modal-content", ConvertProcessMessagesToHTML(Record.ProcessMessages));

            //Create <td> for Status
            string tdStatus = CreateTd_GlyphSpan(Record.Conclusion);

            //Create <td> for Name and Script file
            string tdName = CreateTd_Text_SmallText(Record.Name, Record.ScriptFilename);

            //Prepare the data for primary and secondary data
            ResultPrimarySecondary rps = new ResultPrimarySecondary(Record);

            //If the caller gave us PrimaryData, we will use that in any case.
            if (string.IsNullOrWhiteSpace(PrimaryData) == false)
            {
                rps.Primary = PrimaryData;
            }

            string tdValue = CreateTd_Text_SmallText(rps.Primary, rps.Secondary);


            tr.InnerHtml = tdStatus + tdName + tdValue;
            return tr.OuterHtml + "\r\n";
        }

        string ConvertProcessMessagesToHTML(string ProcessMessages)
        {
            string htmlEncoded = HtmlDocument.HtmlEncode(ProcessMessages);

            StringBuilder sb = new StringBuilder(htmlEncoded);
            sb.Replace("\r\n", "<br/>");
            sb.Replace("\n", "<br/>");

            //Do do not add <samp> tags anymore because it it easier to set this using jQuery
            //return "<samp>" + sb.ToString() + "</samp>";
            return sb.ToString();
        }

        string CreateTd_GlyphSpan(ConclusionEnum Conclusion)
        {
            //<td class="success"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></td>
            HtmlNode td = _doc.CreateElement("td");

            string cssClass = ConclusionToCSSModifier(Conclusion);
            if (string.IsNullOrWhiteSpace(cssClass) == false)
            {
                td.Attributes.Add("class", cssClass);
            }

            td.InnerHtml = CreateSpan_Glyphicon(Conclusion);
            return td.OuterHtml;
        }

        /*
        string CreateTd_GlyphSpan_SmallText(ConclusionEnum Conclusion, string SmallText)
        {
            string smalltext = CreateEm_SmallText(SmallText);

            string span = CreateSpan_Glyphicon(Conclusion);

            HtmlNode td = _doc.CreateElement("td");
            td.InnerHtml = span + "<br/>" + smalltext;

            return td.OuterHtml;
        }
        */
        string CreateTd_Text_SmallText(string Text, string SmallText)
        {
            //<td>NAME<br><small><em>SMALLTEXT</em></small></td>      
            string smalltext = CreateEm_SmallText(SmallText);

            HtmlNode td = _doc.CreateElement("td");
            td.InnerHtml = HtmlDocument.HtmlEncode(Text) + "<br/>" + smalltext;

            return td.OuterHtml;
        }

        string CreateEm_SmallText(string Text)
        {
            HtmlNode em = _doc.CreateElement("em");
            em.InnerHtml = HtmlDocument.HtmlEncode(Text);

            HtmlNode small = _doc.CreateElement("small");
            small.InnerHtml = em.OuterHtml;

            return small.OuterHtml;
        }

        string CreateSpan_Glyphicon(ConclusionEnum Conclusion)
        {
            //<span class="glyphicon glyphicon-ok" aria-hidden="true">
            string glyphicon = ConclusionToGlyphicon(Conclusion);

            HtmlNode span = _doc.CreateElement("span");
            span.Attributes.Add("class", "glyphicon " + glyphicon);
            span.Attributes.Add("aria-hidden", "true");

            return span.OuterHtml;
        }

        string ConclusionToGlyphicon(ConclusionEnum Conclusion)
        {
            switch (Conclusion)
            {
                case ConclusionEnum.DoesNotApply:
                    return "glyphicon-asterisk";

                case ConclusionEnum.Fatal:
                    return "glyphicon-warning-sign";

                case ConclusionEnum.Inconclusive:
                    return "glyphicon-ban-circle";

                case ConclusionEnum.Major:
                    return "glyphicon-remove";

                case ConclusionEnum.Minor:
                    return "glyphicon-info-sign";

                case ConclusionEnum.Success:
                    return "glyphicon-ok";

                default:
                    return "glyphicon-exclamation-sign";
            }
        }


        string ConclusionToCSSModifier(ConclusionEnum Conclusion)
        {
            switch (Conclusion)
            {
                case ConclusionEnum.DoesNotApply:
                case ConclusionEnum.Inconclusive:
                    return "";

                case ConclusionEnum.Success:
                    return "success";

                case ConclusionEnum.Fatal:
                    return "danger";

                case ConclusionEnum.Major:
                    return "warning";

                case ConclusionEnum.Minor:
                    return "info";

                default:
                    return "ConclusionToCssModifierFailed";
            }
        }

    }
}
