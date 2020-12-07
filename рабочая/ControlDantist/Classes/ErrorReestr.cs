using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// ’ранит список ошибок дл€ конкретного льготника
    /// </summary>
    public class ErrorReestr
    {
        private string фио;
        private List<ErrorsReestrUnload> listError;

        private decimal сумма»того—тоимость”слуг;
        private decimal итого—тоимость”слуг;

        /// <summary>
        /// ѕравильна€ стоимость услуг дл€ конкретного пользовател€
        /// </summary>
        public decimal —умма»того—тоимость”слуг
        {
            get
            {
                return итого—тоимость”слуг;
            }
            set
            {
                итого—тоимость”слуг = value;
            }
        }

        /// <summary>
        /// ”казывает что данный договор уже оплачивалс€.
        /// </summary>
        public string јкт¬ыполненных–абот { get; set; }
        

        /// <summary>
        /// ќшибка всего по услугам дл€ конкретного льготника
        /// </summary>
        public decimal Error—умма»того—тоимость”слуг
        {
            get
            {
                return сумма»того—тоимость”слуг;
            }
            set
            {
                сумма»того—тоимость”слуг = value;
            }
        }

        /// <summary>
        /// ’ранит ‘»ќ льготника
        /// </summary>
        public string ‘»ќ
        {
            get
            {
                return фио;
            }
            set
            {
                фио = value;
            }
        }

        /// <summary>
        /// ’ранит список ошибок в услугах дл€ конкретного льготника
        /// </summary>
        public List<ErrorsReestrUnload> ErrorList”слуги
        {
            get
            {
                return listError;
            }
            set
            {
                listError = value;
            }
        }

    }
}
