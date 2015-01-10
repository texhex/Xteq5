using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public class BootstrapHTMLGenerator2 : BaseTemplateReplaceGenerator
    {
        public BootstrapHTMLGenerator2()
        {

        }

        public override string Generate(Report Report, string TemplateFilepath)
        {
            ReadTemplate(TemplateFilepath);

            StartGenerating(Report);


            return "";
        }

        protected override void ReplaceHeaderValue(string ValueName, string Value)
        {
            Replace(ValueName, Value);
        }

        private void Replace(string ValueName, string Value)
        {
            _content.Replace("@@" + ValueName + "@@", Value);
        }

    }
}
