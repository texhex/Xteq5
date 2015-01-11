using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public abstract class BaseGenerator
    {
        //Base function that is called by a consumer of an implementation of this class
        public abstract string Generate(Report Report);

    }
}
