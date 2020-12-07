using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public class Contract:IContract
    {
        public string numContract { get; set; }
        public string DateContract { get; set; }
        public string DataAct { get; set; }
        public decimal SummAct { get; set; }
        public int IdPreferencCategory { get; set; }
        public int idHospital { get; set; }
        /// <summary>
        /// Примечание.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// ID комитета.
        /// </summary>
        public int IdConmmite
        {
            get
            {
                return 1;
            }
        }


        public bool FlagContract { get; set; }
        public bool FalgAct { get; set; }
        public int IdPerson { get; set; }
        public bool FlagДопСоглашения { get; set; }
        public string DateWriteContract { get; set; }
        public bool FlagValidate { get; set; }
        public string logWrite { get; set; }

    }
}
