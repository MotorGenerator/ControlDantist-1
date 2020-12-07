using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    ///  Вспомогательный класс описывающий сумму акта выполенных работ и дату подписания акта выполненных работ.
    /// </summary>
    public class ItemLetSumDataAct
    {
        /// <summary>
        /// Сумма акта выполненных работ.
        /// </summary>
        public decimal SummAct { get; set; }

        /// <summary>
        /// Дата подписания акта выполненных работ.
        /// </summary>
        public string DateAct { get; set; }
    }
}
