using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ClassValidRegions
{
    /// <summary>
    /// Вспомогательный класс описывающий льготника.
    /// </summary>
    public class Person
    {
        public int id_льготник {get;set;}
        public string Фамилия {get; set;}
        public string Имя {get; set;}
        public string Отчество {get; set;}
        public DateTime ДатаРождения {get; set;}
        public string СерияПаспорта {get; set;}
        public string НомерПаспорта{get; set;}
        public DateTime ДатаВыдачиПаспорта {get; set;}
        public string СерияДокумента {get;set;}
        public string НомерДокумента {get; set;}
        public DateTime ДатаВыдачиДокумента { get; set; }
        public bool FlagValid { get; set; }
        public int idRegion { get; set; }
        public string SNILS { get; set; }
    }

}
