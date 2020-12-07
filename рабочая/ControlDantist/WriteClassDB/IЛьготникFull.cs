using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public interface IЛьготникFull : IЛьготник
    {
    //   [Фамилия]
    //  ,[Имя]
    //  ,[Отчество]
    //  ,[ДатаРождения]
      string улица {set; get;}
      string НомерДома {get; set;}
      string корпус {get; set;}
      string НомерКвартиры {get; set;}
      string СерияПаспорта {get; set;}
      string НомерПаспорта {get; set;}
      string ДатаВыдачиПаспорта {get; set;}
      string КемВыданПаспорт {get; set;}
      int id_льготнойКатегории {get; set;}
      int id_документ {get;set;}
      string СерияДокумента {get; set;}
      string НомерДокумента {get; set;}
      string ДатаВыдачиДокумента {get; set;}
      string КемВыданДокумент {get; set;}
      int id_область {get; set;}
      int id_район {get; set;}
      int id_насПункт {get;set;}
      string Снилс { get; set; }
    }
}
