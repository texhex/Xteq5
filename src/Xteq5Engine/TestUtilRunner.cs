using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeadlessPS;
using Yamua;
using System.Reflection;


namespace Xteq5
{
    /// <summary>
    /// This allows to run a test suite (Assets and Tests).
    /// </summary>
    public class Xteq5Runner 
    {

        public Xteq5Runner()
        {
        }
        

        /// <summary>
        /// Executes all assets and tests found in BasePath asynchronously
        /// </summary>
        /// <param name="BasePath">Directory to read data from. Must contain the required subfolders ASSETS, TESTS and MODULES.</param>
        public Report Run(string BasePath)
        {
            Task<Report> task = RunAsync(BasePath);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Executes all assets and tests found in BasePath asynchronously
        /// </summary>
        /// <param name="BasePath">Directory to read data from. Must contain the required subfolders ASSETS, TESTS and MODULES.</param>
        public async Task<Report> RunAsync(string BasePath)
        {
            //Check if all folders are present
            string rootfolder = Path.GetFullPath(BasePath);
            CheckDirectoryExists(BasePath);

            string assetScriptsPath = Path.Combine(rootfolder, Xteq5Constant.DirectoryNameAssets);
            CheckDirectoryExists(assetScriptsPath);

            string testScriptsPath = Path.Combine(rootfolder, Xteq5Constant.DirectoryNameTests);
            CheckDirectoryExists(testScriptsPath);

            string modulePath = Path.Combine(rootfolder, Xteq5Constant.DirectoryNameModules);
            CheckDirectoryExists(modulePath);


            //Create the result object
            Report report = new Report();

            //Set source folder
            report.SourceFolder = BasePath;

            //Everything looks fine so far. Let's go. 
            PSScriptRunnerPreferences prefs = new PSScriptRunnerPreferences();

            //We require at least version 4 of PowerShell
            prefs.RequiredPSVersion = 4;

            //Load modules from this path
            prefs.ModulePath = modulePath;

            //Add Xteq5EngineVersion read-only variable
            prefs.Variables.Add(new VariablePlain(Xteq5Constant.VariableNameEngineVersion, Xteq5Constant.AssemblyVersion, true));
            //Add Xteq5Running read-only variable
            prefs.Variables.Add(new VariablePlain(Xteq5Constant.VariableNameIsActive, true, true));

            
            //Execute all assets
            List<AssetRecord> assets;
            using (PSScriptRunner psScriptRunnerAssets = new PSScriptRunner(prefs))
            {
                //Check that the PowerShell environment is ready. If not, we'll error out from here. 
                psScriptRunnerAssets.TestPowerShellEnvironment();

                //Now execute all assets
                AssetScriptRunner assetRunner = new AssetScriptRunner();
                assets = await assetRunner.Run(psScriptRunnerAssets, assetScriptsPath);
            }


            //Add Xteq5Assets read-only variable
            Hashtable hashtableAssets = CreateHashtableFromAssetRecords(assets);
            prefs.Variables.Add(new VariablePlain(Xteq5Constant.VariableNameAssets, hashtableAssets, true));
            
            //Execute all tests
            List<TestRecord> tests;
            using (PSScriptRunner psScriptRunnerTests = new PSScriptRunner(prefs))
            {
                TestScriptRunner testsRunner = new TestScriptRunner();
                tests = await testsRunner.RunAsync(psScriptRunnerTests, testScriptsPath);
            }


            //Contstruct the final result            
            report.Assets = assets;
            report.Tests = tests;
            
            CalculateRecordStatistics(report, assets, tests);

            report.UserName = Environment.UserName;
            report.ComputerName = Environment.MachineName;
            report.Xteq5Version = Xteq5Constant.AssemblyVersion;


            report.Finish();            
            return report;
        }


        private void CalculateRecordStatistics(Report Report, List<AssetRecord> Assets, List<TestRecord> Tests)
        {
            List<BaseRecord> assetBaseRecords= new List<BaseRecord>();
            foreach (AssetRecord asset in Assets)
            {
                assetBaseRecords.Add(asset);
            }
            Report.AssetStatiscs = new RecordsStatistics(assetBaseRecords);

            List<BaseRecord> testBaseRecords = new List<BaseRecord>();
            foreach (TestRecord test in Tests)
            {
                testBaseRecords.Add(test);
            }
            Report.TestStatiscs = new RecordsStatistics(testBaseRecords);

        }


        private void CheckDirectoryExists(string DirectoryPath)
        {
            if (Directory.Exists(DirectoryPath) == false)
            {
                throw new DirectoryNotFoundException(string.Format("Directory {0} not found", DirectoryPath));
            }
        }


        Hashtable CreateHashtableFromAssetRecords(List<AssetRecord> Assets)
        {
            //Create a case insensitive hashtable
            Hashtable table = System.Collections.Specialized.CollectionsUtil.CreateCaseInsensitiveHashtable();

            foreach (AssetRecord asset in Assets)
            {
                //Ignore any asset that is not Successful
                if (asset.Conclusion == ConclusionEnum.Success)
                {
                    //If the table already contains a key with this name, update it and overwrite the existing entry
                    if (table.ContainsKey(asset.Name))
                    {
                        if (asset.IsDataNativeSet)
                        {
                            table[asset.Name] = asset.DataNative;
                        }
                        else
                        {
                            table[asset.Name] = asset.Data;
                        }

                    }
                    else
                    {
                        //Add new entry 
                        if (asset.IsDataNativeSet)
                        {
                            //Add native data
                            table.Add(asset.Name, asset.DataNative);
                        }
                        else
                        {
                            //Add it as string
                            table.Add(asset.Name, asset.Data);
                        }
                    }

                }
            }

            return table;
        }




    }
}
