using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.BalanceContract
{
    /// <summary>
    /// Строка отчета баланса.
    /// </summary>
    public class ReportBalance : IReportBalance
    {
        public string ЛьготнаяКатегория {get; set;}


        public int КоличествоДоговоров { get; set; }


        public decimal СуммаДоговоров { get; set; }
        
    }
}
