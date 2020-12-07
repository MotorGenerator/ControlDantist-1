using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Класс содержит лог проверки услуг по наименованиям и цене
    /// </summary>
    class Log
    {
        private string наименование;
        private string цена;

        private string наименованиеФайл;
        private string ценаФайл;

        private string наименованиеError;
        private string ценаError;

        /// <summary>
        /// Наименование на SQL сервер
        /// </summary>
        public string НаименованиеSQL
        {
            get
            {
                return наименование;
            }
            set
            {
                наименование = value;
            }
        }

        /// <summary>
        /// Цена на SQL сервер
        /// </summary>
        public string ЦенаSQL
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
        /// Наименование услуги в файле выгрузки
        /// </summary>
        public string НаименованиеФайл
        {
            get
            {
                return наименованиеФайл;
            }
            set
            {
                наименованиеФайл = value;
            }
        }


        /// <summary>
        /// Цена услуги в файле выгрузки
        /// </summary>
        public string ЦенаФайл
        {
            get
            {
                return ценаФайл;
            }
            set
            {
                ценаФайл = value;
            }
        }

        /// <summary>
        /// Наименование услуги в файле выгрузки 
        /// </summary>
        public string НаименованиеError
        {
            get
            {
                return наименованиеError;
            }
            set
            {
                наименованиеError = value;
            }
        }

        /// <summary>
        /// Цена содержащаяся в файле выгрузки
        /// </summary>
        public string ЦенаError
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
    }
}
