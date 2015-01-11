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
        /// <param name="Report">The Report which should be converted to XML</param>
        /// <param name="TemplateFilepath">Templatefile to be used</param>
        /// <returns>XML string</returns>
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
            sbAssets.AppendLine(CreateRecordDetails("Asset", BaseRec, ResultPrimSecond));
        }

        protected override void EndAssetDetails(StringBuilder sbAssets)
        {
            //We do not need any footer, it's all in the template file
        }

        protected override void ReplaceAssetList(string ValueName, string AssetList)
        {
            ReplaceXMLComment(ValueName, AssetList);
        }

        #endregion



        #region Test details replacement

        protected override void StartTestDetails(StringBuilder sbTests)
        {
            //We do not need any header, it's all in the template file
        }

        protected override void ProcessTest(StringBuilder sbTests, TestRecord Test, BaseRecord BaseRec, ResultPrimarySecondary ResultPrimSecond)
        {
            sbTests.AppendLine(CreateRecordDetails("Test", BaseRec, ResultPrimSecond));
        }

        protected override void EndTestDetails(StringBuilder sbTests)
        {
            //We do not need a footer, it's all in the template file
        }

        protected override void ReplaceTestList(string ValueName, string TestList)
        {
            ReplaceXMLComment(ValueName, TestList);
        }

        #endregion



        private void Replace(string ValueName, string Value)
        {
            _content.Replace("@@" + ValueName + "@@", WeakHTMLTag.HTMLEncode(Value));
        }

        void ReplaceXMLComment(string ValueName, string Value)
        {
            _content.Replace("<!--@@" + ValueName + "@@-->", Value);
        }

        string CreateRecordDetails(string TagName, BaseRecord Record, ResultPrimarySecondary ResultPrimSecond)
        {
            //Yes, this somewhat cheating....
            WeakHTMLTag tag = new WeakHTMLTag(TagName);

            WeakHTMLTag name = new WeakHTMLTag("Name");
            name.Text = Record.Name;

            WeakHTMLTag filename = new WeakHTMLTag("Filename");
            filename.Text = Record.ScriptFilename;

            WeakHTMLTag conclusion = new WeakHTMLTag("Conclusion");
            conclusion.Text = Record.Conclusion.ToString();


            //Create sub tag "result"
            WeakHTMLTag primary = new WeakHTMLTag("Primary");
            primary.Text = ResultPrimSecond.Primary;

            WeakHTMLTag secondary = new WeakHTMLTag("Secondary");
            secondary.Text = ResultPrimSecond.Secondary;

            WeakHTMLTag result = new WeakHTMLTag("Result");
            result.HTML = primary.ToString() + secondary.ToString();


            //Construct the final XML
            tag.HTML = name.ToString() + filename.ToString() + conclusion.ToString() + result.ToString();

            return tag.ToString();
        }
    }
}
