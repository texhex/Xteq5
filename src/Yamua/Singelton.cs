using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yamua.Templates
{
    //From: C# in Depth
    //http://csharpindepth.com/Articles/General/Singleton.aspx
    public sealed class Singleton
    {
        private Singleton()
        {
        }

        #region Singelton code
        public static Singleton Instance { get { return Nested.instance; } }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly Singleton instance = new Singleton();
        }
        #endregion

    }
}
