using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessPS
{
    [Serializable]
    public class PSTestFailedException : Exception
    {        
        private PSTestFailedException()
        { }

        public PSTestFailedException(string message)
            : base(message) { }

        public PSTestFailedException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PSTestFailedException(string message, Exception innerException)
            : base(message, innerException) { }

        public PSTestFailedException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }         
    }
}
