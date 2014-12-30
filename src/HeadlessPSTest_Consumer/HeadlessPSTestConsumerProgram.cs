using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using HeadlessPS;
using System.Management.Automation;
using Yamua;

namespace HeadlessPSTestConsumer
{
    class HeadlessPSTestConsumerProgram
    {
        static void Main(string[] args)
        {

            string basefolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string testscript;
            //testscript = "TestScript_LongRunningOutput.ps1";
            //testscript = "TestScript_LongRunningOutputWithErrors.ps1";
            //testscript = "TestScript_FatalError.ps1";
            //testscript = "TestScript_IncorrectCmdletParameter.ps1";
            //testscript = "TestScript_DataInAllStreams.ps1";
            //testscript = "TestScript_Using out-null.ps1";
            //testscript = "TestScript_ReturnHashtable1.ps1";
            //testscript = "TestScript_ReturnHashtable2.ps1";
            testscript = "TestScript_Using Variables 1.ps1";
            

            string scriptfilename = Path.Combine(basefolder, testscript);

            Version testVersion = new Version(1, 2, 3, 4);

            HashSet<VariablePlain> vars = new HashSet<VariablePlain>();
            vars.Add(new VariablePlain("MyTestVarVersion", testVersion));
            vars.Add(new VariablePlain("MyTestVarWrite","Write Var Set by C#",false));
            vars.Add(new VariablePlain("MyTestVarReadOnly","ReadOnly Var set by C#",true));            


            PSScriptRunnerPreferences prefs = new PSScriptRunnerPreferences();
            prefs.RequiredPSVersion = 4;
            prefs.Variables = vars;

            PSScriptRunner runner = new PSScriptRunner(prefs);
            runner.TestPowerShellEnvironment();

            Task<ExecutionResult> task = RunScriptfileAsync(runner, scriptfilename);
            
            bool finished = false;
            while (finished == false)
            {
                Console.WriteLine("Waiting for runner to finish...");
                finished = task.Wait(333);
            }

            ExecutionResult execResult = task.Result;
            
            Console.WriteLine("Finished!");
            Console.WriteLine(execResult.ToString());

            runner.Dispose();

            Console.WriteLine("Press return to exit...");
            Console.ReadLine();
        }


        //This function allows use to use await - see http://blogs.msdn.com/b/pfxteam/archive/2012/04/12/10293335.aspx
        private static async Task<ExecutionResult> RunScriptfileAsync(PSScriptRunner Runner, string Scriptfile)
        {
            ExecutionResult result;
            result = await Runner.RunScriptFileAsync(Scriptfile);
            Console.WriteLine("Async finished!");
            return result;
        }
    }
}
