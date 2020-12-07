using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Хранит данные по услугам
    /// </summary>
    class DateService
    {
        private string наименованиеУслуги;
        //private decimal цена;
        //private decimal сумма;
        private string цена;
        private string сумма;
        private string количество;

        /// <summary>
        /// Количество
        /// </summary>
        public string Количество
        {
            get
            {
                return количество;
            }
            set
            {
                количество = value;
            }
        }

        /// <summary>
        /// Хранит наименование
        /// </summary>
        public string Наименование
        {
            get
            {
                return наименованиеУслуги;
            }
            set
            {
                наименованиеУслуги = value;
            }
        }

        /// <summary>
        /// Хранит стоимость услуги
        /// </summary>
        //public decimal Цена
        public string Цена
        {
            get
            {
                return цена;
            }
            set
            {
                цена = value;
            }
        }

        /// <summary>
        /// Хранит сумму услуги
        /// </summary>
        //public decimal Сумма
        public string Сумма
        {
            get
            {
                return сумма;
            }
            set
            {
                сумма = value;
            }
        }
    }
}
