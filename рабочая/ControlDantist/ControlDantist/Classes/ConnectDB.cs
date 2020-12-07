using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ControlDantist.Classes
{
    class ConnectDB
    {
        static public string ConnectionString()
        {
            return ConfigurationSettings.AppSettings["connect"].ToString(); //connectionString

            //string sCon = string.Empty;
            //sCon = ConfigurationSettings.AppSettings["connectionString"].ToString();

            //return sCon;
        }
    }
}
