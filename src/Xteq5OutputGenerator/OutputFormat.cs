using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public enum OutputFormatEnum
    {
        //No idea
        Unknown = 0,
        
        //Bootstrap HTML
        HTML = 1,
        
        //Simple XML
        XML = 2,
        
        //JavaScript Object Notation
        JSON = 4
    }

    public static class OutputFormatConverter
    {
        /// <summary>
        /// Tries to parse a report format as string. Returns UNKNOWN if the string could not be undestood
        /// </summary>
        /// <param name="reportFormat">ReportFormat as string</param>
        /// <returns>A ReportFormatEnum</returns>
        public static OutputFormatEnum ParseReportFormat(string reportFormat)
        {
            string dataLowerCase = reportFormat.ToLower(CultureInfo.InvariantCulture);

            switch (dataLowerCase)
            {
                case "html":
                    {
                        return OutputFormatEnum.HTML;
                    }

                case "xml":
                    {
                        return OutputFormatEnum.XML;
                    }

                case "json":
                    {
                        return OutputFormatEnum.JSON;
                    }

                default:
                    {
                        return OutputFormatEnum.Unknown;
                    }
            }
        }

    }
}
