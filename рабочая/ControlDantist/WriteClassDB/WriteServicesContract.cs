using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist;

namespace ControlDantist.WriteClassDB
{
    public class WriteServicesContract : IServicesContract
    {
        public string НаименованиеУслуги { get; set; }
        public decimal цена { get; set; }
        public int Количество { get; set; }
        public int id_договор { get; set; }
        public string НомерПоПеречню { get; set; }
        public decimal Сумма { get; set; }
        public string ТехЛист { get; set; }
    }

       
}
