using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using ControlDantist.Find;

namespace ControlDantist.Classes
{
    public class ShowResultPerson
    {

        private IFindPerson findPerson;

        public ShowResultPerson(IFindPerson findPerson)
        {
            this.findPerson = findPerson;
        }


        public List<ValideContract> DisplayDate()
        {
            // Список для хранения резултатов поиска.
            List<ValideContract> listDisplay = new List<ValideContract>();

            using (SqlConnection con = new SqlConnection(ConnectDB.ConnectionString()))
            {
                con.Open();
                SqlTransaction transact = con.BeginTransaction();

                //DataTable dtContract = ТаблицаБД.GetTableSQL(query, "ТаблицаДоговоров", con, transact);
                SqlCommand com = new SqlCommand(this.findPerson.Query(), con);
                com.Transaction = transact;

                SqlDataReader read = com.ExecuteReader();

                IFindContract findContract = new FindContractForNumber(read);

                var list = findContract.Adapter();

                // Сгруппируем строки таблицы.
                var listG = list.GroupBy(w => w.НомерДоговора);

                listDisplay = FIltrDowbel(listG);
            }

            return listDisplay;
        }

        private List<ValideContract> FIltrDowbel(IEnumerable<IGrouping<string, ValideContract>> listG)
        {
            // Список для отображения на экране.
            List<ValideContract> listDisplay = new List<ValideContract>();

            foreach (var itms in listG)
            {
                if (itms.All(w => w.flag2019AddWrite == false) == true)
                {
                    foreach (var itm in itms)
                    {
                        listDisplay.Add(itm);
                    }
                }
                else
                {
                    foreach (var itm in itms)
                    {
                        if (itm.flag2019AddWrite == true)
                        {
                            listDisplay.Add(itm);
                        }
                    }
                }
            }

            return listDisplay;
        }
    }
}
