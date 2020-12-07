using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ControlDantist.ClassValidRegions
{
    /// <summary>
    /// Формирует коллекцию классов.
    /// </summary>
    public class FillPerson
    {
        string sConnect = string.Empty;
        string query = string.Empty;

        public FillPerson()
        {
            // Присвоим строку подключения к БД.
            sConnect = ConfigurationSettings.AppSettings["connect"];
        }

        public List<Person> GetPersonzs()
        {
            List<Person> listPerson = new List<Person>();

            query = "select  id_льготник, Фамилия,Имя,Отчество, ДатаРождения,СерияПаспорта, " + 
                           " НомерПаспорта,ДатаВыдачиПаспорта,СерияДокумента,НомерДокумента, " +
                           " ДатаВыдачиДокумента  from Льготник " +
                            "where id_льготник in (select id_льготник from Договор " +
                            "where ДатаЗаписиДоговора > '20180501' and ДатаЗаписиДоговора < '20180901')";

            try
            {
                using (SqlConnection con = new SqlConnection(sConnect))
                {
                    con.Open();

                    SqlCommand com = new SqlCommand(query, con);

                    SqlDataReader read = com.ExecuteReader();

                    while (read.Read())
                    {
                        Person person = new Person();
                        person.FlagValid = false;
                        person.id_льготник = Convert.ToInt32(read["id_льготник"]);
                        person.idRegion = 0;
                        person.Фамилия = read["Фамилия"].ToString();
                        person.Имя = read["Имя"].ToString();
                        person.Отчество = read["Отчество"].ToString();
                        person.ДатаРождения = Convert.ToDateTime(read["ДатаРождения"]);
                        person.СерияПаспорта = read["СерияПаспорта"].ToString();
                        person.НомерПаспорта = read["НомерПаспорта"].ToString();
                        person.ДатаВыдачиПаспорта = Convert.ToDateTime(read["ДатаВыдачиПаспорта"]);
                        person.СерияДокумента = read["СерияДокумента"].ToString();
                        person.НомерДокумента = read["НомерДокумента"].ToString();
                        person.ДатаВыдачиДокумента = Convert.ToDateTime(read["ДатаВыдачиДокумента"]);

                        listPerson.Add(person);
                    }
                }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ОШибка при считывании данных по льготникам в нашей БД.");
            }

            return listPerson;
        }
    }
}
