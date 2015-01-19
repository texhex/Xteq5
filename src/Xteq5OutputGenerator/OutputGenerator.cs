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
        /// <param name="report">Report to generate output for</param>
        /// <param name="format">Format that should be generated</param>
        /// <returns>A string representation (source) of the requested output</returns>
        public static string GenerateReportOutput(Report report, OutputFormatEnum format)
        {
            switch (format)
            {
                case OutputFormatEnum.HTML:
                    string htmlTemplatePath = PathExtension.Combine(report.CompilationFolder, "BootstrapTemplate.html");
                    BootstrapHTMLGenerator htmlGenerator = new BootstrapHTMLGenerator();                    
                    return htmlGenerator.Generate(report, htmlTemplatePath);

                case OutputFormatEnum.XML:
                    string xmlTemplate = PathExtension.Combine(report.CompilationFolder, "XMLtemplate.xml");
                    XMLGenerator xmlGenerator = new XMLGenerator();
                    return xmlGenerator.Generate(report, xmlTemplate);

                case OutputFormatEnum.JSON:
                    //JSONGenerator does not use a template
                    JSONGenerator jsonGenerator = new JSONGenerator();
                    return jsonGenerator.Generate(report);

                default:
                    throw new ArgumentException(string.Format("OutputFormat {0} is not supported", format.ToString()));
            }
             
        }

        /// <summary>
        /// Generates a report in the requested format, based on the given Report object and save it to a file.
        /// If Filename is empty, a filename in the TEMP folder of the current user will be generated
        /// </summary>
        /// <param name="report">Report to generate output for</param>
        /// <param name="format">Format that should be generated</param>
        /// <param name="format">Filepath to store the result. If empty, a filename will be generated</param>
        /// <returns>Filepath of the generated and saved output</returns>
        public static string GenerateReportOutputFile(Report report, OutputFormatEnum format, string filePath)
        {
            string fileExtension = "";

            switch (format)
            {
                case OutputFormatEnum.HTML:
                    fileExtension=".html";
                    break;

                case OutputFormatEnum.XML:
                    fileExtension = ".xml";
                    break;

                case OutputFormatEnum.JSON:
                    fileExtension = ".json";
                    break;

                default:
                    throw new ArgumentException(string.Format("OutputFormat {0} is not supported", format.ToString()));
            }

            string source = GenerateReportOutput(report, format);

            string fullFilename = filePath;
            if (string.IsNullOrWhiteSpace(fullFilename))
            {
                                
                string datepart = string.Format("{0:yyyy-MM-dd_HHmm}", report.EndedUTC); //2014-12-31_1458
                string guidpart = report.ID.ToString("N"); //00000000000000000000000000000000
                string fileName = "Xteq5_Report_" + datepart + "_" + guidpart + fileExtension;

                string tempPath = Path.GetTempPath();
                fullFilename = PathExtension.Combine(tempPath, fileName);
            }

            File.WriteAllText(fullFilename, source);

            return fullFilename;

        }
    }
}
