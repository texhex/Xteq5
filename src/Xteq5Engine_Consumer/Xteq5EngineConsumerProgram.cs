using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xteq5;

namespace Xteq5EngineConsumer
{
    class Xteq5EngineConsumerProgram
    {

        static void Main(string[] args)
        {
            Xteq5Runner runner = new Xteq5Runner();

            string basepath="";
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
        private static async Task<Report> RunEngineAsync(Xteq5Runner Runner, string BasePath)
        {
            Report result;
            result = await Runner.RunAsync(BasePath);
            Console.WriteLine("Async finished!");
            return result;
        }
    }
}
