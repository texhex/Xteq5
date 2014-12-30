using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessPS
{
    [Serializable]
    public class PowerShellTestFailedException : Exception
    {
        private PowerShellTestFailedException()
        { }

        public PowerShellTestFailedException(string message)
            : base(message) { }

        public PowerShellTestFailedException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PowerShellTestFailedException(string message, Exception innerException)
            : base(message, innerException) { }

        public PowerShellTestFailedException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
