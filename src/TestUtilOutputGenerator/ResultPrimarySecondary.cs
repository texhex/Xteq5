using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtil
{
    /// <summary>
    /// Represents primary and secondary result for an item. This differs for assets and tests
    /// </summary>
    internal class ResultPrimarySecondary
    {
        private ResultPrimarySecondary()
        {

        }

        /// <summary>
        /// Initalize this class with a record with recommended values for a BaseRecord
        /// </summary>
        /// <param name="Record"></param>
        internal ResultPrimarySecondary(BaseRecord Record)
        {
            //Short-circut some values
            Conclusion = Record.Conclusion;
            string description = string.IsNullOrWhiteSpace(Record.Description) ? string.Empty : Record.Description;
                   
            switch (Conclusion)
            {
                //The primary result for these conclusion is always our pre-set value and (if available) the description from the script as secondary 
                case ConclusionEnum.Success:
                case ConclusionEnum.DoesNotApply:
                case ConclusionEnum.Fatal:
                    Primary = ConclusionEnumConverter.ConclusionHumanString(Conclusion);
                    if (Record is AssetRecord)
                    {
                        //Assign to secondary either the default conclusion description or the one from the script
                        Secondary = string.IsNullOrWhiteSpace(description) ? ConclusionEnumConverter.AssetRecordConclusionDescriptionHumanString(Conclusion) : description;
                    }
                    else
                    {
                        Secondary = string.IsNullOrWhiteSpace(description) ? ConclusionEnumConverter.TestRecordConclusionDescriptionHumanString(Conclusion) : description;
                    }

                    break;


                //For these conclusions, the text will be promoted to be Primary, and our internal description for the conclusion is secondary
                //The exception for this rule is if we have text, then the internal value will be used
                case ConclusionEnum.Inconclusive:
                case ConclusionEnum.Major:
                case ConclusionEnum.Minor:
                    if (Record is AssetRecord)
                    {
                        Primary = string.IsNullOrEmpty(description) ? ConclusionEnumConverter.ConclusionHumanString(Conclusion) : description;
                        Secondary = ConclusionEnumConverter.AssetRecordConclusionDescriptionHumanString(Conclusion);
                    }
                    else
                    {
                        Primary = string.IsNullOrEmpty(description) ? ConclusionEnumConverter.ConclusionHumanString(Conclusion) : description;
                        Secondary = ConclusionEnumConverter.TestRecordConclusionDescriptionHumanString(Conclusion);

                    }
                    break;


                default:
                    throw new ArgumentOutOfRangeException(string.Format("Found unknown ConclusionEnum: {0}", Conclusion.ToString()));
            }



        }

        /// <summary>
        /// Caches the Conclusion from the BaseRecord that was created to initialize this class
        /// </summary>
        internal ConclusionEnum Conclusion { get; set; }

        /// <summary>
        /// Primary (important) result
        /// </summary>
        internal string Primary { get; set; }

        /// <summary>
        /// Secondary (less important) result 
        /// </summary>
        internal string Secondary { get; set; }
    }
}
