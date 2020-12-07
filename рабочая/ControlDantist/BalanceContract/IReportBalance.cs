using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.BalanceContract
{
    interface IReportBalance
    {
        string ЛьготнаяКатегория { get; set; }
        int КоличествоДоговоров { get; set; }
        decimal СуммаДоговоров { get; set; }
    }
}
