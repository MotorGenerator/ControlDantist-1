using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ControlDantist.Reports
{
    /// <summary>
    /// Абстрактаня фаблика методов формирующих отчет.
    /// </summary>
    public abstract class AbstractFactory
    {
        //public abstract DataTable ExecuteReport(string query, bool flagLetter);
        //public abstract DataTable FindInvoiceAndNumRegistr(string num_счётФактура, string numReestr, string dataStart, string dataEnd);
        //public abstract DataTable FindNumRegistr(string unmReestr, string dataStart, string dataEnd);
        public IReportRegistr reportRegistr;
    }
}
