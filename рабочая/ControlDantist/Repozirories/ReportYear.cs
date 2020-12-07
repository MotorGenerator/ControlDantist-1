using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Repozirories
{
    public class ReportYear
    {
        public string Район { get; set; }
        public string Поликлинника { get; set; }
        public Int16 ПропускнаяСпособностьГод { get; set; }
        public Int16 ОчередностьГод { get; set; }
        public Int16 КоличествоЗаключенныхДоговоров { get; set; }
        public decimal СуммаЗаключенныхДоговоров { get; set; }
        public Int16 КоличествоДоговоровВДеле { get; set; }
        public decimal СуммаДоговоровВДеле { get; set; }
        public Int16 КоличествоДоговоровПоступившихНаОплату { get; set; }
        public decimal СуммаДоговороПоступившихНаОплату { get; set; }
        public Int16 SerialNumber { get; set; }


    }
}
