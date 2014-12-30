using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtil;

namespace TestUtilEngineConsumer
{
    class TestUtilEngineConsumerProgram
    {

        static void Main(string[] args)
        {
            TestUtilRunner runner = new TestUtilRunner();

            string basepath="";
            //basepath = @"C:\dev\TestUtil\current\TestingScripts\Test1";
            basepath = DebugOnlyHelper.DebugOnlyScriptFolderHelper.TestingScriptDirectory();
            basepath = DebugOnlyHelper.DebugOnlyScriptFolderHelper.TestingScriptDirectory(9);


            Task<Report> task = RunEngineAsync(runner, basepath);

            bool finished = false;
            while (finished == false)
            {
                Console.WriteLine("Waiting for runner to finish...");
                finished = task.Wait(333);
            }

            Report result = task.Result;


            Console.WriteLine("Press return to exit...");
            Console.ReadLine();
        }


        //This function allows use to use await - see http://blogs.msdn.com/b/pfxteam/archive/2012/04/12/10293335.aspx
        private static async Task<Report> RunEngineAsync(TestUtilRunner Runner, string BasePath)
        {
            Report result;
            result = await Runner.RunAsync(BasePath);
            Console.WriteLine("Async finished!");
            return result;
        }
    }
}
