using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadlessPS
{
    //Detailed description: [Understanding Streams, Redirection, and Write-Host in PowerShell](http://blogs.technet.com/b/heyscriptingguy/archive/2014/03/30/understanding-streams-redirection-and-write-host-in-powershell.aspx)
    //The int for each entry is the stream ID as PowerShell defines it. 
    public enum PSStreamNameEnum
    {
        Output = 1,
        Error = 2,
        Warning = 3,
        Verbose = 4,
        Debug = 5
    }


    public static class HeadlessPSConstant
    {
        //MTH: These are static readonly fields BY PURPOSE (not CONST) to avoid that the compiler performs an copy+paste to the assembly of the consumer of these constants.
        //See [Const Strings - a very convenient way to shoot yourself in the foot](http://www.stum.de/2009/01/14/const-strings-a-very-convenient-way-to-shoot-yourself-in-the-foot/) by [Michael Stum](http://www.stum.de/)        
        
        public static readonly string JustAnExample= "This value is used nowhere"; 


    }

}
