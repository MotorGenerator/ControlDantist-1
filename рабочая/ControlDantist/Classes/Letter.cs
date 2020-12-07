using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Хранит письмо обоплате реестра
    /// </summary>
    public class Letter
    {
       
        private string комитет;
        private string начальник;
        private string обращение;
        private string фио;
        private string адрес;
        private string номерДокумента;
        private string суммаАкта;

        /// <summary>
        /// Хранит id акта
        /// </summary>
        public int IdАкт { get; set; }

        public string CуммаАкта
        {
            get
            {
                return суммаАкта;
            }
            set
            {
                суммаАкта = value;
            }
        }

        public string НомерДокумента
        {
            get
            {
                return номерДокумента;
            }
            set
            {
                номерДокумента = value;
            }
        }

        public string Адрес
        {
            get
            {
                return адрес;
            }
            set
            {
                адрес = value;
            }
        }

        public string Фио
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
        /// Уважаемый Виктор Константинович
        /// </summary>
        public string Обращение
        {
            get
            {
                return обращение;
            }
            set
            {
                обращение = value;
            }
        }

        /// <summary>
        /// Иницылы и фамилия начальника кому отправлено письмо
        /// </summary>
        public string Начальник
        {
            get
            {
                return начальник;
            }
            set
            {
                начальник = value;
            }
        }

        /// <summary>
        /// Наименование организации кому отправлено письмо
        /// </summary>
        public string Комитет
        {
            get
            {
                return комитет;
            }
            set
            {
                комитет = value;
            }
        }
    
            
    }
}
