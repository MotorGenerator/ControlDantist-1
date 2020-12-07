using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public class InsertDateHospital : IAddHospital
    {



        public string НаименованиеПоликлинники { get; set; }
        public string КодПоликлинники { get; set; }
        public string ЮридическийАдрес { get; set; }
        public string ФактическийАдрес { get; set; }
        public int id_главВрач { get; set; }
        public int id_главБух { get; set; }
        public string СвидетельствоРегистрации { get; set; }
        public string ИНН { get; set; }
        public string КПП { get; set; }
        public string БИК { get; set; }
        public string НаименованиеБанка { get; set; }
        public string РасчётныйСчёт { get; set; }
        public string ЛицевойСчёт { get; set; }
        public string НомерЛицензии { get; set; }
        public string ДатаРегистрацииЛицензии { get; set; }
        public string ОГРН { get; set; }
        public string СвидетельствоРегистрацииЕГРЮЛ { get; set; }
        public string ОрганВыдавшийЛицензию { get; set; }
        public string Постановление { get; set; }
        public string ОКПО { get; set; }
        public string ОКАТО { get; set; }
        public bool Flag { get; set; }
        public int НачальныйНомерДоговора { get; set; }
       
    }
}
