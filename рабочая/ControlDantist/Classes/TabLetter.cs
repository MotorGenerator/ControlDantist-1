using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    ///  ласс дл€ хранени€ строки в письме на устранени€ недостатков в актах выполненных работ
    /// </summary>
    class TabLetter
    {
        private string фио;
        private string услугаƒоговор;
        private string услугајкт;

        /// <summary>
        /// ’ранит  ‘»ќ льготника
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

        /// <summary>
        /// ’ранит расхождениы€ услуги с актом выполненных работ
        /// </summary>
        public string ”слугаƒоговор
        {
            get
            {
                return услугаƒоговор;
            }
            set
            {
                услугаƒоговор = value;
            }
        }

        /// <summary>
        /// ’ранит расхождениы€ услуги с договором
        /// </summary>
        public string ”слугајкт
        {
            get
            {
                return услугајкт;
            }
            set
            {
                услугајкт = value;
            }
        }

    }
}
