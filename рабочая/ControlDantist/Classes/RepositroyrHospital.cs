using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    public class RepositroyrHospital
    {

        public IEnumerable<ПоликлинникиИнн> GetListHospital()
        {
            string query = "SELECT [F1] " +
                           ",[F2] " +
                           ",[F3] " +
                           " FROM [ПоликлинникиИнн]";

            DataTable tab = ТаблицаБД.GetTableSQL(query, "ПоликлиннкаИНН");

            List<ПоликлинникиИнн> list = new List<ПоликлинникиИнн>();

            foreach (DataRow row in tab.Rows)
            {
                ПоликлинникиИнн it = new ПоликлинникиИнн();
                it.ID = Convert.ToInt16(row["F1"]);
                it.Наименование = row["F2"].ToString();
                it.ИНН = row["F3"].ToString();

                list.Add(it);
            }

            return list;
        }


    }
}
