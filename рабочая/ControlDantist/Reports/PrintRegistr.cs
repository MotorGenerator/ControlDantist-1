using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Reports
{
    public class PrintRegistr
    {
        private IReportRegistr findNumRegistr;

        public PrintRegistr(ReportRegistr report)
        {
            findNumRegistr = report.reportRegistr;
        }


    }
}
