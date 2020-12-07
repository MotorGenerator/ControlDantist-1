using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    //—одержит строку реестра с ошибкой
    class ReestrErrorControl
    {
        private string номер;
        private string фио;
        private string наименование”слуги;
        private string ценаError;
        private string ценаControl;
        private string сумаError;
        private string суммаControl;
        private string стоимость”слугиError;
        private string стоимость”слугиControl;


        /// <summary>
        /// ’ранит ‘»ќ льготника
        /// </summary>
        public string Ќомерѕор€дковый
        {
            get
            {
                return номер;
            }
            set
            {
                номер = value;
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
        /// ’ранит наименование услуги
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
        /// ’ранит значение стоимости услуги с ошибкой
        /// </summary>
        public string ÷енаError
        {
            get
            {
                return ценаError;
            }
            set
            {
                ценаError = value;
            }
        }

        /// <summary>
        /// ’ранит контрольное значение стоимости услуги
        /// </summary>
        public string ÷енаControl
        {
            get
            {
                return ценаControl;
            }
            set
            {
                ценаControl = value;
            }
        }
        
        //’ранит сумму услуги с ошибкой
        public string —умаError
        {
            get
            {
                return сумаError;
            }
            set
            {
                сумаError = value;
            }
        }

        //’ранит сумму услуги контрольную
        public string —уммаControl
        {
            get
            {
                return суммаControl;
            }
            set
            {
                суммаControl = value;
            }
        }

        //’ранит значение услуги с ошибкой
        public string —тоимость”слугиError
        {
            get
            {
                return стоимость”слугиError;
            }
            set
            {
                стоимость”слугиError = value;
            }
        }
       
        //’ранит контрольное значение —тоимости оказанных услуг
        public string —тоимость”слугиControl
        {
            get
            {
                return стоимость”слугиControl;
            }
            set
            {
                стоимость”слугиControl = value;
            }
        }

    }
}
