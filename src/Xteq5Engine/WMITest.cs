using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace Xteq5
{
    /// <summary>
    /// A simple test that checks if WMI is working or not. 
    /// </summary>
    public class WMITest
    {
        public WMITest()
        {

        }

        /// <summary>
        /// Runs the WMI test. If the test fails, an instance of type WMITestException() is thrown
        /// </summary>
        public void Test()
        {
            try
            {
                //Standard WMI path
                ManagementScope scope = new ManagementScope(@"root\cimv2");
                scope.Connect();

                //Simple query
                ObjectQuery query = new ObjectQuery("Select * from Win32_OperatingSystem");

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    searcher.Options.Timeout = new TimeSpan(0, 0, 10); //create timeout value of 10 seconds

                    //Execute the query
                    using (ManagementObjectCollection result = searcher.Get())
                    {
                        //The value itself is not interessting, only if exactly one object was returned. This means WMI is working. 
                        if (result.Count != 1)
                        {
                            throw new WMITestException("WMI test failed; expected result was not returned by WMI");
                        }
                    }

                }

            }
            catch (Exception exc)
            {
                if (exc is WMITestException)
                {
                    //If the exception is already the type we want, just rethrow it
                    throw;
                }
                else
                {
                    throw new WMITestException("WMI test failed - " + exc.Message, exc);

                }

            }


        }
    }

    /// <summary>
    /// Thrown if the WMI test has failed
    /// </summary>
    [Serializable]
    public class WMITestException : Exception
    {
        private WMITestException()
        {
        }

        public WMITestException(string explanation)
            : base(explanation)
        {
        }

        public WMITestException(string message, Exception innerException)
            : base(message, innerException) { }

    }
}
