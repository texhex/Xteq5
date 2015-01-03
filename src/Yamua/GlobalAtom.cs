using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Yamua
{
    //A class to create and query for global atoms 
    //see http://www.codeproject.com/KB/dotnet/csharponeinst.aspx
    public sealed class GlobalAtoms
    {
 
        GlobalAtoms()
        {            
        }

        public bool AtomExists(string AtomName)
        {
            ushort Atom = SafeNativeMethods.GlobalFindAtom(AtomName);
            if (Atom > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool DeleteAtom(string AtomName)
        {
            ushort Atom = SafeNativeMethods.GlobalFindAtom(AtomName);
            if (Atom > 0)
            {
                SafeNativeMethods.GlobalDeleteAtom(Atom);
                return true;
            }
            else
            {
                return false;
            }

        }

        public void CreateAtom(string AtomName)
        {
            SafeNativeMethods.GlobalAddAtom(AtomName);

        }

        internal static class SafeNativeMethods  
        {

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, ThrowOnUnmappableChar=true, BestFitMapping=false)]
            internal static extern ushort GlobalAddAtom(string lpString);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi, ThrowOnUnmappableChar=true, BestFitMapping=false)]
            internal static extern ushort GlobalFindAtom(string lpString);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
            internal static extern ushort GlobalDeleteAtom(ushort atom);
        }

        
#region SingleTon handling
        public static GlobalAtoms Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly GlobalAtoms instance = new GlobalAtoms();
        }
#endregion
    }
}
