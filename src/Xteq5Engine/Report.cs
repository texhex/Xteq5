using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    /// <summary>
    /// Represents the result of Xteq5Runner.Run()
    /// </summary>
    public class Report
    {
        internal Report()
        {
            ID = Guid.NewGuid();
            StartedUTC = DateTime.UtcNow;
        }

        /// <summary>
        /// A unique value (GUID)
        /// </summary>
        public Guid ID { get; private set; }

        /// <summary>
        /// Start time of the report 
        /// </summary>
        public DateTime StartedUTC { get; private set; }

        /// <summary>
        /// End time of the report
        /// </summary>
        public DateTime EndedUTC { get; private set; }

        /// <summary>
        /// Runtime of the report in seconds
        /// </summary>
        public string RuntimeSeconds { get; private set; }

        /// <summary>
        /// True if the report is complete
        /// </summary>
        public bool IsFinished { get; private set; }


        internal void Finish() {
            if (IsFinished)
                throw new InvalidOperationException("Report has already finished");

            EndedUTC = DateTime.UtcNow;

            TimeSpan ts = EndedUTC - StartedUTC;                       
            RuntimeSeconds = ts.TotalSeconds.ToString("0");
            
            //Runtime += " seconds";
            
            IsFinished = true;            
        }

        /// <summary>
        /// Assets in this report
        /// </summary>
        public List<AssetRecord> Assets { get; internal set; }

        /// <summary>
        /// Tests in this report
        /// </summary>
        public List<TestRecord> Tests { get; internal set; }

        /// <summary>
        /// Statistics for all assets
        /// </summary>
        public RecordsStatistics AssetStatiscs { get; internal set; }

        /// <summary>
        /// Statistics for all tests
        /// </summary>
        public RecordsStatistics TestStatiscs { get; internal set; }

        /// <summary>
        /// Username that generated the report
        /// </summary>
        public string UserName { get; internal set; }

        /// <summary>
        /// Computername that was used to generate the report
        /// </summary>
        public string ComputerName { get; internal set; }

        /// <summary>
        /// Version of Xteq5 that generated this report
        /// </summary>
        public Version Xteq5Version { get; internal set; }

        /// <summary>
        /// Additonal text provided by the user to distinguish this report
        /// </summary>
        public string UserText { get; set; }

        /// <summary>
        /// Source folder where scripts were loaded from
        /// </summary>
        public string SourceFolder { get; internal set; }
    }
}
