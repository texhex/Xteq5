using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public class ReportCreationProgress
    {
        public ReportCreationProgress()
        {
            Starting = false;
            Ended = false;
        }

        /// <summary>
        /// TRUE when the report is about to be created
        /// </summary>
        public bool Starting { get; internal set; }

        /// <summary>
        /// TRUE when the report was created
        /// </summary>
        public bool Ended { get; internal set; }
    }
}
