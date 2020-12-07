using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ControlDantist.ValidPersonRegion
{
    public class Region
    {
        public void GetRegions(List<ItemLetterToMinistr> listContract)
        {

            using (SqlConnection con = new SqlConnection(ConfigurationSettings.AppSettings["connect"]))
            {
                con.Open();

                SqlTransaction transaction = con.BeginTransaction();

                foreach (ItemLetterToMinistr item in listContract)
                {
                    PersonAddress personAddress = new PersonAddress(item.IdRegion);
                    item.РайонОбласти = personAddress.GetNameRegion(con, transaction);
                    item.IdRegion = personAddress.IdRegion;
                }

            }
        }
    }
}
