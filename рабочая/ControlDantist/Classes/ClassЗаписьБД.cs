using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Класс для записи в БД данных из реестров.
    /// </summary>
    public class ClassЗаписьБД
    {
        //public ClassЗаписьБДДоговор Договор { get; set; }
        public ClassЗаписьБДЛьготник Льготник { get; set; }
        public ClassЗаписьСтоимостьУслуг СтоимостьУслуг { get; set; }
    }
}
