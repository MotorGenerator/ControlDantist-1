using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ValidPersonContract
{
    /// <summary>
    /// Договора прошедшие проверку.
    /// </summary>
    public class ValidItemsContract
    {

        /// <summary>
        /// Id договор.
        /// </summary>
        public int IdContract { get; set; }
        /// <summary>
        /// Текущий номер договора.
        /// </summary>
        public string CurrentNumContract { get; set; }

        /// <summary>
        /// Номер договора который ранее заключал льготник.
        /// </summary>
        public string NumContract { get; set; }

        /// <summary>
        /// Дата ранее заключенного договора.
        /// </summary>
        public string DateContract { get; set; }

        /// <summary>
        /// Флаг указывающий на таблицу в которой записан договор.
        /// true - ДоговорAdd, false - Договор.
        /// </summary>
        public bool flag2019Add { get; set; }

        /// <summary>
        /// Флаг проверки договора.
        /// </summary>
        public bool FlagValidateContract { get; set; }

        /// <summary>
        /// Флаг указывающий анулирован договор или нет.
        /// </summary>
        public bool flagАнулирован { get; set; }
    }
}
