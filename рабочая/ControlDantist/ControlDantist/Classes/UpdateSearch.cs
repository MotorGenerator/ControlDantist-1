using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ControlDantist.Classes
{
    class UpdateSearch:ICommand
    {
        //переменная хранит номер конфигурации
        private int numСonfig;

        public UpdateSearch(int numberConfig)
        {
            numСonfig = numberConfig;
        }

        public void Execute()
        {
            //string query = "UPDATE ConfgSearch " +
            //               "SET [configSearch] = " + numСonfig + " " +
            //               "WHERE id_config = (select top 1 id_config from dbo.ConfgSearch) ";

            //SqlConnection con = new SqlConnection(ConnectDB.ConnectionString());
            //SqlCommand com = new SqlCommand(query, con);
            //con.Open();
            //com.ExecuteNonQuery();
            //con.Close();

            using (FileStream fs = File.OpenWrite("Config.dll"))
            using (TextWriter writ = new StreamWriter(fs))
            {
                //if (this.chkВыгрузки.Checked == true)
                //{
                    writ.WriteLine(numСonfig);
                //}
                //else
                //{
                //    writ.WriteLine("0");
                //}
            }

        }

    }
}
