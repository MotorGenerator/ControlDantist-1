using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ControlDantist.Reports
{
    interface IReportingRegistrFindNum
    {
        DataTable FindRegistrNum(string unmReestr, string dataStart, string dataEnd);
    }
}
