﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class HeadLetterToMinistr : ItemLetterToMinistr
    {
        public string РайонОбласти { get; set; }
        public string Фамилия { get; set; }

        public string Имя { get; set; }

        public string Отчество { get; set; }

        public string ДатаРождения { get; set; }
        public string НаселенныйПункт { get; set; }
        public string Улица { get; set; }
        public string НомерДома { get; set; }
        public string Корпус { get; set; }
        public string Квартира { get; set; }
        public string СерияДокумента { get; set; }
        public string НомерДокумента { get; set; }
        //public virtual decimal СуммаАкта { get; set; }

        /// <summary>
        /// Дата подписания акта выполненных работ.
        /// </summary>
        public string ДатаАкта { get; set; }

        public string СуммаАкта { get; set; }
        
    }
}
