using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ControlDantist.Reports
{
    interface IReportingRegistrInvoiceAndNumRegistr
    {
        DataTable FindInvoiceAndNumRegistr(string num_счётФактура, string numReestr, string dataStart, string dataEnd);
    }
}
