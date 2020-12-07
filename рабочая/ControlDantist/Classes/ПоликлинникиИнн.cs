using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Класс описывающий строку в таблице наименование поликлинники.
    /// </summary>
    public class ПоликлинникиИнн
    {
        public int ID { get; set; }
        public string Наименование { get; set; }
        public string ИНН { get; set; }
    }
}
