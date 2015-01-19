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
    /// This processes an compilation folder (Assets and Tests)
    /// </summary>
    public class Xteq5Runner
    {

        public Xteq5Runner()
        {
        }


        /// <summary>
        /// Executes all assets and tests found in CompilationPath asynchronously
        /// </summary>
        /// <param name="compilationPath">Directory to read data from. Must contain the required subfolders ASSETS, TESTS and MODULES.</param>
        public Report Run(string compilationPath)
        {
            Task<Report> task = RunAsync(compilationPath);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Executes all assets and tests found in CompilationPath asynchronously
        /// </summary>
        /// <param name="compilationPath">Directory to read data from. Must contain the required subfolders ASSETS, TESTS and MODULES.</param>
        /// <param name="progress">An IProgress implementation to report status to</param>
        public async Task<Report> RunAsync(string compilationPath, IProgress<RunnerProgressDetail> progress = null)
        {
            if (string.IsNullOrWhiteSpace(compilationPath))
                throw new ArgumentException("Compilation path is not set");

            //Check if all folders are present
            string rootfolder = PathExtension.FullPath(compilationPath);
            if (PathExtension.DirectoryExists(rootfolder) == false)
                throw new CompilationFolderException(rootfolder);

            //Check subfolders
            string assetScriptsPath = PathExtension.Combine(rootfolder, Xteq5EngineConstant.DirectoryNameAssets);
            CheckCompilationSubfolder(assetScriptsPath);

            string testScriptsPath = PathExtension.Combine(rootfolder, Xteq5EngineConstant.DirectoryNameTests);
            CheckCompilationSubfolder(testScriptsPath);

            string modulePath = PathExtension.Combine(rootfolder, Xteq5EngineConstant.DirectoryNameModules);
            CheckCompilationSubfolder(modulePath);


            //Create the result object
            Report report = new Report();

            //Set source folder
            report.CompilationFolder = compilationPath;

            //Everything looks fine so far. Let's go. 
            PSScriptRunnerPreferences prefs = new PSScriptRunnerPreferences();

            //We require at least version 4 of PowerShell
            prefs.RequiredPSVersion = 4;

            //Load modules from this path
            prefs.ModulePath = modulePath;

            //Add Xteq5EngineVersion read-only variable
            prefs.Variables.Add(new VariablePlain(Xteq5EngineConstant.VariableNameEngineVersion, Xteq5EngineConstant.Version, true));
            //Add Xteq5Running read-only variable
            prefs.Variables.Add(new VariablePlain(Xteq5EngineConstant.VariableNameIsActive, true, true));


            //Execute all assets
            List<AssetRecord> assets;
            using (PSScriptRunner psScriptRunnerAssets = new PSScriptRunner(prefs))
            {
                //Check that the PowerShell environment is ready. If not, we'll error out from here. 
                psScriptRunnerAssets.TestPowerShellEnvironment();

                //Now execute all assets
                AssetScriptRunner assetRunner = new AssetScriptRunner();
                assets = await assetRunner.Run(psScriptRunnerAssets, assetScriptsPath, progress);
            }


            //Add Xteq5Assets read-only variable
            Hashtable hashtableAssets = CreateHashtableFromAssetRecords(assets);
            prefs.Variables.Add(new VariablePlain(Xteq5EngineConstant.VariableNameAssets, hashtableAssets, true));

            //Execute all tests
            List<TestRecord> tests;
            using (PSScriptRunner psScriptRunnerTests = new PSScriptRunner(prefs))
            {
                TestScriptRunner testsRunner = new TestScriptRunner();
                tests = await testsRunner.RunAsync(psScriptRunnerTests, testScriptsPath, progress);
            }


            //Contstruct the final result            
            report.UserName = Environment.UserName;
            report.ComputerName = Environment.MachineName;
            report.EngineVersion = Xteq5EngineConstant.Version;

            report.Assets = assets;
            report.Tests = tests;

            CalculateRecordStatistics(report, assets, tests);

            //Set IssuesFound
            report.IssuesFound = false;
            report.TestIssuesFound = false;
            report.AssetIssuesFound = false;

            if ((report.AssetStatiscs.FatalCount + report.AssetStatiscs.MajorCount + report.AssetStatiscs.MinorCount) > 0)
                report.AssetIssuesFound = true;

            if ((report.TestStatiscs.FatalCount + report.TestStatiscs.MajorCount + report.TestStatiscs.MinorCount) > 0)
                report.TestIssuesFound = true;

            if (report.AssetIssuesFound || report.TestIssuesFound)
                report.IssuesFound = true;


            report.Finish();
            return report;
        }

        private void CalculateRecordStatistics(Report report, List<AssetRecord> assets, List<TestRecord> tests)
        {
            List<BaseRecord> assetBaseRecords = new List<BaseRecord>();
            foreach (AssetRecord asset in assets)
            {
                assetBaseRecords.Add(asset);
            }
            report.AssetStatiscs = new RecordsStatistics(assetBaseRecords);

            List<BaseRecord> testBaseRecords = new List<BaseRecord>();
            foreach (TestRecord test in tests)
            {
                testBaseRecords.Add(test);
            }
            report.TestStatiscs = new RecordsStatistics(testBaseRecords);

        }


        private void CheckCompilationSubfolder(string subfolderPath)
        {
            if (PathExtension.DirectoryExists(subfolderPath) == false)
            {
                throw new CompilationSubFolderException(subfolderPath);
            }
        }


        Hashtable CreateHashtableFromAssetRecords(List<AssetRecord> assets)
        {
            //Create a case insensitive hashtable
            Hashtable table = System.Collections.Specialized.CollectionsUtil.CreateCaseInsensitiveHashtable();

            foreach (AssetRecord asset in assets)
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
