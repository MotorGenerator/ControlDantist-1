using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Строка таблицы ВидУслуг.
    /// </summary>
    public class ВидУслуг
    {
        public int Id_ВидУслуг { get; set; }
        public string ВидУслуги1 { get; set; }
        public decimal Цена { get; set; }
        public int Id_Поликлинники { get; set; }
        public string НомерПоПеречню { get; set; }
        public bool Выбрать { get; set; }
        public int Id_КодУслуги { get; set; }
        public string ТехЛист { get; set; }
    }
}
