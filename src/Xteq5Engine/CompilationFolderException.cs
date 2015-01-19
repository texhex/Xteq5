using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    /// <summary>
    /// Thrown if the given compilation folder path does not exist
    /// </summary>
    [Serializable]
    public class CompilationFolderException : Exception
    {
        private CompilationFolderException()
        {
        }

        public CompilationFolderException(string compilationFolder)
            : base("Compilation folder " + compilationFolder + " not found")
        {

        }
    }

    /// <summary>
    /// Thrown if the given compilation folder exists, but a required sub folder is missing
    /// </summary>
    [Serializable]
    public class CompilationSubFolderException : Exception
    {
        private CompilationSubFolderException()
        {
        }

        public CompilationSubFolderException(string compilationSubFolder)
            : base("Compilation sub folder " + compilationSubFolder + " not found")
        {

        }
    }

}
