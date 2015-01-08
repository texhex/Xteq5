using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    /// <summary>
    /// Represents how to display the result of an item (Test or Asset) to a user. 
    /// For an assets we have the conclusion, data and description, while for a test we have only conclusion and description. Description might be empty if the script does not return anything for .Text.
    /// Therefore it's not trivial what to display as important information (Primary) and less important (Secondary) information to a user.
    /// This class tries to solve this. 
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
                    Primary = ConclusionEnumConverter.ConclusionHumanized(Conclusion);
                    if (Record is AssetRecord)
                    {
                        //Assign to secondary either the default conclusion description or the one from the script
                        Secondary = string.IsNullOrWhiteSpace(description) ? ConclusionEnumConverter.AssetRecordConclusionDescription(Conclusion) : description;
                    }
                    else
                    {
                        Secondary = string.IsNullOrWhiteSpace(description) ? ConclusionEnumConverter.TestRecordConclusionDescription(Conclusion) : description;
                    }

                    break;


                //For these conclusions, the text will be promoted to be Primary, and our internal description for the conclusion is secondary.
                //The exception for this rule is if we have text, then the value from the script will be used
                case ConclusionEnum.Inconclusive:
                case ConclusionEnum.Major:
                case ConclusionEnum.Minor:
                    if (Record is AssetRecord)
                    {
                        Primary = string.IsNullOrEmpty(description) ? ConclusionEnumConverter.ConclusionHumanized(Conclusion) : description;
                        Secondary = ConclusionEnumConverter.AssetRecordConclusionDescription(Conclusion);
                    }
                    else
                    {
                        Primary = string.IsNullOrEmpty(description) ? ConclusionEnumConverter.ConclusionHumanized(Conclusion) : description;
                        Secondary = ConclusionEnumConverter.TestRecordConclusionDescription(Conclusion);

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
