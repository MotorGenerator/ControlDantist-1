using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Класс содержит данные для льготника Загрузка данных первой половины 2013 г.
    /// </summary>
    public class ClassЗаписьБДЛьготник
    {
        /// <summary>
        /// Фамилия.
        /// </summary>
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }

        /// <summary>
        /// Серия документа на право получения льготы.
        /// </summary>
        public string СерияДокумента { get; set; }

        /// <summary>
        /// Номер документа на право получения льготы.
        /// </summary>
        public string НомерДокумента { get; set; }

        /// <summary>
        /// Дата получения документа.
        /// </summary>
        public DateTime ДатаПолученияДокумента { get; set; }

        public string НомерАкта { get; set; }

        /// <summary>
        /// Id льготной категории.
        /// </summary>
        public int IdЛьготнойКатегории { get; set; }

    }
}
