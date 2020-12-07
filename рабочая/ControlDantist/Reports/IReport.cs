using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Repozirories;

namespace ControlDantist.Reports
{
    interface IReport
    {
        void Print(List<ReportYear> listData);
    }
}
