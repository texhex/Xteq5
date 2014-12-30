using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TestUtil;
using Yamua;
using Xunit; //If you get "The type or namespace name 'Xunit' could not be found (are you missing a using directive or an assembly reference?)" xUnit needs to be downloaded using NuGet


//To execute these tests in Visual Studio, select Test > Windows > Test Explorer

namespace TestUtilEngine_xUnitTest
{
    public class Assets_Test
    {
        private string GetTestingScriptsFolder()
        {
            //This function should return the path of "TestingScripts\Test1" (e.g. C:\dev\TestUtil\current\TestingScripts\Test1)
            return DebugOnlyHelper.DebugOnlyScriptFolderHelper.TestingScriptDirectory(1);
        }


        [Fact]
        public void Assets_Test__First_TestingScriptsTest1_DirectoryAvailable()
        {
            Assert.True(Directory.Exists(GetTestingScriptsFolder()), string.Format("Unable to find \\TestingScripts\\Test1 folder - tried with path {0}", GetTestingScriptsFolder()));

        }

        [Fact]
        public void Asset_Test__FromFolder_TestingScripts_Subfolder_Test1()
        {
            string baseFolder = GetTestingScriptsFolder();

            if (Directory.Exists(baseFolder) == false)
            {
                Assert.True(false, "Unable to find folder");
            }
            else
            {
                TestUtilRunner runner = new TestUtilRunner();

                Task<Report> task = runner.RunAsync(baseFolder);
                Report result = task.Result;

                Assert.True(result.Assets.Count > 15, "<15 assets found");

                //Perform conistentcheck on all members
                foreach (AssetRecord asset in result.Assets)
                {
                    FieldsAreNotNull(asset);
                    ConclusionIsConsistent(asset);
                    ConclusionMatchesOtherFields(asset);
                }

            }
        }

        private void FieldsAreNotNull(AssetRecord Record)
        {

            Assert.True(Record.Name != null, "Name is null in " + Record.ToString());
            Assert.True(Record.Data != null, "Data is null in " + Record.ToString());
            Assert.True(Record.ScriptFilePath != null, "ScriptFilename is null in " + Record.ToString());
            Assert.True(Record.ProcessMessages != null, "ProcessMessages is null in " + Record.ToString());
        }

        private void ConclusionIsConsistent(AssetRecord Record)
        {
            //A assent can only have the state Success (Has Value), DoesNotApply (Has No Value) or Fatal (Error)
            Assert.True(Record.Conclusion == ConclusionEnum.DoesNotApply |
                        Record.Conclusion == ConclusionEnum.Success |
                        Record.Conclusion == ConclusionEnum.Fatal,
                        "Wrong Conclusion value for: " + Record.ToString());


        }

        private void ConclusionMatchesOtherFields(AssetRecord Record)
        {
            if (Record.Conclusion == ConclusionEnum.Success)
            {
                ConclusionMatchesOtherFields_ForSuccess(Record);
            }

            if (Record.Conclusion == ConclusionEnum.DoesNotApply)
            {
                ConclusionMatchesOtherFields_ForDoesNotApply(Record);
            }

            if (Record.Conclusion == ConclusionEnum.Fatal)
            {
                ConclusionMatchesOtherFields_ForFatal(Record);
            }

        }

        private void ConclusionMatchesOtherFields_ForSuccess(AssetRecord Record)
        {
            //If the Conclusion is success, both name and value must be filled
            Assert.False(string.IsNullOrWhiteSpace(Record.Name), "Name is empty in " + Record.ToString());
            Assert.False(string.IsNullOrWhiteSpace(Record.Data), "Data is empty in " + Record.ToString());
        }

        private void ConclusionMatchesOtherFields_ForDoesNotApply(AssetRecord Record)
        {
            //If the Conclusion is DoesNotApply, Name must be set but value must be empty
            Assert.False(string.IsNullOrWhiteSpace(Record.Name), "Name is empty in " + Record.ToString());
            Assert.True(string.IsNullOrWhiteSpace(Record.Data), "Data is set in " + Record.ToString());
        }

        private void ConclusionMatchesOtherFields_ForFatal(AssetRecord Record)
        {
            //If the Conclusion is Fatal, Name must be set but value must be empty
            Assert.False(string.IsNullOrWhiteSpace(Record.Name), "Name is empty in " + Record.ToString());
            Assert.True(string.IsNullOrWhiteSpace(Record.Data), "Data is set in " + Record.ToString());
        }

    }
}
