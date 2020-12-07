using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    public static class  УслугиДоговорForComparer
    {
        public static string InsertDate(DataTable table)
        {
            StringBuilder builder = new StringBuilder();

            foreach(DataRow row in table.Rows)
            {
                string insert = "INSERT INTO [УслугиПоДоговоруCopy] " +
                                "([НаименованиеУслуги] " +
                                ",[цена] " +
                                ",[Количество] " +
                                ",[id_договор] " +
                                ",[НомерПоПеречню] " +
                                ",[Сумма] " +
                                ",[ТехЛист]) " +
                                "VALUES " +
                                "('" + row["НаименованиеУслуги"].ToString() + "' " +
                                "," + Convert.ToDecimal(row["цена"]) + " " +
                                "," + Convert.ToInt32(row["Количество"]) + " " +
                                //"," + Convert.ToInt32(row["id_договор"]) + " " +
                                 "," + 3861 + " " +
                                ",'" + row["НомерПоПеречню"].ToString() + "' " +
                                "," + Convert.ToDecimal(row["Сумма"]) + " " +
                                "," + Convert.ToInt32(row["ТехЛист"]) + " ) ";
                
                builder.Append(insert);
            }

            return builder.ToString().Trim();
        }
    }
}
