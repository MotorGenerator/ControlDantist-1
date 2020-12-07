using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class ValideContract
    {
        public string id_договор { get; set; }

        public int IdContract { get; set; }

        public string НомерДоговора { get; set; }

        public string Фамилия { get; set; }

        public string Имя { get; set; }

        public string Отчество { get; set; }

        public string ЛьготнаяКатегория { get; set; }

        public string Сумма { get; set; }

        public string Дата { get; set; }

        public string КтоЗаписал { get; set; }

        public string НомерАкта { get; set; }

        public string ДатаПодписания { get; set; }

        public bool flag2019AddWrite { get; set; }

        public bool flagАнулирован { get; set; }

        public int Год { get; set; }
    }

}
