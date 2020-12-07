using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.WriteClassDB
{
    public interface IStringQuery
    {
        IЛьготник PersonFio { get; set; }
        ISity NameSity { get; set; }
        string ЛьготнаяКатегория { get; set; }
        string ТипДокумента { get; set; }

        IЛьготникFull pf { get; set; }

        IContract contract { get; set; }

        /// <summary>
        /// id Файла проектов договоров.
        /// </summary>
        int IdFileRegistrProject { get; set; }

        void GetServicesContract(IEnumerable<IServicesContract> listContract);

        // ИНН поликлинники.
        IHospital hospInn { get; set; }

        /// <summary>
        /// Данные по поликлиннике.
        /// </summary>
        IAddHospital dataHospital { get; set; }

        string Query(int count);

        string QueryReception(int count);
    }
}
