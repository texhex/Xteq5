using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace Xteq5
{
    public class XMLGenerator : BaseTemplateReplaceGenerator
    {
      
        /// <summary>
        /// Generates XML based on the given Xteq5 Report object
        /// </summary>
        /// <param name="report">The Report which should be converted to XML</param>
        /// <param name="templateFilepath">Templatefile to be used</param>
        /// <returns>XML string</returns>
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
            sbAssets.AppendLine(CreateRecordDetails("Asset", baseRec, resultPrimSecond));
        }

        protected override void EndAssetDetails(StringBuilder sbAssets)
        {
            //We do not need any footer, it's all in the template file
        }

        protected override void ReplaceAssetList(string valueName, string assetList)
        {
            ReplaceXMLComment(valueName, assetList);
        }

        #endregion



        #region Test details replacement

        protected override void StartTestDetails(StringBuilder sbTests)
        {
            //We do not need any header, it's all in the template file
        }

        protected override void ProcessTest(StringBuilder sbTests, TestRecord test, BaseRecord baseRec, ResultPrimarySecondary resultPrimSecond)
        {
            sbTests.AppendLine(CreateRecordDetails("Test", baseRec, resultPrimSecond));
        }

        protected override void EndTestDetails(StringBuilder sbTests)
        {
            //We do not need a footer, it's all in the template file
        }

        protected override void ReplaceTestList(string valueName, string testList)
        {
            ReplaceXMLComment(valueName, testList);
        }

        #endregion



        private void Replace(string valueName, string value)
        {
            _content.Replace("@@" + valueName + "@@", WeakHTMLTag.HTMLEncode(value));
        }

        void ReplaceXMLComment(string valueName, string value)
        {
            _content.Replace("<!--@@" + valueName + "@@-->", value);
        }

        string CreateRecordDetails(string tagName, BaseRecord record, ResultPrimarySecondary resultPrimSecond)
        {
            //Yes, this somewhat cheating....
            WeakHTMLTag tag = new WeakHTMLTag(tagName);

            WeakHTMLTag name = new WeakHTMLTag("Name");
            name.Text = record.Name;

            WeakHTMLTag filename = new WeakHTMLTag("Filename");
            filename.Text = record.ScriptFilename;

            WeakHTMLTag conclusion = new WeakHTMLTag("Conclusion");
            conclusion.Text = record.Conclusion.ToString();


            //Create sub tag "result"
            WeakHTMLTag primary = new WeakHTMLTag("Primary");
            primary.Text = resultPrimSecond.Primary;

            WeakHTMLTag secondary = new WeakHTMLTag("Secondary");
            secondary.Text = resultPrimSecond.Secondary;

            WeakHTMLTag result = new WeakHTMLTag("Result");
            result.HTML = primary.ToString() + secondary.ToString();


            //Construct the final XML
            tag.HTML = name.ToString() + filename.ToString() + conclusion.ToString() + result.ToString();

            return tag.ToString();
        }
    }
}
