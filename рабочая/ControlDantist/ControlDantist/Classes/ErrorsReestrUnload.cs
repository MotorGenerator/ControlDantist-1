using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// ’ранит расхождение информации дл€ конкретонго льготника в услугах
    /// </summary>
    public class ErrorsReestrUnload
    {
        private string наименование”слуги;
        private string ошибкаЌаименование”слуги;

        private decimal цена;
        private decimal ошибка÷ена;

        private decimal сумма;
        private decimal ошибка—умма;



        /// <summary>
        /// ќшибка в сумме дл€ конкретной работы
        /// </summary>
        public decimal Error—умма
        {
            get
            {
                return ошибка—умма;
            }
            set
            {
                ошибка—умма = value;
            }
        }

        /// <summary>
        /// ’ранит правильную сумму
        /// </summary>
        public decimal —умма
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

        /// <summary>
        /// ’ранит правильную цену
        /// </summary>
        public decimal ÷ена
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
        /// ’ранит цену с ошибкой
        /// </summary>
        public decimal Error÷ена
        {
            get
            {
                return ошибка÷ена;
            }
            set
            {
                ошибка÷ена = value;
            }
        }

        /// <summary>
        /// ’ранит правильное наименование услуги
        /// </summary>
        public string Ќаименование”слуги
        {
            get
            {
                return наименование”слуги;
            }
            set
            {
                наименование”слуги = value;
            }
        }

        /// <summary>
        /// ’ранит ошибочное наименование услуги
        /// </summary>
        public string ErrorЌаименование”слуги
        {
            get
            {
                return ошибкаЌаименование”слуги;
            }
            set
            {
                ошибкаЌаименование”слуги = value;
            }
        }
    }
}
