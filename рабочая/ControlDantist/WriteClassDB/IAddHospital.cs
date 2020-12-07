using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public interface IAddHospital
    {
        string НаименованиеПоликлинники {get; set; }
      string КодПоликлинники {get; set;}
      string ЮридическийАдрес {get; set;}
      string ФактическийАдрес {get; set;}
      int id_главВрач {get; set;}
      int id_главБух {get; set; }
      string СвидетельствоРегистрации {get; set; }
      string ИНН {get; set;}
      string КПП {get; set;}
      string БИК {get; set;}
      string НаименованиеБанка {get; set;}
      string РасчётныйСчёт {get; set;}
      string ЛицевойСчёт {get; set;}
      string НомерЛицензии {get;set;}
      string ДатаРегистрацииЛицензии {get; set;}
      string ОГРН {get; set;}
      string СвидетельствоРегистрацииЕГРЮЛ {get; set;}
      string ОрганВыдавшийЛицензию {get; set;}
      string Постановление {get; set; }
      string ОКПО {get; set;}
      string ОКАТО {get; set;}
      bool Flag {get; set;}
      int НачальныйНомерДоговора { get; set; }

    }
}
