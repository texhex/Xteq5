using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    [Serializable]
    //A base class for custom exceptions. 
    public abstract class BaseException : Exception
    {
        public BaseException()
        { }

        public BaseException(string message)
            : base(message) { }

        public BaseException(string formatString, params object[] args)
            : base(string.Format(formatString, args)) { }

        public BaseException(string message, Exception innerException)
            : base(message, innerException) { }

        public BaseException(string formatString, Exception innerException, params object[] args)
            : base(string.Format(formatString, args), innerException) { }

    }
}
