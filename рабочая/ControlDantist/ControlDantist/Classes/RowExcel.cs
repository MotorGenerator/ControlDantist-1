using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Строка с льготниками импортированных из Excel
    /// </summary>
    public class RowExcel
    {
        private string num;
        private string фамилия;
        private string имя;
        private string отчество;

        /// <summary>
        /// Хранит номер договора
        /// </summary>
        public string Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
            }
        }

        /// <summary>
        /// Фамилия льготника
        /// </summary>
        public string Фамилия
        {
            get
            {
                return фамилия;
            }
            set
            {
                фамилия = value;
            }
        }

        /// <summary>
        /// Имя льготника
        /// </summary>
        public string Имя
        {
            get
            {
                return имя;
            }
            set
            {
                имя = value;
            }
        }

        /// <summary>
        /// Отчество льготника
        /// </summary>
        public string Отчество
        {
            get
            {
                return отчество;
            }
            set
            {
                отчество = value;
            }
        }
                
    }
}
