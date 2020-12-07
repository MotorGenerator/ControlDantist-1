using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Записывает данные в таблицу Услуги по договору (Загрузка данных первой половины 2013 г.).
    /// </summary>
    public class ClassЗаписьСтоимостьУслуг
    {
        /// <summary>
        /// Хранит название что услуги записывались при записи данных (первая половина 2013 г).
        /// </summary>
        public string НаименованиеУслуги 
        {
            get
            {
                return "Запись 2013 год";
            }
            //set
            //{
                
            //}
        }

        /// <summary>
        /// Хранит id договор.
        /// </summary>
        public int IdДоговор { get; set; }

        /// <summary>
        /// Хранит сумму договора.
        /// </summary>
        public decimal Сумма { get; set; }


    }
}
