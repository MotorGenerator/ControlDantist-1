using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    class СуммаКоличествоДоговоров: СуммаДоговоровБезАктов
    {
        //public string НаименованиеПоликлинники { get; set; }
        public int ВетераныТрудаКоличество { get; set; }
        public int ВетераныВоеннойСлужбыКОличество { get; set; }
        public int ВетераныТрудаСаратовскойОбластиКОличество { get; set; }
        public int ТруженникиТылаКоличество { get; set; }
        public int РеабелитированныеКоличество { get; set; }
        public int ВсегоКоличество { get; set; }
    }
}
