using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.ReportVipNet
{
    /// <summary>
    /// Класс описывающий льготника.
    /// </summary>
    public class Person : IPersonRegistr
    {
        public string FirstName {get; set;}


        public string Name { get; set; }


        public string SecondName { get; set; }


        public DateTime DateBirth { get; set; }


        public string SeriesDoc { get; set; }


        public string NumDoc { get; set; }


        public decimal SumAct { get; set; }


        public DateTime DateWriteAct { get; set; }


        public string Region { get; set; }


        public string City { get; set; }


        public string Street { get; set; }


        public string NumHous { get; set; }


        public string NumApartment { get; set; }


        public string NumBobyBuilder { get; set; }
        
    }
}
