using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Reports
{
    /// <summary>
    /// Реализуем абстрактную фабрику.
    /// </summary>
    public class ReportRegistr : AbstractFactory
    {
        /// <summary>
        /// Реализуем отчет.
        /// </summary>
        
        public void Print()
        {
            IReportRegistr reportRegistr = new FindReestrInvoce();
            //reportRegistr.GetDataTable(


        }
    }
}
