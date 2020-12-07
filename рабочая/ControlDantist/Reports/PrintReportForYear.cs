using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Reports;
using ControlDantist.Repozirories;

namespace ControlDantist.Reports
{
    public class PrintReportForYear : PrintReport
    {
        public override void Print(List<ReportYear> listDate)
        {
            // Передадим данные на печать.
            ReportInformToYear report = new ReportInformToYear();

            report.Print(listDate);
        }
    }
}
