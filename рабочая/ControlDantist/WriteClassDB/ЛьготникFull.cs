using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public class ЛьготникFull: IЛьготникFull
    {
        public string Famili { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string DateBirtch { get; set; }
        public string улица { set; get; }
        public string НомерДома { get; set; }
        public string корпус { get; set; }
        public string НомерКвартиры { get; set; }
        public string СерияПаспорта { get; set; }
        public string НомерПаспорта { get; set; }
        public string ДатаВыдачиПаспорта { get; set; }
        public string КемВыданПаспорт { get; set; }
        public int id_льготнойКатегории { get; set; }
        public int id_документ { get; set; }
        public string СерияДокумента { get; set; }
        public string НомерДокумента { get; set; }
        public string ДатаВыдачиДокумента { get; set; }
        public string КемВыданДокумент { get; set; }
        public int id_область { get; set; }
        public int id_район { get; set; }
        public int id_насПункт { get; set; }
        public string Снилс { get; set; }
       
    }
}
