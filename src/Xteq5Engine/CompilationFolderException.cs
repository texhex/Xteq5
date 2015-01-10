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

        public CompilationFolderException(string CompilationFolder)
            : base("Compilation folder " + CompilationFolder + " not found")
        {

        }
    }

    /// <summary>
    /// Thrown if the given compilation folder exists, but a required sub folder does not exist
    /// </summary>
    [Serializable]
    public class CompilationSubFolderException : Exception
    {
        private CompilationSubFolderException()
        {
        }

        public CompilationSubFolderException(string CompilationSubFolder)
            : base("Compilation sub folder " + CompilationSubFolder + " not found")
        {

        }
    }

}
