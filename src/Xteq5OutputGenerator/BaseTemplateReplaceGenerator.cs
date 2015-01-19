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
    public abstract class BaseTemplateReplaceGenerator : BaseGenerator
    {
        protected StringBuilder _content = new StringBuilder();

        protected void ReadTemplate(string templateFilepath)
        {
            if (string.IsNullOrWhiteSpace(templateFilepath))
                throw new ArgumentException("TemplateFilepath can not be empty");

            if (PathExtension.FileExists(templateFilepath) == false)
            {
                throw new TemplateFileNotFoundException(templateFilepath);
            }

            //If there is a load error, this will throw an exception
            _content = new StringBuilder(File.ReadAllText(templateFilepath, Encoding.UTF8));
        }

        //Because we require a template, this function can't be used
        public override string Generate(Report report)
        {
            throw new NotImplementedException();
        }

        //Base function that is called by a consumer of an implementation of this class
        public abstract string Generate(Report report, string templateFilepath);


        //This needs to be called by the implementation to start the replacement process
        protected void StartGenerating(Report report)
        {
            ReplaceHeaderValuesInternal(report);

            ReplaceAssetStatisticsInternal(report);
            ReplaceTestStatisticsInternal(report);

            ReplaceResultTextInternal();
            ReplaceAssetConclusionTextInternal();
            ReplaceTestConclusionTextInternal();

            ReplaceTestRecommendedActionTextInternal();

            //Begin details replacement for assets
            StringBuilder sbAssets = new StringBuilder();

            StartAssetDetails(sbAssets);
            foreach (AssetRecord asset in report.Assets)
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
            foreach (TestRecord test in report.Tests)
            {
                BaseRecord baseRec = test as BaseRecord;
                ResultPrimarySecondary rps = new ResultPrimarySecondary(baseRec);

                ProcessTest(sbTests, test, baseRec, rps);
            }
            EndTestDetails(sbAssets);

            ReplaceTestList("TestRows".ToUpper(), sbTests.ToString());


        }


        private void ReplaceHeaderValuesInternal(Report report)
        {
            //Replace all known string values
            ReplaceHeaderValueInternal("ReportID", report.ID.ToString());
            ReplaceHeaderValueInternal("UserName", report.UserName);
            ReplaceHeaderValueInternal("Computername", report.ComputerName);
            ReplaceHeaderValueInternal("UserText", report.UserText);
            ReplaceHeaderValueInternal("SourceFolder", report.CompilationFolder);
            ReplaceHeaderValueInternal("VersionString", report.EngineVersion.ToString());

            //Currently not used by any report
            ReplaceHeaderValueInternal("AssetIssuesFound", report.AssetIssuesFound.ToString());
            ReplaceHeaderValueInternal("TestIssuesFound", report.TestIssuesFound.ToString());
            ReplaceHeaderValueInternal("IssuesFound", report.IssuesFound.ToString());

            //Datetime in UTC and ISO 8601 format without fraction of second
            ReplaceHeaderValueInternal("StartDateTimeUTC", report.StartedUTC.ToString("s") + "Z");
            ReplaceHeaderValueInternal("EndDateTimeUTC", report.EndedUTC.ToString("s") + "Z");
            ReplaceHeaderValueInternal("RuntimeSeconds", report.RuntimeSeconds);
        }

        private void ReplaceHeaderValueInternal(string valueName, string value)
        {
            ReplaceHeaderValue(valueName.ToUpper(), value);
        }

        //Called from this class to replace typical "Header" items: Data that is only found once in the report. 
        protected abstract void ReplaceHeaderValue(string valueName, string value);


        private void ReplaceAssetStatisticsInternal(Report report)
        {
            ReplaceAssetStaticValueInternal("Assets.DoesNotApply", report.AssetStatiscs.DoesNotApplyCount);
            ReplaceAssetStaticValueInternal("Assets.Fatal", report.AssetStatiscs.FatalCount);
            ReplaceAssetStaticValueInternal("Assets.Inconclusive", report.AssetStatiscs.InconclusiveCount);
            ReplaceAssetStaticValueInternal("Assets.Major", report.AssetStatiscs.MajorCount);
            ReplaceAssetStaticValueInternal("Assets.Minor", report.AssetStatiscs.MinorCount);
            ReplaceAssetStaticValueInternal("Assets.Success", report.AssetStatiscs.SuccessCount);
            ReplaceAssetStaticValueInternal("Assets.Total", report.AssetStatiscs.Total);
        }

        private void ReplaceAssetStaticValueInternal(string valueName, int value)
        {
            ReplaceAssetStatisticValue(valueName.ToUpper(), value.ToString());
        }

        //Called from this class to replace statistical counter for all assets found
        protected abstract void ReplaceAssetStatisticValue(string valueName, string value);


        private void ReplaceTestStatisticsInternal(Report report)
        {
            ReplaceTestStatisticValueInternal("Tests.DoesNotApply", report.TestStatiscs.DoesNotApplyCount);
            ReplaceTestStatisticValueInternal("Tests.Fatal", report.TestStatiscs.FatalCount);
            ReplaceTestStatisticValueInternal("Tests.Inconclusive", report.TestStatiscs.InconclusiveCount);
            ReplaceTestStatisticValueInternal("Tests.Major", report.TestStatiscs.MajorCount);
            ReplaceTestStatisticValueInternal("Tests.Minor", report.TestStatiscs.MinorCount);
            ReplaceTestStatisticValueInternal("Tests.Success", report.TestStatiscs.SuccessCount);
            ReplaceTestStatisticValueInternal("Tests.Total", report.TestStatiscs.Total);
        }

        private void ReplaceTestStatisticValueInternal(string valueName, int value)
        {
            ReplaceTestStatisticValue(valueName.ToUpper(), value.ToString());
        }

        //Called from this class to replace statistical counter for all tests found
        protected abstract void ReplaceTestStatisticValue(string valueName, string value);


        private void ReplaceResultTextInternal()
        {
            ReplaceResultTextInternal("ResultText.DoesNotApply", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.DoesNotApply));
            ReplaceResultTextInternal("ResultText.Success", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Success));
            ReplaceResultTextInternal("ResultText.Fatal", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Fatal));
            ReplaceResultTextInternal("ResultText.Inconclusive", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Inconclusive));
            ReplaceResultTextInternal("ResultText.Major", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Major));
            ReplaceResultTextInternal("ResultText.Minor", ConclusionEnumConverter.ConclusionHumanized(ConclusionEnum.Minor));
        }

        private void ReplaceResultTextInternal(string valueName, string value)
        {
            ReplaceResultText(valueName.ToUpper(), value);

        }

        //Called from this class to replace a description for the different conclusion an item can have (e.g. Success = "OK", Fatal = "Crashed" etc.)
        protected abstract void ReplaceResultText(string valueName, string value);



        private void ReplaceAssetConclusionTextInternal()
        {
            ReplaceAssetConclusionTextInternal("AssetText.DoesNotApply", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.DoesNotApply));
            ReplaceAssetConclusionTextInternal("AssetText.Success", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.Success));
            ReplaceAssetConclusionTextInternal("AssetText.Fatal", ConclusionEnumConverter.AssetRecordConclusionDescription(ConclusionEnum.Fatal));
        }

        private void ReplaceAssetConclusionTextInternal(string valueName, string value)
        {
            ReplaceAssetConclusionText(valueName.ToUpper(), value);
        }

        //Called from this class to replace a Conclusion with a human readable string (OK = The asset successfully retrieved data)
        protected abstract void ReplaceAssetConclusionText(string valueName, string value);


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

        private void ReplaceTestConclusionTextInternal(string valueName, string value)
        {
            ReplaceTestConclusionText(valueName.ToUpper(), value);
        }

        //Called from this class to replace a Conclusion with a human readable string (OK = The test found no issues)
        protected abstract void ReplaceTestConclusionText(string valueName, string value);


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

        private void ReplaceTestRecommendedActionTextInternal(string valueName, string value)
        {
            ReplaceTestRecommendedActionText(valueName.ToUpper(), value);
        }

        //Called from this class to replace a text what the user should do when a test has a given value (Fail = Fix the issue immediately)
        protected abstract void ReplaceTestRecommendedActionText(string valueName, string value);




        //Called once when the imlementation should start to begin asset value replacement. Can be used to add a header to the given StringBuilder
        protected abstract void StartAssetDetails(StringBuilder sbAssets);

        //Called by this class for each asset that exists. Imlementation must add the content to the given stringbuilder.
        protected abstract void ProcessAsset(StringBuilder sbAssets, AssetRecord asset, BaseRecord baseRec, ResultPrimarySecondary resultPrimSecond);

        //Called once when the imlementation should end the asset replacement. Can be used to add a footer to the given StringBuilder
        protected abstract void EndAssetDetails(StringBuilder sbAssets);

        //Called once to replace the generated details in the template
        protected abstract void ReplaceAssetList(string valueName, string assetList);





        //Called once when the imlementation should start to begin asset value replacement. Can be used to add a header to the given StringBuilder
        protected abstract void StartTestDetails(StringBuilder sbTests);

        //Called by this class for each test that exists. Imlementation must add the content to the given stringbuilder.
        protected abstract void ProcessTest(StringBuilder sbTests, TestRecord test, BaseRecord baseRec, ResultPrimarySecondary resultPrimSecond);

        //Called once when the imlementation should end the test replacement. Can be used to add a footer to the given StringBuilder
        protected abstract void EndTestDetails(StringBuilder sbTests);

        //Called once to replace the generated details in the template
        protected abstract void ReplaceTestList(string valueName, string testList);





    }
}
