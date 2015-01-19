using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    /// <summary>
    /// Thrown if the given template file does not exist
    /// </summary>
    [Serializable]
    public class TemplateFileNotFoundException : Exception
    {
        private TemplateFileNotFoundException()
        {
        }

        public TemplateFileNotFoundException(string templateFilepath)
            : base("Template file " + templateFilepath + " not found")
        {

        }
    }
}
