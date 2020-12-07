using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Хранит результат проверки льготника в ЭСРН
    /// </summary>
    public class ЭСРНvalidate
    {
        private string фамилия;
        private string имя;
        private string отчество;
        private string серияДокумента;
        private string номерДокумента;
        private string названиеДокумента;
        private string датаРождения;
        private string адрес;
        private string датаВыдачиДокумента;

        /// <summary>
        /// Фамилия льготника
        /// </summary>
        public string Фамилия
        {
            get
            {
                return фамилия;
            }
            set
            {
                фамилия = value;
            }
        }

        /// <summary>
        /// Имя льгтника
        /// </summary>
        public string Имя
        {
            get
            {
                return имя;
            }
            set
            {
                имя = value;
            }
        }

        /// <summary>
        /// Отчество льготника
        /// </summary>
        public string Отчество
        {
            get
            {
                return отчество;
            }
            set
            {
                отчество = value;
            }
        }

        /// <summary>
        /// Серия документа по которому предоставлен льгота
        /// </summary>
        public string СерияДокумента
        {
            get
            {
                return серияДокумента;
            }
            set
            {
                серияДокумента = value;
            }
        }

        /// <summary>
        /// Номер документа по которому предоставлена льгота
        /// </summary>
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

        /// <summary>
        /// Название документа на основании которого предоставлена льгота
        /// </summary>
        public string НазваниеДокумента
        {
            get
            {
                return названиеДокумента;
            }
            set
            {
                названиеДокумента = value;
            }
        }

        /// <summary>
        /// Дата рождения льготника
        /// </summary>
        public string ДатаРождения
        {
            get
            {
                return датаРождения;
            }
            set
            {
                датаРождения = value;
            }
        }

        /// <summary>
        /// Адрес льготника
        /// </summary>
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

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public string ДатаВыдачиДокумента
        {
            get
            {
                return датаВыдачиДокумента;
            }
            set
            {
                датаВыдачиДокумента = value;
            }
        }
    }
}
