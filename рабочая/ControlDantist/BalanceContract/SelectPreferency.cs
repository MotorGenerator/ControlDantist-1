using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.BalanceContract
{
    /// <summary>
    /// Выбор льготной категории.
    /// </summary>
    public class SelectPreferency
    {
        /// <summary>
        /// ID льлготной категории.
        /// </summary>
        public int IdLK { get; set; }

        /// <summary>
        /// Название льготной категории.
        /// </summary>
        public string NameLK { get; set; }

        /// <summary>
        /// Выбрана или нет льготная категория.
        /// </summary>
        public bool Flag { get; set; }

    }
}
