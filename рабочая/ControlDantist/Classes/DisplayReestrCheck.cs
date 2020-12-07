using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// класс содержит результаты проверок выгрузки с актами
    /// </summary>
    class DisplayReestrCheck
    {
        private string номерƒоговора;
        private string датаƒоговора;
        private string датајкта;
        private string льготник;
        private string номер;
        List<ErrorsReestrUnload> errorList;
        private bool flagError;
        //private decimal sum;
        private string sum;
        //private decimal sumAct;
        private string sumAct;

        private bool flagAddContract;

        /// <summary>
        /// —одержит номер ранее оплаченного акта.
        /// </summary>
        public string ќплаченный–анееƒоговорјкт { get; set; }

        /// <summary>
        /// ”казывает что договор уже оплачивалс€.
        /// </summary>
        public bool FlagErrorActќплата { get; set; }

       /// <summary>
        /// ”казывает что есть доп соглашение
        /// </summary>
        public bool FlagAddContract
        {
            get
            {
                return flagAddContract;
            }
            set
            {
                flagAddContract = value;
            }
        }

        /// <summary>
        /// ’ранит сумму акта выполненных работ
        /// </summary>
        public string —уммајкта
        {
            get
            {
                return sumAct;
            }
            set
            {
                sumAct = value;
            }
        }

        /// <summary>
        /// ’ранит дату акта
        /// </summary>
        public string ƒатајкта
        {
            get
            {
                return датајкта;
            }
            set
            {
                датајкта = value;
            }
        }

        /// <summary>
        ///’ранит дату договора
        /// </summary>
        public string ƒатаƒоговора
        {
            get
            {
                return датаƒоговора;
            }
            set
            {
                датаƒоговора = value;
            }
        }

        /// <summary>
        /// ’ранит номер и дату договора
        /// </summary>
        public string Ќомерƒоговора
        {
            get
            {
                return номерƒоговора;
            }
            set
            {
                номерƒоговора = value;
            }
        }

        /// <summary>
        /// ’ранит сумму оказываемых услуг дл€ конкретного договора
        /// </summary>
        public string Sum
        {
            get
            {
                return sum;
            }
            set
            {
                sum = value;
            }
        }

        /// <summary>
        /// ‘»ќ льготника
        /// </summary>
        public string ‘»ќ_Ћьготник
        {
            get
            {
                return льготник;
            }
            set
            {
                льготник = value;
            }
        }

        /// <summary>
        /// флаг указывает что акт приЄма содержит ошибки в видах работ
        /// </summary>
        public bool FlagError
        {
            get
            {
                return flagError;
            }
            set
            {
                flagError = value;
            }
        }

        /// <summary>
        /// Ќомер акта выполненных работ
        /// </summary>
        public string Ќомерјкта
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
        /// —писок ошибок
        /// </summary>
        public List<ErrorsReestrUnload> —писокќшибок
        {
            get
            {
                return errorList;
            }
            set
            {
                errorList = value;
            }
        }

        /// <summary>
        /// ‘лаг указывает на наличие ошибки в льготной категории
        /// </summary>
        public bool FlagЋьготна€ атегори€ { get; set; }

        /// <summary>
        /// ”казывает текущую льготную категорию.
        /// </summary>
        //public string Ћьготна€ атегри€ { get; set; }

        /// <summary>
        /// ”казывает какие льготные категории указаны в Ѕƒ и в файле.
        /// </summary>
        public PreferentCategory Ћьготна€ атегори€ { get; set; }

    }
}
