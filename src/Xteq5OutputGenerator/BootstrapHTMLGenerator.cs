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

        public override string Generate(Report Report, string TemplateFilepath)
        {
            ReadTemplate(TemplateFilepath);

            StartGenerating(Report);

            return _content.ToString();
        }

        protected override void ReplaceHeaderValue(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        protected override void ReplaceAssetStatisticValue(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        protected override void ReplaceTestStatisticValue(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        protected override void ReplaceResultText(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        protected override void ReplaceAssetConclusionText(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        protected override void ReplaceTestConclusionText(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        protected override void ReplaceTestRecommendedActionText(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        #region Asset details replacement
        protected override void StartAssetDetails(StringBuilder sbAssets)
        {
            //We do not need any header, it's all in the template file
        }

        protected override void ProcessAsset(StringBuilder sbAssets, AssetRecord Asset, BaseRecord BaseRec, ResultPrimarySecondary ResultPrimSecond)
        {

            sbAssets.AppendLine(CreateTableRow(BaseRec, ResultPrimSecond));
        }

        protected override void EndAssetDetails(StringBuilder sbAssets)
        {
            //We do not need any footer, it's all in the template file
        }

        protected override void ReplaceAssetList(string ValueName, string AssetList)
        {
            ReplaceHTMLComment(ValueName, AssetList);
        }

        #endregion



        #region Test details replacement

        protected override void StartTestDetails(StringBuilder sbTests)
        {
            //We do not need any header, it's all in the template file
        }

        protected override void ProcessTest(StringBuilder sbTests, TestRecord Test, BaseRecord BaseRec, ResultPrimarySecondary ResultPrimSecond)
        {
            sbTests.AppendLine(CreateTableRow(BaseRec, ResultPrimSecond));
        }

        protected override void EndTestDetails(StringBuilder sbTests)
        {
            //We do not need a footer, it's all in the template file
        }

        protected override void ReplaceTestList(string ValueName, string TestList)
        {
            ReplaceHTMLComment(ValueName, TestList);
        }

        #endregion



        private void Replace(string ValueName, string Value)
        {
            _content.Replace("@@" + ValueName + "@@", Value);
        }

        void ReplaceHTMLComment(string ValueName, string Value)
        {
            _content.Replace("<!--@@" + ValueName + "@@-->", Value);
        }



        //Bootstrap helper function
        string CreateTableRow(BaseRecord Record, ResultPrimarySecondary PrimarySecondary)
        {
            WeakHTMLTag tr = new WeakHTMLTag("tr");

            //If the conclusion is MAJOR or FATAL, add a class to the TR so the entire row is colored
            if ((Record.Conclusion == ConclusionEnum.Major) || (Record.Conclusion == ConclusionEnum.Fatal))
            {
                tr.CSSClass = ConclusionToCSSModifier(Record.Conclusion);
            }

            //Add parameter for script details modal
            tr.Attributes["data-toggle"] = "modal";
            tr.Attributes["data-target"] = "#modalDetails";
            tr.Attributes["data-modal-title"] = WeakHTMLTag.HTMLEncode(Record.ScriptFilename);
            tr.Attributes["data-modal-content"] = ConvertProcessMessagesToHTML(Record.ProcessMessages);

            //Create <td> for Status
            string tdStatus = CreateHTMLElement_TdGlyphSpan(Record.Conclusion);

            //Create <td> for Name and Script file
            string tdName = CreateHTMLElement_TdTextSmallText(Record.Name, Record.ScriptFilename);

            string tdValue = CreateHTMLElement_TdTextSmallText(PrimarySecondary.Primary, PrimarySecondary.Secondary);
            tr.HTML = tdStatus + tdName + tdValue;

            return tr.ToString() + "\r\n";
        }


        string ConvertProcessMessagesToHTML(string ProcessMessages)
        {
            string htmlEncoded = WeakHTMLTag.HTMLEncode(ProcessMessages);

            StringBuilder sb = new StringBuilder(htmlEncoded);
            sb.Replace("\r\n", "<br/>");
            sb.Replace("\n", "<br/>");

            return sb.ToString();
        }

        string CreateHTMLElement_TdGlyphSpan(ConclusionEnum Conclusion)
        {
            //<td class="success"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></td>
            WeakHTMLTag td = new WeakHTMLTag("td");

            string cssClass = ConclusionToCSSModifier(Conclusion);
            if (string.IsNullOrWhiteSpace(cssClass) == false)
            {
                td.Attributes["class"] = cssClass;
            }

            td.HTML = CreateHTMLElement_SpanGlyphicon(Conclusion);
            return td.ToString();
        }

        string CreateHTMLElement_TdTextSmallText(string Text, string SmallText)
        {
            //<td>NAME<br><small><em>SMALLTEXT</em></small></td>      
            string smalltext = CreateHTMLElement_EmSmallText(SmallText);

            WeakHTMLTag td = new WeakHTMLTag("td");
            td.Text = Text;
            td.HTML += "<br/>";
            td.HTML += smalltext;

            return td.ToString();
        }

        string CreateHTMLElement_EmSmallText(string Text)
        {
            WeakHTMLTag em = new WeakHTMLTag("em");
            em.Text = Text;

            WeakHTMLTag small = new WeakHTMLTag("small", em);

            return small.ToString();
        }

        string CreateHTMLElement_SpanGlyphicon(ConclusionEnum Conclusion)
        {
            //<span class="glyphicon glyphicon-ok" aria-hidden="true">
            string glyphicon = ConclusionToGlyphicon(Conclusion);

            WeakHTMLTag span = new WeakHTMLTag("span");
            span.CSSClass = "glyphicon " + glyphicon;
            span.Attributes["aria-hidden"] = "true";
            return span.ToString();
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
