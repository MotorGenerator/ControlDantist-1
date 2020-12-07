using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ControlDantist.Classes;

namespace ControlDantist.Reports
{
    public class Registr
    {
        public DataTable GetDataTable(string query, bool flagLetter)
        {
            DataTable tab = new DataTable();

            // Сформируем структуру таблицы.
            DataColumn col0 = new DataColumn("id_акт", typeof(int));
            tab.Columns.Add(col0);
            DataColumn col1 = new DataColumn("НомерАкта", typeof(string));
            tab.Columns.Add(col1);
            DataColumn col2 = new DataColumn("НомерРеестра", typeof(string));
            tab.Columns.Add(col2);
            DataColumn col3 = new DataColumn("НомерСчётФактуры", typeof(string));
            tab.Columns.Add(col3);
            DataColumn col4 = new DataColumn("id_договор", typeof(int));
            tab.Columns.Add(col4);
            DataColumn col5 = new DataColumn("НомерДоговора", typeof(string));
            tab.Columns.Add(col5);
            DataColumn col6 = new DataColumn("Фамилия", typeof(string));
            tab.Columns.Add(col6);
            DataColumn col7 = new DataColumn("Имя", typeof(string));
            tab.Columns.Add(col7);
            DataColumn col8 = new DataColumn("Отчество", typeof(string));
            tab.Columns.Add(col8);
            DataColumn col9 = new DataColumn("ЛьготнаяКатегория", typeof(string));
            tab.Columns.Add(col9);
            DataColumn col10 = new DataColumn("Сумма", typeof(string));
            tab.Columns.Add(col10);
            DataColumn col11 = new DataColumn("ДатаЗаписиДоговора", typeof(string));
            tab.Columns.Add(col11);
            DataColumn col111 = new DataColumn("ДатаРеестра", typeof(string));
            tab.Columns.Add(col111);
            DataColumn col112 = new DataColumn("ДатаАктаВыполненныхРабот", typeof(string));
            tab.Columns.Add(col112);
            if (flagLetter == false)
            {
                DataColumn col12 = new DataColumn("logWrite", typeof(string));
                tab.Columns.Add(col12);
                DataColumn col13 = new DataColumn("logDate", typeof(string));
                tab.Columns.Add(col13);
            }
            //Наименование населённого пункта
            DataColumn col14 = new DataColumn("Наименование", typeof(string));
            tab.Columns.Add(col14);

            DataColumn col15 = new DataColumn("Адрес", typeof(string));
            tab.Columns.Add(col15);

            DataColumn col16 = new DataColumn("Район", typeof(string));
            tab.Columns.Add(col16);

            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                try
                {
                    con.Open();
                }
                catch
                {
                    return tab;
                }
                SqlTransaction transact = con.BeginTransaction();

                //DataTable dtContract = ТаблицаБД.GetTableSQL(query, "ТаблицаДоговоров", con, transact);

                string iTestQuery = query;

                SqlCommand com = new SqlCommand(query, con);
                com.Transaction = transact;

                int iCountD = 0;

                SqlDataReader read = com.ExecuteReader();
                while (read.Read())
                {
                    DataRow row = tab.NewRow();
                    row["id_акт"] = read["id_акт"].ToString().Trim();

                    row["id_договор"] = read["id_договор"].ToString().Trim();
                    row["НомерАкта"] = read["НомерАкта"].ToString().Trim();
                    row["НомерРеестра"] = read["НомерРеестра"].ToString().Trim();

                    row["НомерСчётФактуры"] = read["НомерСчётФактуры"].ToString().Trim();
                    row["НомерДоговора"] = read["НомерДоговора"].ToString().Trim();
                    row["Фамилия"] = read["Фамилия"].ToString().Trim();

                    row["Имя"] = read["Имя"].ToString().Trim();
                    row["Отчество"] = read["Отчество"].ToString().Trim();
                    row["ЛьготнаяКатегория"] = read["ЛьготнаяКатегория"].ToString().Trim();

                    row["Сумма"] = Convert.ToDecimal(read["Сумма"]).ToString("c").Trim();
                   
                    if (read["ДатаРеестра"] != DBNull.Value)
                    {
                        row["ДатаРеестра"] = Convert.ToDateTime(read["ДатаРеестра"]).ToShortDateString().Trim();
                    }
                    if (read["ДатаАктаВыполненныхРабот"] != DBNull.Value)
                    {
                        row["ДатаАктаВыполненныхРабот"] = Convert.ToDateTime(read["ДатаАктаВыполненныхРабот"]).ToShortDateString().Trim();
                    }
                    if (read["ДатаЗаписиДоговора"] != DBNull.Value)
                    {
                        row["ДатаЗаписиДоговора"] = Convert.ToDateTime(read["ДатаЗаписиДоговора"]).ToShortDateString().Trim();
                    }

                    if (flagLetter == false)
                    {
                        row["logWrite"] = read["logWrite"].ToString().Trim();
                        //row["logDate"] = read["logDate"].ToString().Trim(); // Старая реализация.
                        //row["logDate"] = Convert.ToDateTime(read["ДатаЗаписиДоговора"]).ToShortDateString().Trim();
                        if (read["ДатаЗаписиДоговора"] != DBNull.Value)
                        {
                            row["ДатаЗаписиДоговора"] = Convert.ToDateTime(read["ДатаЗаписиДоговора"]).ToShortDateString().Trim();
                        }
                    }

                    int id_район = 0;
                    int id_насПункт = 0;

                    if (read["id_район"] != DBNull.Value)
                    {
                        id_район = Convert.ToInt32(read["id_район"]);

                        string queryR = "SELECT РайонОбласти FROM НаименованиеРайона where id_район = " + id_район + " ";

                        DataTable tabРайон = ТаблицаБД.GetTableSQL(queryR, "НазваниеРайона");
                        if (tabРайон.Rows.Count != 0)
                        {
                            string район = ТаблицаБД.GetTableSQL(queryR, "НазваниеРайона").Rows[0][0].ToString().Trim();
                            row["Район"] = район;
                        }

                    }

                    if (read["id_насПункт"] != DBNull.Value)
                    {
                        id_насПункт = Convert.ToInt32(read["id_насПункт"]);

                        string queryA = "SELECT Наименование FROM НаселённыйПункт where id_насПункт = " + id_насПункт + " ";
                        DataTable tabГород = ТаблицаБД.GetTableSQL(queryA, "НаселённыйПункт");

                        if (tabГород.Rows.Count != 0)
                        {
                            string город = ТаблицаБД.GetTableSQL(queryA, "НаселённыйПункт").Rows[0][0].ToString().Trim();
                            row["Наименование"] = город;
                        }

                    }

                    //Получим данные об адресе проживания льготника

                    StringBuilder buldAdr = new StringBuilder();
                    buldAdr.Append(read["улица"].ToString().Trim() + " ");

                    buldAdr.Append(read["НомерДома"].ToString().Trim() + " ");
                    buldAdr.Append(read["корпус"].ToString().Trim() + " ");
                    buldAdr.Append(read["НомерКвартиры"].ToString().Trim());

                    row["Адрес"] = buldAdr.ToString().Trim();

                    tab.Rows.Add(row);

                    iCountD++;
                }

                return tab;
            }
        }
    }
}
