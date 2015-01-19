using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    /// <summary>
    /// Statiscs (Count records, successful record etc.) for a report
    /// </summary>
    public class RecordsStatistics
    {
        private RecordsStatistics()
        {
        }

        /// <summary>
        /// Total number of records
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Number of records that are ConclusionEnum.DoesNotApply
        /// </summary>
        public int DoesNotApplyCount { get; private set; }

        /// <summary>
        /// Number of records that are ConclusionEnum.Fatal
        /// </summary>
        public int FatalCount { get; private set; }

        /// <summary>
        /// Number of records that are ConclusionEnum.Inconclusive
        /// </summary>
        public int InconclusiveCount { get; private set; }

        /// <summary>
        /// Number of records that are ConclusionEnum.Major
        /// </summary>
        public int MajorCount { get; private set; }

        /// <summary>
        /// Number of records that are ConclusionEnum.Minor
        /// </summary>
        public int MinorCount { get; private set; }

        /// <summary>
        /// Number of records that are ConclusionEnum.Success
        /// </summary>
        public int SuccessCount { get; private set; }

        
        internal RecordsStatistics(List<BaseRecord> records)
        {

            foreach (BaseRecord rec in records)
            {
                Total++;

                switch (rec.Conclusion)
                {
                    case ConclusionEnum.DoesNotApply:
                        DoesNotApplyCount++;
                        break;

                    case ConclusionEnum.Fatal:
                        FatalCount++;
                        break;

                    case ConclusionEnum.Inconclusive:
                        InconclusiveCount++;
                        break;

                    case ConclusionEnum.Major:
                        MajorCount++;
                        break;
                    
                    case ConclusionEnum.Minor:
                        MinorCount++;
                        break;

                    case ConclusionEnum.Success:
                        SuccessCount++;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(string.Format("Found unknown ConclusionEnum: {0}", rec.Conclusion.ToString()));

                }

            }



        }
    }
}
