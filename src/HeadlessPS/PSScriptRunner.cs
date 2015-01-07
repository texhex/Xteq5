using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
/* The PowerShell assembly (System.Management.Automation) is either located here:
 *   - c:\Program Files (x86)\Reference Assemblies\Microsoft\WindowsPowerShell\3.0\System.Management.Automation.dll
 *  or
 *   - C:\Windows\Microsoft.NET\assembly\GAC_MSIL\System.Management.Automation\v4.0_3.0.0.0__31bf3856ad364e35\System.Management.Automation.dll
 *  
 *  To find the later path, enter this command within PowerShell console:
 *    [PSObject].Assembly.Location
 *  
 *  This tip is taken from [kravits88](http://stackoverflow.com/users/1427220/kravits88) via [Referencing system.management.automation.dll in Visual Studio](http://stackoverflow.com/questions/1186270/referencing-system-management-automation-dll-in-visual-studio).
 */
using System.Management.Automation;
using System.Management.Automation.Runspaces; //required for InitialSessionState
using Yamua;


namespace HeadlessPS
{
    /// <summary>
    /// A slim API around System.Automation.PowerShell which hopefully makes things a little bit easier to interact with PowerShell if the following use case is given:
    /// - A bunch of scripts (*.ps1) should mainly be executed
    /// - An instance of this object is used in a loop to do execute the scripts
    /// - Modules are stored in one path and all modules should be loaded 
    /// - Required PowerShell version is at least 4.0
    /// - A human-readable abstract is required 
    /// - Direct exception handling of PowerShell related exceptions is not needed, rather after the script has finished the result is checked
    /// 
    /// Base class started with the excellent example: [Executing PowerShell scripts from C#](http://blogs.msdn.com/b/kebab/archive/2014/04/28/executing-powershell-scripts-from-c.aspx) from [Keith Babinec](https://social.msdn.microsoft.com/profile/keith%20babinec/).
    /// </summary>
    public class PSScriptRunner : DisposableObject
    {
        PSScriptRunnerPreferences _prefs;

        //The only difference I found between CreateDefault() and CreateDefault2() is inside "Notes" of Get-Module: http://technet.microsoft.com/en-us/library/hh849700.aspx
        InitialSessionState _initialSessionState = InitialSessionState.CreateDefault2(); //Docs: http://msdn.microsoft.com/en-us/library/system.management.automation.runspaces.initialsessionstate(v=vs.85).aspx
        PowerShell _psInstance = null; //Docs: http://msdn.microsoft.com/en-us/library/system.management.automation.powershell(v=vs.85).aspx
        PSDataCollection<PSObject> _outputStreamCollection = null; //Docs: http://msdn.microsoft.com/en-us/library/dd144531%28v=vs.85%29.aspx

        IAsyncResult _executionWaitHandle = null;

        ExecutionResult _result = null;


        public PSScriptRunner()
            : this(new PSScriptRunnerPreferences())
        {
        }

        public PSScriptRunner(PSScriptRunnerPreferences prefs)
            : base()
        {
            _prefs = prefs;

            /* MTH: Throw an exception if there is an error when opening a runspace. This is FALSE by default. 
             * This is set because if this is FALSE, PowerShell does not report anything about broken modules. 
             */
            _initialSessionState.ThrowOnRunspaceOpenError = true;
            
            //Replace PSAuthorizationManager with a null manager which ignores execution policy.
            //This is required because else no script will be allowed to run if ExecutionPoliy is not at least RemoteSigned.
            //Because we do not set our own ShellId, the parameter is set to "Microsoft.PowerShell"
            //Source: [Bypassing Restricted Execution Policy in Code or in Script](http://www.nivot.org/blog/post/2012/02/10/Bypassing-Restricted-Execution-Policy-in-Code-or-in-Script) by Nivot Ink
            _initialSessionState.AuthorizationManager = new System.Management.Automation.AuthorizationManager("Microsoft.PowerShell"); 

            //Import modules if a module path is set
            if (string.IsNullOrWhiteSpace(prefs.ModulePath) == false)
            {
                _initialSessionState.ImportPSModulesFromPath(prefs.ModulePath);
            }

            //Set variables (if any)
            foreach (VariablePlain var in _prefs.Variables)
            {
                //Variables are always created with AllScope because it should be visible and writable even if a script runs an other script using: & "otherscript.ps1"
                ScopedItemOptions scopeOptions = ScopedItemOptions.AllScope; //Docs: http://msdn.microsoft.com/query/dev12.query?appId=Dev12IDEF1&l=EN-US&k=k%28System.Management.Automation.ScopedItemOptions%29;k%28TargetFrameworkMoniker-.NETFramework

                if (var.ReadOnly)
                {
                    //.ReadOnly would also be an option but .ReadOnly variables can be removed while constanct can not 
                    scopeOptions = scopeOptions | ScopedItemOptions.Constant;

                    //If trying to write to a variable with .Constanct set, PowerShell will issue the error: Cannot overwrite variable NAME because it is read-only or constant.
                }

                _initialSessionState.Variables.Add(new SessionStateVariableEntry(var.Name, var.Value, string.Empty, scopeOptions));
            }

            //MTH: If we every want to change the value of $WarningPreference, $VerbosePreference or $DebugPreference, this can be done by using _initialSessionState.Variables

        }


        /// <summary>
        /// Perform some basic tests of the PowerShell environment, including trying to load the modules you wish to use. If this test passes, PowerShell is ready for your scripts.
        /// If the test fails, a PowerShellTestFailedException is thrown.
        /// </summary>
        /// <param name="MinimumPowerShellVersion">Minimum PowerShell version that is required</param>
        public void TestPowerShellEnvironment()
        {
            string currentPosition = "Start";

            try
            {
                Collection<PSObject> outputCollection;
                

                currentPosition = "Creating PowerShell runspace";
                using (PowerShell psTest = PowerShell.Create(_initialSessionState))
                {
                    currentPosition = "After creating PowerShell runspace";

                    currentPosition = "Before addition test";
                    //MTH: Sorry, couldn't resist. 
                    psTest.AddScript("[int]$AUQLUE=41+1; $AUQLUE");
                    outputCollection = psTest.Invoke();
                    currentPosition = "After addition test";

                    if (outputCollection.Count != 1)
                    {
                        throw new PowerShellTestFailedException("Return count is not 1");
                    }

                    if (outputCollection[0].BaseObject.ToString() != "42")
                    {
                        throw new PowerShellTestFailedException("Return value is not 42");
                    }

                    //Test #2: Check the compatible version
                    currentPosition = "Before PSVersionTable test";
                    psTest.AddScript("$PSVersionTable.PSCompatibleVersions");
                    outputCollection = psTest.Invoke();
                    currentPosition = "After PSVersionTable test";

                    if (outputCollection.Count < 2)
                    {
                        throw new PowerShellTestFailedException("Return count is smaller than 2");
                    }

                    currentPosition = "PSCompatibleVersions return";

                    bool compatibleVersionDetected = false;
                    foreach (PSObject pso in outputCollection)
                    {
                        Version ver = pso.BaseObject as Version;

                        if (ver.Major == _prefs.RequiredPSVersion)
                        {
                            compatibleVersionDetected = true;
                            break;
                        }
                    }

                    if (compatibleVersionDetected == false)
                    {
                        throw new PowerShellTestFailedException("No compatible PowerShell version ({0}.x) found", _prefs.RequiredPSVersion);
                    }

                }

            }
            catch (PowerShellTestFailedException exp)
            {
                throw new PowerShellTestFailedException("PowerShellTest failed at [{0}]: {1}", currentPosition, exp.Message);
            }
            catch (Exception ex) //MTH: Catch any is bad, but only that way we can pass "currentPosition" to get an idea where the problem is located. 
            {
                throw new PowerShellTestFailedException("PowerShellTest failed at [{0}]: {1}", ex, currentPosition, ex.Message);
            }
        }

        bool _withinRunScriptInline = false; //TRUE = A script is currently beeing executed

        //TODO: Write test when script is empty - should throw exception
        //TODO: Write test when function is called second time if there is something executing 

        /// <summary>
        /// Executes a script file.
        /// This function can throw an exception if there is a problem with PowerShell or this object itself, but not for any script related error (e.g. Syntax Errors). 
        /// </summary>
        /// <param name="Filename">Path to script file that will be executed</param>
        public ExecutionResult RunScriptFile(string Filename)
        {
            Task<ExecutionResult> task = RunScriptFileAsync(Filename);
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Executes a script file asynchronously.
        /// This function can throw an exception if there is a problem with PowerShell or this object itself, but not for any script related error (e.g. Syntax Errors). 
        /// </summary>
        /// <param name="Filename">Path to script file that will be executed</param>
        public async Task<ExecutionResult> RunScriptFileAsync(string Filename)
        {
            if (string.IsNullOrWhiteSpace(Filename))
                throw new ArgumentException("No PowerShell script file specified");

            //MTH: We need to check if the file exists because PowerShell will be happy to accept a non-existing file to be executed and simply assumes that it is an incorrect cmdlet name.
            if (File.Exists(Filename) == false)
                throw new FileNotFoundException(string.Format("Script file {0} not found", Filename));

            //MTH: Make sure that we have an absolute and not a relative path. In the later case PowerShell wants the script to be dot sourced.
            FileInfo fi = new FileInfo(Filename);

            //As the filename can contain blank spaces, we add " around it and tell PowerShell to parse it as a command (&), not as a string
            string script = "& \"" + fi.FullName + "\"";

            ExecutionResult execResult = await RunInlineScriptAsync(script);
            return execResult;
        }


        /// <summary>
        /// Executes an inline script.
        /// This function can throw an exception if there is a problem with PowerShell or this object itself, but not for any script related error (e.g. Syntax Errors). 
        /// </summary>
        /// <param name="Script">Inline PowerShell script to be executed</param>
        public ExecutionResult RunInlineScript(string Script)
        {
            Task<ExecutionResult> task = RunInlineScriptAsync(Script);
            task.Wait();
            return task.Result;
        }


        /// <summary>
        /// Executes an inline script asynchronously. 
        /// This function can throw an exception if there is a problem with PowerShell or this object itself, but not for any script related error (e.g. Syntax Errors). 
        /// </summary>
        /// <param name="Script">Inline PowerShell script to be executed</param>
        public async Task<ExecutionResult> RunInlineScriptAsync(string Script)
        {
            if (string.IsNullOrWhiteSpace(Script))
                throw new ArgumentException("No PowerShell script specified");

            if (_withinRunScriptInline == true)
                throw new InvalidOperationException("Can not execute two scripts at the same time");


            _result = null;
            _withinRunScriptInline = true;
            try
            {
                //Dispose any left over objects
                DisposeManagedResources();

                /* MTH: There is a known (and confirmed) memory leak within PowerShell when creating a RunSpace manually in PowerShell 3.0:
                 * [Use RunSpacePool instead of RunSpace because of MemoryLeak](http://stackoverflow.com/questions/23234170/memory-leak-using-powershell-remote-calls-in-c-sharp)
                 * I was able to confirm that this still happens when using PowerShell 4.0 from within a script but *not* when using PowerShell from C# although this will also create a RunSpace for each RunXXScriptXXX() method we receive.
                 * The only solution seems to be to create a runspace pool like this:
                 * using (RunspacePool rsp = RunspaceFactory.CreateRunspacePool()) {
                 *   rsp.Open();
                 *   PowerShell psInst = PowerShell.Create();        
                 *   psInst.RunspacePool = rsp;
                 * }
                 */

                //Prepare the result object
                ExecutionResult execResult;
                try
                {
                    //Create a new instance to make sure nothing from one script is carried over to another
                    //If there is a faulty module that PowerShell can't load, this call will result in an exception and _psInstance will be NULL
                    _psInstance = PowerShell.Create(_initialSessionState);

                    //I still try to figure out was LocalScope (second parameter) means - http://msdn.microsoft.com/en-us/library/dd182435%28v=vs.85%29.aspx
                    _psInstance.AddScript(Script); 

                    _executionWaitHandle = _psInstance.BeginInvoke();

                    //MTH: Await the result from the execution using a task object
                    _outputStreamCollection = await Task.Factory.FromAsync<PSDataCollection<PSObject>>(_executionWaitHandle, _psInstance.EndInvoke);

                    /* MTH: About the call above: Best practise for BeginInvoke() clearly say that you have to call EndInvoke() as well.
                     * However, during all test I made the exception PowerShell throws in EndInvoke() are already available in _psInstance.InvocationStateInfo.Reason. 
                     * It seems this call to EndInvoke() is redundant as the exception is already available. Hence, EndInvoke() does only slow the execution down but does not add any additonal information.
                     * But I do not know if this information is also true if there is an Runspace Exception or any other fatal error. 
                     */
                    execResult = new ExecutionResult(_psInstance, _prefs.Variables, _outputStreamCollection);
                }
                catch (Exception exc)
                {
                    //We do not have an outputCollection here because EndInvoke throw an exception
                    execResult = new ExecutionResult(_psInstance, _prefs.Variables, exc);
                }

                //Save the result we have generated to later on be able to dispose it. 
                _result = execResult;
                _withinRunScriptInline = false;
                return _result;
            }
            catch
            {
                //Something got terrible wrong at the wrong place. We will need to clean up before rethrowing the exception.
                DisposeManagedResources();
                throw;
            }

        }

        protected override void DisposeManagedResources()
        {
            if (_outputStreamCollection != null)
            {
                _outputStreamCollection.Dispose();
                _outputStreamCollection = null;
            }

            if (_psInstance != null)
            {
                _psInstance.Dispose();
                _psInstance = null;
            }

            if (_executionWaitHandle != null)
            {
                //MTH: Best practice is to close the wait handle, if there is one.
                //See [IAsyncResult.AsyncWaitHandle Property](http://msdn.microsoft.com/en-us/library/system.iasyncresult.asyncwaithandle%28v=vs.110%29.aspx)
                if (_executionWaitHandle.AsyncWaitHandle != null)
                {
                    _executionWaitHandle.AsyncWaitHandle.Close();
                }

                _executionWaitHandle = null;
            }

            if (_result != null)
            {
                _result.Dispose();
                _result = null;
            }

            base.DisposeManagedResources();
        }





    }




}
