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

        public BaseException(string Message)
            : base(Message) { }

        public BaseException(string FormatString, params object[] Args)
            : base(string.Format(FormatString, Args)) { }

        public BaseException(string Message, Exception InnerException)
            : base(Message, InnerException) { }

        public BaseException(string FormatString, Exception InnerException, params object[] Args)
            : base(string.Format(FormatString, Args), InnerException) { }

    }
}
