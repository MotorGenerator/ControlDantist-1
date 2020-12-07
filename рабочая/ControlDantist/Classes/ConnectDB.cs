using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ControlDantist.Classes
{
    public class ConnectDB
    {
        /// <summary>
        /// Возвращает строку подключения к серверу баз данных.
        /// </summary>
        /// <returns></returns>
        static public string ConnectionString()
        {
            return ConfigurationSettings.AppSettings["connect"].ToString(); 

        }
    }
}
