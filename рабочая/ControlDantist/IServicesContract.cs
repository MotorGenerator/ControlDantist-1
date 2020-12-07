using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist
{
    public interface IServicesContract
    {
       string НаименованиеУслуги {get;set;}
       decimal цена {get;set;}
       int Количество  {get;set;}
       int id_договор {get;set;}
       string НомерПоПеречню {get;set;}
       decimal Сумма {get;set;}
       string ТехЛист { get; set; }

       //string Query(int id_договор);
    }
}
