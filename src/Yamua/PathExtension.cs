using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamua
{
    /// <summary>
    /// Unfortunately, since System.IO.Path is a static class, it does not allow extension methods. Therfore this class exists.
    /// For details see [Can I add extension methods to an existing static class](http://stackoverflow.com/questions/249222/can-i-add-extension-methods-to-an-existing-static-class)
    /// </summary>
    public static class PathExtension
    {
        /// <summary>
        /// Checks if a given string is valid, this means it does not include any chars that can not be used for a filename 
        /// </summary>
        /// <param name="fileName">A filename to be checked</param>
        /// <returns>TRUE if the Filename should be OK, FALSE otherwise</returns>
        public static bool IsFilenameValid(string fileName)
        {
            bool returnvalue = false;

            //First check if this maybe a full path (C:\Testing123\MyFile.ext)
            try
            {
                string filename_only = Path.GetFileName(fileName);
                
                //Original code from http://stackoverflow.com/a/4650495/612954 by [Phil Hunt](http://stackoverflow.com/users/492543/phil-hunt)
                returnvalue = !string.IsNullOrWhiteSpace(filename_only) && filename_only.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
            }
            finally
            {
            }

            return returnvalue;
        }

        //TODO: Also provide a IsFullPathValud function - use GetInvalidPathChars? 


        /// <summary>
        /// Identical to Path.GetFullPath()
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string FullPath(string fileOrDirectoryPath)
        {
            return Path.GetFullPath(fileOrDirectoryPath);
        }

        /// <summary>
        /// Identifical to Path.GetFileName()
        /// </summary>
        /// <param name="filePath">Full path to a file</param>
        /// <returns>The filename, including extension, from the given path</returns>
        public static string Filename(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <summary>
        /// Identical to Path.Combine(string1, string2)
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string Combine(string directoryPath, string fileName)
        {
            return Path.Combine(directoryPath, fileName);
        }

        /// <summary>
        /// Identical to Directory.Exists(path)
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// Identical to File.Exists(path)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
