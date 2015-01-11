using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xteq5;
using Yamua;
using CMDLine;

/* Xteq5CLI Properties > Debug > Start options > Command line arguments
   -help
   /?
   -Path C:\x
   -Run 
   -Run -Path "C:\Users\TeX HeX\AppData\Local\Temp"
   -Run -Path "C:\Users\TeX HeX\AppData\Local\Temp" -Format zxY
   -Run -Path "C:\ProgramData\Xteq5" 
   -Run -Path "C:\ProgramData\Xteq5" -Format HTML -Filename "???.axt"
   -Run -Path "C:\ProgramData\Xteq5" -Format HTML -Filename "C:\Temp\batchresult.htm"
   -Run -Path "C:\ProgramData\Xteq5" -Format HTML -Filename "C:\Temp\batchresult.htm" -Text 
   -Run -Path "C:\ProgramData\Xteq5" -Format HTML -Filename "C:\Temp\batchresult.htm" -Text "My comment"
   -Run -Filename "C:\Temp\batchresult.htm" 
   -Run -Filename "C:\Temp\daresult.xml" -Format XML
   -Run -Filename "C:\Temp\daresult.xml" -Format XML -Text "blah BLUB"
   -Run -Path "C:\dev\git\Xteq5\scripts" -Format XML -Filename "C:\Temp\result.xml" -Text "My comment"
 
*/
namespace Xteq5CLI
{
    class Xteq5CLIMain
    {
        //Return codes: [System Error Codes (0-499)](http://msdn.microsoft.com/en-us/library/windows/desktop/ms681382%28v=vs.85%29.aspx)
        public const int ERROR_SUCCESS = 0; //Everything worked, no issues detected
        public const int ERROR_INVALID_DATA = 13; //Execution worked, but at least one asset or test has the status FATAL, MAJOR or MINOR
        public const int ERROR_BAD_COMMAND = 22; //Did not work at all. Wrong arguments, compilation folder not found, PowerShell broken etc. 

        static int Main(string[] args)
        {
            //Set to error code by default
            int returnCode = ERROR_BAD_COMMAND;

            Console.WriteLine("Xteq5CLI " + AssemblyInformation.Version);
            Console.WriteLine(AssemblyInformation.Copyright);
            Console.WriteLine(AssemblyInformation.Description);
            Console.WriteLine();


            //Create the work order. If something isn't right with the arguments, WorkOrder will print to console automatically
            WorkInstruction instruction = new WorkInstruction(args);

            if (instruction.Execute == false)
            {
                Console.WriteLine("Nothing to do, exiting.");
            }
            else
            {
                Console.WriteLine("Compilation folder: " + instruction.CompilationPath);

                //Create default UIRunner
                SimplifiedXteq5Runner simpleRunner = new SimplifiedXteq5Runner(instruction.CompilationPath);

                if (instruction.GenerateReport)
                {
                    Console.WriteLine("Destination file  : " + instruction.DestinationFilepath);
                    Console.WriteLine("Destination format: " + instruction.DestinationFormat.ToString());
                    
                    if (string.IsNullOrWhiteSpace(instruction.UserText)==false)
                       Console.WriteLine("Additional text   : " + instruction.UserText);

                    simpleRunner = new SimplifiedXteq5Runner(instruction.CompilationPath, instruction.UserText, instruction.DestinationFormat, instruction.DestinationFilepath);
                }

                //Start the runner
                Console.WriteLine();
                Console.WriteLine("Running, please wait...");

                bool result = simpleRunner.Run();
                if (result == true)
                {
                    //Execution finished
                    Console.WriteLine("Finished.");
                    Console.WriteLine();

                    //Check if these is an issue in order to set the return code correctly
                    if (simpleRunner.Report.IssuesFound)
                    {
                        Console.WriteLine("There is at least one asset or test that reports an issue");
                        returnCode = ERROR_INVALID_DATA;
                    }
                    else
                    {
                        Console.WriteLine("No issues found. Your system is operating within established parameters.");
                        returnCode = ERROR_SUCCESS;
                    }
                }
                else
                {
                    //This did not work at all
                    Console.WriteLine();
                    Console.WriteLine("Error: ");
                    Console.WriteLine("  " + simpleRunner.FailedMessage);
                    Console.WriteLine();
                    Console.WriteLine("Details:");
                    Console.WriteLine("  " + simpleRunner.FailedException.GetType().ToString() + " - " + simpleRunner.FailedException.Message);
                }

                //All done
            }


            Console.WriteLine();
            Console.WriteLine("Exiting, return code is {0}.", returnCode);
#if DEBUG
            Console.ReadLine();
#endif

            return returnCode;
        }
    }
}
