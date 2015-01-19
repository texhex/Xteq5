using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace Xteq5
{
    public class BootstrapHTMLGenerator : BaseTemplateReplaceGenerator
    {
        public BootstrapHTMLGenerator()
        {

        }

        public override string Generate(Report report, string templateFilepath)
        {
            ReadTemplate(templateFilepath);

            StartGenerating(report);

            return _content.ToString();
        }

        protected override void ReplaceHeaderValue(string valueName, string value)
        {
            Replace(valueName, value);
        }

        protected override void ReplaceAssetStatisticValue(string valueName, string value)
        {
            Replace(valueName, value);
        }

        protected override void ReplaceTestStatisticValue(string valueName, string value)
        {
            Replace(valueName, value);
        }

        protected override void ReplaceResultText(string valueName, string value)
        {
            Replace(valueName, value);
        }

        protected override void ReplaceAssetConclusionText(string valueName, string value)
        {
            Replace(valueName, value);
        }

        protected override void ReplaceTestConclusionText(string valueName, string value)
        {
            Replace(valueName, value);
        }

        protected override void ReplaceTestRecommendedActionText(string valueName, string value)
        {
            Replace(valueName, value);
        }

        #region Asset details replacement
        protected override void StartAssetDetails(StringBuilder sbAssets)
        {
            //We do not need any header, it's all in the template file
        }

        protected override void ProcessAsset(StringBuilder sbAssets, AssetRecord asset, BaseRecord baseRec, ResultPrimarySecondary resultPrimSecond)
        {

            sbAssets.AppendLine(CreateTableRow(baseRec, resultPrimSecond));
        }

        protected override void EndAssetDetails(StringBuilder sbAssets)
        {
            //We do not need any footer, it's all in the template file
        }

        protected override void ReplaceAssetList(string valueName, string assetList)
        {
            ReplaceHTMLComment(valueName, assetList);
        }

        #endregion



        #region Test details replacement

        protected override void StartTestDetails(StringBuilder sbTests)
        {
            //We do not need any header, it's all in the template file
        }

        protected override void ProcessTest(StringBuilder sbTests, TestRecord test, BaseRecord baseRec, ResultPrimarySecondary resultPrimSecond)
        {
            sbTests.AppendLine(CreateTableRow(baseRec, resultPrimSecond));
        }

        protected override void EndTestDetails(StringBuilder sbTests)
        {
            //We do not need a footer, it's all in the template file
        }

        protected override void ReplaceTestList(string valueName, string testList)
        {
            ReplaceHTMLComment(valueName, testList);
        }

        #endregion



        private void Replace(string valueName, string value)
        {
            _content.Replace("@@" + valueName + "@@", value);
        }

        void ReplaceHTMLComment(string valueName, string value)
        {
            _content.Replace("<!--@@" + valueName + "@@-->", value);
        }



        //Bootstrap helper function
        string CreateTableRow(BaseRecord record, ResultPrimarySecondary primarySecondary)
        {
            WeakHTMLTag tr = new WeakHTMLTag("tr");

            //If the conclusion is MAJOR or FATAL, add a class to the TR so the entire row is colored
            if ((record.Conclusion == ConclusionEnum.Major) || (record.Conclusion == ConclusionEnum.Fatal))
            {
                tr.CSSClass = ConclusionToCSSModifier(record.Conclusion);
            }

            //Add parameter for script details modal
            tr.Attributes["data-toggle"] = "modal";
            tr.Attributes["data-target"] = "#modalDetails";
            tr.Attributes["data-modal-title"] = WeakHTMLTag.HTMLEncode(record.ScriptFilename);
            tr.Attributes["data-modal-content"] = ConvertProcessMessagesToHTML(record.ProcessMessages);

            //Create <td> for Status
            string tdStatus = CreateHTMLElement_TdGlyphSpan(record.Conclusion);

            //Create <td> for Name and Script file
            string tdName = CreateHTMLElement_TdTextSmallText(record.Name, record.ScriptFilename);

            string tdValue = CreateHTMLElement_TdTextSmallText(primarySecondary.Primary, primarySecondary.Secondary);
            tr.HTML = tdStatus + tdName + tdValue;

            return tr.ToString() + "\r\n";
        }


        string ConvertProcessMessagesToHTML(string processMessages)
        {
            string htmlEncoded = WeakHTMLTag.HTMLEncode(processMessages);

            StringBuilder sb = new StringBuilder(htmlEncoded);
            sb.Replace("\r\n", "<br/>");
            sb.Replace("\n", "<br/>");

            return sb.ToString();
        }

        string CreateHTMLElement_TdGlyphSpan(ConclusionEnum conclusion)
        {
            //<td class="success"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></td>
            WeakHTMLTag td = new WeakHTMLTag("td");

            string cssClass = ConclusionToCSSModifier(conclusion);
            if (string.IsNullOrWhiteSpace(cssClass) == false)
            {
                td.Attributes["class"] = cssClass;
            }

            td.HTML = CreateHTMLElement_SpanGlyphicon(conclusion);
            return td.ToString();
        }

        string CreateHTMLElement_TdTextSmallText(string text, string smallText)
        {
            //<td>NAME<br><small><em>SMALLTEXT</em></small></td>      
            string smalltext = CreateHTMLElement_EmSmallText(smallText);

            WeakHTMLTag td = new WeakHTMLTag("td");
            td.Text = text;
            td.HTML += "<br/>";
            td.HTML += smalltext;

            return td.ToString();
        }

        string CreateHTMLElement_EmSmallText(string text)
        {
            WeakHTMLTag em = new WeakHTMLTag("em");
            em.Text = text;

            WeakHTMLTag small = new WeakHTMLTag("small", em);

            return small.ToString();
        }

        string CreateHTMLElement_SpanGlyphicon(ConclusionEnum conclusion)
        {
            //<span class="glyphicon glyphicon-ok" aria-hidden="true">
            string glyphicon = ConclusionToGlyphicon(conclusion);

            WeakHTMLTag span = new WeakHTMLTag("span");
            span.CSSClass = "glyphicon " + glyphicon;
            span.Attributes["aria-hidden"] = "true";
            return span.ToString();
        }

        string ConclusionToGlyphicon(ConclusionEnum conclusion)
        {
            switch (conclusion)
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


        string ConclusionToCSSModifier(ConclusionEnum conclusion)
        {
            switch (conclusion)
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
