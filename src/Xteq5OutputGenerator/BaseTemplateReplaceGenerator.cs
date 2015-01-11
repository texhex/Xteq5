using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace Xteq5
{
    /// <summary>
    /// A base class for class that reads a template file and then replaces the content with the actual values.
    /// </summary>
    public abstract class BaseTemplateReplaceGenerator
    {
        protected StringBuilder _content = new StringBuilder();

        protected void ReadTemplate(string TemplateFilepath)
        {
            if (string.IsNullOrWhiteSpace(TemplateFilepath))
                throw new ArgumentException("TemplateFilepath can not be empty");

            if (PathExtension.FileExists(TemplateFilepath) == false)
            {
                throw new TemplateFileNotFoundException(TemplateFilepath);
            }

            //If there is a load error, this will throw an exception
            _content = new StringBuilder(File.ReadAllText(TemplateFilepath, Encoding.UTF8));
        }

        //Base function that is called by a consumer of an implementation of this class
        public abstract string Generate(Report Report, string TemplateFilepath);


        //This needs to be called by the implementation to start the replacement process
        protected void StartGenerating(Report Report)
        {
            ReplaceHeaderValuesInternal(Report);

            ReplaceAssetStatisticsInternal(Report);
            ReplaceTestStatisticsInternal(Report);

            ReplaceResultTextInternal();
            ReplaceAssetConclusionTextInternal();
            ReplaceTestConclusionTextInternal();

            ReplaceTestRecommendedActionTextInternal();

            //Begin details replacement for assets
            StringBuilder sbAssets = new StringBuilder();

            StartAssetDetails(sbAssets);
            foreach (AssetRecord asset in Report.Assets)
            {
                BaseRecord baseRec = asset as BaseRecord;
                ResultPrimarySecondary rps = new ResultPrimarySecondary(baseRec);
                
                ProcessAsset(sbAssets, asset, baseRec, rps);
            }
            EndAssetDetails(sbAssets);

            ReplaceAssetList("AssetRows".ToUpper(), sbAssets.ToString());


            //Begin details replacment for tests
            StringBuilder sbTests = new StringBuilder();

            StartTestDetails(sbTests);
            foreach (TestRecord test in Report.Tests)
            {
                BaseRecord baseRec = test as BaseRecord;
                ResultPrimarySecondary rps = new ResultPrimarySecondary(baseRec);

                ProcessTest(sbTests, test, baseRec, rps);
            }
            EndTestDetails(sbAssets);

            ReplaceTestList("TestRows".ToUpper(), sbTests.ToString());


        }


        private void ReplaceHeaderValuesInternal(Report Report)
        {
            //Replace all known string values
            ReplaceHeaderValueInternal("ReportID", Report.ID.ToString());
            ReplaceHeaderValueInternal("UserName", Report.UserName);
            ReplaceHeaderValueInternal("Computername", Report.ComputerName);
            ReplaceHeaderValueInternal("UserText", Report.UserText);
            ReplaceHeaderValueInternal("SourceFolder", Report.CompilationFolder);
            ReplaceHeaderValueInternal("VersionString", Report.EngineVersion.ToString());

            //Currently not used by any report
            ReplaceHeaderValueInternal("AssetIssuesFound", Report.AssetIssuesFound.ToString());
            ReplaceHeaderValueInternal("TestIssuesFound", Report.TestIssuesFound.ToString());
            ReplaceHeaderValueInternal("IssuesFound", Report.IssuesFound.ToString());

            //Datetime in UTC and ISO 8601 format without fraction of second
            ReplaceHeaderValueInternal("StartDateTimeUTC", Report.StartedUTC.ToString("s") + "Z");
            ReplaceHeaderValueInternal("EndDateTimeUTC", Report.EndedUTC.ToString("s") + "Z");
            ReplaceHeaderValueInternal("RuntimeSeconds", Report.RuntimeSeconds);
        }

        private void ReplaceHeaderValueInternal(string ValueName, string Value)
        {
            ReplaceHeaderValue(ValueName.ToUpper(), Value);
        }

        //Called from this class to replace typical "Header" items: Data that is only found once in the report. 
        protected abstract void ReplaceHeaderValue(string ValueName, string Value);


        private void ReplaceAssetStatisticsInternal(Report Report)
        {
            ReplaceAssetStaticValueInternal("Assets.DoesNotApply", Report.AssetStatiscs.DoesNotApplyCount);
            ReplaceAssetStaticValueInternal("Assets.Fatal", Report.AssetStatiscs.FatalCount);
            ReplaceAssetStaticValueInternal("Assets.Inconclusive", Report.AssetStatiscs.InconclusiveCount);
            ReplaceAssetStaticValueInternal("Assets.Major", Report.AssetStatiscs.MajorCount);
            ReplaceAssetStaticValueInternal("Assets.Minor", Report.AssetStatiscs.MinorCount);
            ReplaceAssetStaticValueInternal("Assets.Success", Report.AssetStatiscs.SuccessCount);
            ReplaceAssetStaticValueInternal("Assets.Total", Report.AssetStatiscs.Total);
        }

        private void ReplaceAssetStaticValueInternal(string ValueName, int Value)
        {
            ReplaceAssetStatisticValue(ValueName.ToUpper(), Value.ToString());
        }

        //Called from this class to replace statistical counter for all assets found
        protected abstract void ReplaceAssetStatisticValue(string ValueName, string Value);


        private void ReplaceTestStatisticsInternal(Report Report)
        {
            ReplaceTestStatisticValueInternal("Tests.DoesNotApply", Report.TestStatiscs.DoesNotApplyCount);
            ReplaceTestStatisticValueInternal("Tests.Fatal", Report.TestStatiscs.FatalCount);
            ReplaceTestStatisticValueInternal("Tests.Inconclusive", Report.TestStatiscs.InconclusiveCount);
            ReplaceTestStatisticValueInternal("Tests.Major", Report.TestStatiscs.MajorCount);
            ReplaceTestStatisticValueInternal("Tests.Minor", Report.TestStatiscs.MinorCount);
            ReplaceTestStatisticValueInternal("Tests.Success", Report.TestStatiscs.SuccessCount);
            ReplaceTestStatisticValueInternal("Tests.Total", Report.TestStatiscs.Total);
        }

        private void ReplaceTestStatisticValueInternal(string ValueName, int Value)
        {
            ReplaceTestStatisticValue(ValueName.ToUpper(), Value.ToString());
        }

        //Called from this class to replace statistical counter for all tests found
        protected abstract void ReplaceTestStatisticValue(string ValueName, string Value);


        private void ReplaceResultTextInternal()
        {
            ReplaceResultTextInternal("ResultText.DoesNotApply", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.DoesNotApply));
            ReplaceResultTextInternal("ResultText.Success", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Success));
            ReplaceResultTextInternal("ResultText.Fatal", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Fatal));
            ReplaceResultTextInternal("ResultText.Inconclusive", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Inconclusive));
            ReplaceResultTextInternal("ResultText.Major", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Major));
            ReplaceResultTextInternal("ResultText.Minor", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Minor));
        }

        private void ReplaceResultTextInternal(string ValueName, string Value)
        {
            ReplaceResultText(ValueName.ToUpper(), Value);

        }

        //Called from this class to replace a description for the different conclusion an item can have (e.g. Success = "OK", Fatal = "Crashed" etc.)
        protected abstract void ReplaceResultText(string ValueName, string Value);



        private void ReplaceAssetConclusionTextInternal()
        {
            ReplaceAssetConclusionTextInternal("AssetText.DoesNotApply", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.DoesNotApply));
            ReplaceAssetConclusionTextInternal("AssetText.Success", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.Success));
            ReplaceAssetConclusionTextInternal("AssetText.Fatal", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.Fatal));
        }

        private void ReplaceAssetConclusionTextInternal(string ValueName, string Value)
        {
            ReplaceAssetConclusionText(ValueName.ToUpper(), Value);
        }

        //Called from this class to replace a Conclusion with a human readable string (OK = The asset successfully retrieved data)
        protected abstract void ReplaceAssetConclusionText(string ValueName, string Value);


        private void ReplaceTestConclusionTextInternal()
        {
            //Replace text values for tests
            ReplaceTestConclusionTextInternal("TestText.DoesNotApply", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.DoesNotApply));
            ReplaceTestConclusionTextInternal("TestText.Success", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Success));
            ReplaceTestConclusionTextInternal("TestText.Fatal", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Fatal));
            ReplaceTestConclusionTextInternal("TestText.Inconclusive", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Inconclusive));
            ReplaceTestConclusionTextInternal("TestText.Major", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Major));
            ReplaceTestConclusionTextInternal("TestText.Minor", ConclusionEnumConverter.TestRecordConclusionDescription(ConclusionEnum.Minor));
        }

        private void ReplaceTestConclusionTextInternal(string ValueName, string Value)
        {
            ReplaceTestConclusionText(ValueName.ToUpper(), Value);
        }

        //Called from this class to replace a Conclusion with a human readable string (OK = The test found no issues)
        protected abstract void ReplaceTestConclusionText(string ValueName, string Value);


        private void ReplaceTestRecommendedActionTextInternal()
        {
            //Recplace recommended action for tests
            ReplaceTestRecommendedActionTextInternal("TestActionText.DoesNotApply", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.DoesNotApply));
            ReplaceTestRecommendedActionTextInternal("TestActionText.Success", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Success));
            ReplaceTestRecommendedActionTextInternal("TestActionText.Fatal", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Fatal));
            ReplaceTestRecommendedActionTextInternal("TestActionText.Inconclusive", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Inconclusive));
            ReplaceTestRecommendedActionTextInternal("TestActionText.Major", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Major));
            ReplaceTestRecommendedActionTextInternal("TestActionText.Minor", ConclusionEnumConverter.TestRecordConclusionRecommendedAction(ConclusionEnum.Minor));
        }

        private void ReplaceTestRecommendedActionTextInternal(string ValueName, string Value)
        {
            ReplaceTestRecommendedActionText(ValueName.ToUpper(), Value);
        }

        //Called from this class to replace a text what the user should do when a test has a given value (Fail = Fix the issue immediately)
        protected abstract void ReplaceTestRecommendedActionText(string ValueName, string Value);




        //Called once when the imlementation should start to begin asset value replacement. Can be used to add a header to the given StringBuilder
        protected abstract void StartAssetDetails(StringBuilder sbAssets);

        //Called by this class for each asset that exists. Imlementation must add the content to the given stringbuilder.
        protected abstract void ProcessAsset(StringBuilder sbAssets, AssetRecord Asset, BaseRecord BaseRec, ResultPrimarySecondary ResultPrimSecond);

        //Called once when the imlementation should end the asset replacement. Can be used to add a footer to the given StringBuilder
        protected abstract void EndAssetDetails(StringBuilder sbAssets);

        //Called once to replace the generated details in the template
        protected abstract void ReplaceAssetList(string ValueName, string AssetList);





        //Called once when the imlementation should start to begin asset value replacement. Can be used to add a header to the given StringBuilder
        protected abstract void StartTestDetails(StringBuilder sbTests);

        //Called by this class for each test that exists. Imlementation must add the content to the given stringbuilder.
        protected abstract void ProcessTest(StringBuilder sbTests, TestRecord Test, BaseRecord BaseRec, ResultPrimarySecondary ResultPrimSecond);

        //Called once when the imlementation should end the test replacement. Can be used to add a footer to the given StringBuilder
        protected abstract void EndTestDetails(StringBuilder sbTests);

        //Called once to replace the generated details in the template
        protected abstract void ReplaceTestList(string ValueName, string TestList);





    }
}
