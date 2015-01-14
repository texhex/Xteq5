using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    /// <summary>
    /// A helper class to report status using the IProgress interface and reporting back a class (which requires a default constructor).
    /// Note: An instance of this class can only be used ONCE to report status. Create a new instance to report status again.
    /// </summary>
    /// <typeparam name="ReportedObject">The object beeing reported when calling Report()</typeparam>
    public class ProgressReporter<ReportedObject> where ReportedObject : class, new()
    {
        IProgress<ReportedObject> _iProgress;
        bool _burned = false;

        /// <summary>
        /// Provides access to the instance that is beeing reported as progress of the async function
        /// </summary>
        public ReportedObject Content { get; private set; }

        /// <summary>
        /// Create an instance of this class, requires the IProgress<T> that is used to report progress
        /// </summary>
        /// <param name="IProgress"></param>
        public ProgressReporter(IProgress<ReportedObject> IProgress)
        {
            _iProgress = IProgress;
            Content = new ReportedObject();
        }

        /// <summary>
        /// Reports content back. Can be used only ONCE.
        /// </summary>
        public void Report()
        {
            //MTH: We need to make sure that an instance of this class is used only once because ReportedObject might be used in a entire different thread. 
            //Quote from [Reporting Progress from Async Tasks](http://blog.stephencleary.com/2012/02/reporting-progress-from-async-tasks.html) by Stephen Cleary
            //"...it means you can’t modify the progress object after it’s passed to Report. It is an error to keep a single “current progress” object, update it, and repeatedly pass it to Report."
            if (_burned == false)
            {
                //Set _burned no matter if we have an actual IProgress or not to avoid that a wrong use of this function is undetected.
                _burned = true;

                if (_iProgress != null)
                {
                    _iProgress.Report(Content);
                }
            }
            else
            {
                //Seems you didn't read the part about "USE IT ONLY ONCE".
                //Here's an exception for you.
                //You're welcome. 
                throw new InvalidOperationException("Report() can only be used once.");
            }
        }


    }
}
