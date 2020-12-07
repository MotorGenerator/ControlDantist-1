using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class ClassЗаписьБДДоговор
    {
        /// <summary>
        /// Номер реестра.
        /// </summary>
        public string НомерРеестра { get; set; }

        /// <summary>
        /// Дата реестра.
        /// </summary>
        public DateTime ДатаРеестра { get; set; }

        /// <summary>
        /// Id льготной категории.
        /// </summary>
        public int IdЛьготнойКатегории { get; set; }

        /// <summary>
        /// Номер договора.
        /// </summary>
        public string НомерДоговора { get; set; }

        /// <summary>
        /// Хранит дату договора.
        /// </summary>
        public DateTime ДатаДоговора { get; set; }

        /// <summary>
        /// Сумма акта выполненных работ.
        /// </summary>
        public decimal СуммаАкткВыполненныхРабот { get; set; }

        /// <summary>
        /// Дата акта выполненных работ.
        /// </summary>
        public DateTime ДатаАктаВыполненныхРабот { get; set; }

        /// <summary>
        /// Номер счёт фактуры.
        /// </summary>
        public string НомерСчётФактуры { get; set; }

        /// <summary>
        /// Дата счёт фактуры.
        /// </summary>
        public DateTime ДатаСчётФактуры { get; set; }

        /// <summary>
        /// Пишем лог.
        /// </summary>
        public string LogWrite { get; set; }

        /// <summary>
        /// id поликлинники.
        /// </summary>
        public int IdПоликлинники { get; set; }

        /// <summary>
        /// Флаг наличия акта.
        /// </summary>
        public bool ФлагНаличияАкта { get; set; }

    }
}
