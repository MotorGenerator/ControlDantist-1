using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ControlDantist.Reports
{
    public interface IReportRegistr
    {
        DataTable GetDataTable(string query, bool flagLetter);
    }
}
