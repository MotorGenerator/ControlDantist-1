using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Льготник получатель мер соц поддержки.
    /// </summary>
    public class PersonRecipient
    {
        public string РайонОбласти { get; set; }
        public string НаселённыйПункт { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public DateTime ДатаРождения { get; set; }
        public string Улица { get; set; }
        public string НомерДома { get; set; }
        public string Корпус { get; set; }
        public string НомерКвартиры { get; set; }
        public string СерияДокумента { get; set; }
        public string НомерДокумента { get; set; }
        public string ЛьготнаяКатегория { get; set; }
        public string СуммаВыполненныхРабот { get; set; }

    }
}
