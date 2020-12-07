using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ClassValidRegions
{
    public class PersonValidEsrn
    {
        public int IdContract { get; set; }
        public string номерДоговора { get; set; }
  
        public string фамилия { get; set; }
    
        public string имя { get; set; }

        public string отчество { get; set; }

        public string датаРождения { get; set; }

        public string серияПаспорта { get; set; }

        public string номерПаспорта { get; set; }

        public string датаВыдачиПаспорта { get; set; }

        //серия, номер и дата выдачи документа дающего право на льготу
        public string серияДокумента { get; set; }

        public string номерДокумента { get; set; }

        public string датаВыдачиДокумента { get; set; }

        /// <summary>
        /// АДрес проживания льготника.
        /// </summary>
        public string Адрес { get; set; }

        /// <summary>
        /// Флаг проверки медицинских услуг.
        /// </summary>
        public bool flagValidMedicalServices { get; set; }

        /// <summary>
        /// Флаг указывает что льготник прошел проверку по ЭСРН.
        /// </summary>
        public bool flagValidEsrn { get; set; }

        // Флаг проверки льготника в ЭСРН по ФИО.
        public bool FlagValidPersonFioEsrn { get; set; }

        /// <summary>
        /// Флаг проверки льготника в ЭСРН по пасспорту.
        /// </summary>
        public bool FlagValidPersonPassword { get; set; }

    }
}
