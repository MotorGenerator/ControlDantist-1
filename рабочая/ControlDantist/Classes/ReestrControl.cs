using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Ћьготник указанный в реестре прошедший проверку
    /// </summary>
    public class ReestrControl
    {
        private string номер;
        private string фио;
        private string датаЌомерƒоговора;
        private string номерјктаќказанных”слуг;
        private string документЋьгота;
        private string стоимость”слуг;

        /// <summary>
        /// ’ранит номер
        /// </summary>
        public string Ќомер
        {
            get
            {
                return номер;
            }
            set
            {
                номер = value;
            }
        }

        /// <summary>
        /// —тоимость услуги оказанной текущему льготнику
        /// </summary>
        public string —тоимость”слуг
        {
            get
            {
                return стоимость”слуг;
            }
            set
            {
                стоимость”слуг = value;
            }
        }

        /// <summary>
        /// —ери€ и дата документа о праве на льготу
        /// </summary>
        public string ƒокументЋьгота
        {
            get
            {
                return документЋьгота;
            }
            set
            {
                документЋьгота = value;
            }
        }

        /// <summary>
        /// Ќомер и дата акта оказанных услуг
        /// </summary>
        public string Ќомерјктаќказанных”слуг
        {
            get
            {
                return номерјктаќказанных”слуг;
            }
            set
            {
                номерјктаќказанных”слуг = value;
            }
        }

        /// <summary>
        /// ’ранит данные по договору о предоставлении услуг
        /// </summary>
        public string ƒатаЌомерƒоговора
        {
            get
            {
                return датаЌомерƒоговора;
            }
            set
            {
                датаЌомерƒоговора = value;
            }
        }

        /// <summary>
        /// ’ранит ‘»ќ льготника
        /// </summary>
        public string ‘»ќ
        {
            get
            {
                return фио;
            }
            set
            {
                фио = value;
            }
        }
    }
}
