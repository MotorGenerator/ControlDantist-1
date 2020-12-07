using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Вспомогательный класс содержащий данные для не оплаченных договоров.
    /// </summary>
    public class СуммаДоговоровБезАктов
    {
        public string НаименованиеПоликлинники { get; set; }
        public decimal ВетераныТрудаСумма { get;set;}
        public decimal ВетераныВоеннойСлужбыСумма { get; set; }
        public decimal ВетераныТрудаСаратовскойОбластиСумма { get; set; }
        public decimal ТруженникиТылаСумма { get; set; }
        public decimal РеабелитированныеСумма { get; set; }
        public decimal ВсегоСумма { get; set; }


    }
}
