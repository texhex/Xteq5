using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yamua;

namespace Xteq5
{
    /// <summary>
    /// This class provides access to all output generators (HTML, XML etc.) that are currently supported
    /// </summary>
    public static class OutputGenerator
    {

        /// <summary>
        /// Generates a report in the requested format, based on the given Report object. 
        /// </summary>
        /// <param name="Report">Report to generate output for</param>
        /// <param name="Format">Format that should be generated</param>
        /// <returns>A string representation (source) of the requested output</returns>
        public static string GenerateReportOutput(Report Report, OutputFormatEnum Format)
        {
            switch (Format)
            {
                case OutputFormatEnum.HTML:
                    string htmlTemplatePath = PathExtension.Combine(Report.CompilationFolder, "BootstrapTemplate1.html");

                    BootstrapHTMLGenerator2 htmlGenerator = new BootstrapHTMLGenerator2();
                    return htmlGenerator.Generate(Report, htmlTemplatePath);

                default:
                    throw new NotImplementedException("Only HTML is supported at this time");
            }
             
        }

        /// <summary>
        /// Generates a report in the requested format, based on the given Report object and save it to a file.
        /// If Filename is empty, a filename in the TEMP folder of the current user will be generated
        /// </summary>
        /// <param name="Report">Report to generate output for</param>
        /// <param name="Format">Format that should be generated</param>
        /// <param name="Format">Filepath to store the result. If empty, a filename will be generated</param>
        /// <returns>Filepath of the generated and saved output</returns>
        public static string GenerateReportOutputFile(Report Report, OutputFormatEnum Format, string Filepath)
        {
            string fileExtension = "";

            switch (Format)
            {
                case OutputFormatEnum.HTML:
                    fileExtension=".html";
                    break;

                default:
                    throw new NotImplementedException("Only HTML is supported at this time");
            }

            string source = GenerateReportOutput(Report, Format);

            string fullFilename = Filepath;
            if (string.IsNullOrWhiteSpace(fullFilename))
            {
                string tempPath = Path.GetTempPath();
                string datepart = string.Format("{0:yyyy-MM-dd_HHmm}", Report.EndedUTC); //2014-12-02_1458
                string guidpart = Report.ID.ToString("N"); //00000000000000000000000000000000
                string fileName = "Xteq5_Report_" + datepart + "_" + guidpart + fileExtension;
                
                fullFilename = PathExtension.Combine(tempPath, fileName);
            }

            File.WriteAllText(fullFilename, source);

            return fullFilename;

        }
    }
}
