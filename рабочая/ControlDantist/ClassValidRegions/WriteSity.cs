using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace ControlDantist.ClassValidRegions
{
    /// <summary>
    /// Добавление в БД инфоромации по районам проживания льготника.
    /// </summary>
    public class WriteSity
    {
        private IEnumerable<Person> list;


        public WriteSity(IEnumerable<Person> listPerson)
        {
            if (listPerson.Count() > 0)
            {
                list = listPerson.Where(w=>w.FlagValid == true);
            }
        }

        public void WriteDB()
        {
            StringBuilder build = new StringBuilder();

            var rTest = list;

            foreach (Person person in list)
            {
                if (person.SNILS != null)
                {
                    string update = "UPDATE [Льготник] " +
                                    " SET [id_район] =  " + person.idRegion + " " +
                                    ", Снилс = '" + person.SNILS.Trim() + "' " +
                                    " WHERE id_льготник = " + person.id_льготник + " ";

                    build.Append(update);
                }
                else
                {
                    string update = "UPDATE [Льготник] " +
                                   " SET [id_район] =  " + person.idRegion + " " +
                                   ", Снилс = NULL " +
                                   " WHERE id_льготник = " + person.id_льготник + " ";

                    build.Append(update);
                }

            }

            string sConnect = ConfigurationSettings.AppSettings["connect"];

            var sTest = build.ToString();

            using (SqlConnection con = new SqlConnection(sConnect))
            {
                try
                {
                    con.Open();
                    SqlCommand com = new SqlCommand(build.ToString(), con);

                    com.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
