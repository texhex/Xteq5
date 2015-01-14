using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xteq5
{
    public enum ReportAction
    {
        Starting = 1, //The report is about to be created
        Ended = 2 //The report was created
    }

    public class ReportCreationProgress
    {
        public ReportCreationProgress()
        {
            this.Action = ReportAction.Starting;
        }


        public ReportAction Action { get; internal set; }


        public override string ToString()
        {
            switch (this.Action)
            {
                case ReportAction.Starting:
                    return "Creating file...";

                case ReportAction.Ended:
                    return "File created";

                default:
                    return "Unknown action";
            }

        }

    }
}
