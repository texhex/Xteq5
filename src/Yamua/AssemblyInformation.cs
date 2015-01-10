using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    /// <summary>
    /// Helper class to get basic information from the AssemblyInfo file at runtime
    /// </summary>
    public static class AssemblyInformation
    {

        public static Version Version
        {
            get
            {
                Assembly caller = Assembly.GetCallingAssembly();
                return caller.GetName().Version;
            }
        }

        public static string Copyright
        {
            get
            {
                Assembly caller = Assembly.GetCallingAssembly();
                AssemblyCopyrightAttribute copyright = AssemblyCopyrightAttribute.GetCustomAttribute(caller, typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
                return copyright.Copyright;
            }
        }

        public static string Description
        {
            get
            {
                Assembly caller = Assembly.GetCallingAssembly();
                AssemblyDescriptionAttribute description = AssemblyDescriptionAttribute.GetCustomAttribute(caller, typeof(AssemblyDescriptionAttribute)) as AssemblyDescriptionAttribute;
                return description.Description;
            }
        }
    }
}
