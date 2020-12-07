using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public interface IContract
    {
        string numContract { get; set; }
        string DateContract { get; set; }
        string DataAct { get; set; }
        decimal SummAct { get; set; }
        int IdPreferencCategory { get; set; }
        int idHospital { get; set; }
        /// <summary>
        /// Примечание.
        /// </summary>
        string Note { get; set; }

        /// <summary>
        /// ID комитета.
        /// </summary>
        int IdConmmite { get;  }


        bool FlagContract { get; set; }
        bool FalgAct { get; set; }
        int IdPerson { get; set; }
        bool FlagДопСоглашения { get; set; }
        string DateWriteContract { get; set; }
        bool FlagValidate { get; set; }
        string logWrite { get; set; }


    }
}
