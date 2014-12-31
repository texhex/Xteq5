using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtil
{
    public enum ConclusionEnum
    {
        //The test did not execute correctly and was shut down.
        //Recommended action: Check the script for code errors
        Fatal = -3,

        //A major issue was found
        //Recommended action: Investigate and fix the issue immediately.
        Major = -2,

        //A little irregularity was found.
        //Recommended action: Investigate and fix the issue when time permits. 
        Minor = -1,

        //The test does not apply to this system. 
        //Recommended action: None
        DoesNotApply = 0,
        //not applicable, not available

        //No issue was found, your system is operating within established parameters.
        //Recommended action: None 
        Success = 1,

        //The test did not have enough data to conclude a result. 
        //Recommended action: None
        Inconclusive = 2
    }

    public static class ConclusionEnumConverter
    {

        /// <summary>
        /// Tries to parse a conclusion as string. Returns FATAL if the string could not be undestood
        /// </summary>
        /// <param name="ConclusionString">ConclusionEnum as string</param>
        /// <returns>A ConclusionEnum</returns>
        public static ConclusionEnum ParseConclusion(string ConclusionString)
        {
            string dataLowerCase = ConclusionString.ToLower(CultureInfo.InvariantCulture);

            switch (dataLowerCase)
            {
                case "major":
                case "fail":
                    {
                        return ConclusionEnum.Major;
                    }

                case "minor":
                    {
                        return ConclusionEnum.Minor;
                    }

                case "doesnotapply":
                case "n/a":
                    {
                        return ConclusionEnum.DoesNotApply;
                    }

                case "success":
                case "ok":
                    {
                        return ConclusionEnum.Success;
                    }

                case "inconclusive":
                    {
                        return ConclusionEnum.Inconclusive;
                    }

                default:
                    {
                        return ConclusionEnum.Fatal;
                    }
            }
        }

        /// <summary>
        /// Converts a ConclusioEnum to a human friendly string
        /// </summary>
        /// <param name="Conclusion">Conclusion to retrieve a humanized string for</param>
        /// <returns>The humanized string</returns>
        public static string ConclusionHumanString(ConclusionEnum Conclusion)
        {
            switch (Conclusion)
            {
                case ConclusionEnum.Success:
                    return "OK (Success)";

                case ConclusionEnum.DoesNotApply:
                    return "N/A (Does not apply)";

                case ConclusionEnum.Fatal:
                    return "Crashed (Fatal)";

                case ConclusionEnum.Inconclusive:
                    return "Inconclusive";

                case ConclusionEnum.Major:
                    return "Major";

                case ConclusionEnum.Minor:
                    return "Minor";

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Found unknown ConclusionEnum: {0}", Conclusion.ToString()));
            }
        }

        /// <summary>
        /// Returns a sentence that describes the conclusion for a TestRecord.
        /// </summary>
        /// <param name="Conclusion">Conclusion to return text for</param>
        /// <returns>A sentence describing the conclusiong</returns>
        public static string TestRecordConclusionDescriptionHumanString(ConclusionEnum Conclusion)
        {
            switch (Conclusion)
            {
                case ConclusionEnum.Success:
                    //Alternative: The test found no issues, your system is operating within established parameters
                    return "The test found no issues"; 

                case ConclusionEnum.DoesNotApply:
                    return "The test does not apply to this computer and can safely be ignored";

                case ConclusionEnum.Fatal:
                    return "The test script failed to execute correctly and was shut down";

                case ConclusionEnum.Inconclusive:
                    //Alternative: Not enough data to concluse a result
                    return "The test did not find enough data to conclude a result"; 

                case ConclusionEnum.Major:
                    //Alternative: An major issue was found
                    return "The test found a major issue"; 

                case ConclusionEnum.Minor:
                    //Alternative: A little irregularity was found
                    return "The test found a little irregularity"; 

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Found unknown ConclusionEnum: {0}", Conclusion.ToString()));
            }
        }

        /// <summary>
        /// Returns a recommended action for the user, based on the conclusion
        /// </summary>
        /// <param name="Conclusion">Conclusion to get a recommended action for</param>
        /// <returns>An empty string if no recommended action exist, or the recommended action</returns>
        public static string TestRecordConclusionRecommendedActionHumanString(ConclusionEnum Conclusion)
        {
            switch (Conclusion)
            {
                case ConclusionEnum.Success:
                case ConclusionEnum.DoesNotApply:
                case ConclusionEnum.Inconclusive:
                case ConclusionEnum.Fatal: //The user can't do anything about a fatal test
                    return "";

                case ConclusionEnum.Major:
                    return "Investigate and fix the issue immediately";

                case ConclusionEnum.Minor:
                    return "Investigate and fix the issue when time permits";

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Found unknown ConclusionEnum: {0}", Conclusion.ToString()));
            }
        }

        /// <summary>
        /// Returns a sentence that describes the conclusion for an AssetRecord.
        /// </summary>
        /// <param name="Conclusion">Conclusion to return a sentence for</param>
        /// <returns>A sentence describing the conclusion</returns>
        public static string AssetRecordConclusionDescriptionHumanString(ConclusionEnum Conclusion)
        {
            switch (Conclusion)
            {
                case ConclusionEnum.Success:
                    return "The asset successfully retrieved information"; //Different from TestRecord

                case ConclusionEnum.DoesNotApply:
                    return "The asset does not apply to this computer and can safely be ignored";

                case ConclusionEnum.Fatal:
                    return "The asset did not execute correctly and was shut down";

                //These cases are invalid for assets and will therefore result in an exception
                case ConclusionEnum.Inconclusive:                
                case ConclusionEnum.Major:
                case ConclusionEnum.Minor:
                    throw new ArgumentException(string.Format("ConclusionEnum {0} is not valid for an AssetRecord", Conclusion.ToString()));

                default:
                    throw new ArgumentOutOfRangeException(string.Format("Found unknown ConclusionEnum: {0}", Conclusion.ToString()));
            }
        }

    }
}
