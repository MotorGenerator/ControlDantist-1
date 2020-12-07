using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DantistLibrary;

namespace ControlDantist.Classes
{
    public class WriteDbPerson
    {
        SqlConnection con = null;
        SqlTransaction transact = null;

        //Unload unload = null;

        public WriteDbPerson(SqlTransaction trans, SqlConnection connect)//, Unload un)
        {
            bool flagError = false;

            if(trans != null)
            {
                transact = trans;
            }
            else
            {
                flagError = true;
            }

            if (connect != null)
            {
                con = connect;
            }
            else
            {
                flagError = true;
            }

            //if (un != null)
            //{
            //    unload = un;
            //}
            //else
            //{
            //    flagError = true;
            //}

            if (flagError == true)
            {
                throw new Exception("Отсутствует подключение к БД");
            }
        }

        public void WritePerson(Unload unload, SqlTransaction transact)
        {
            // Получим номер договора в текущем реестре
            DataTable tabДоговор = unload.Договор;

            // Получим номер договора.
            string numDog = tabДоговор.Rows[0]["НомерДоговора"].ToString().Trim();

            // Загрузим данные по лгогтнику.
            DataTable tabЛьготник = unload.Льготник;

            string фамилия = tabЛьготник.Rows[0]["Фамилия"].ToString().Trim();
            string имя = tabЛьготник.Rows[0]["Имя"].ToString().Trim();
            string отчество = tabЛьготник.Rows[0]["Отчество"].ToString().Trim();

            string датаРождения = Convert.ToDateTime(tabЛьготник.Rows[0]["ДатаРождения"]).ToShortDateString();
        }

        /// <summary>
        /// Проверяет записан ли текущий договор в БД.
        /// </summary>
        /// <returns></returns>
        private int FormValidContract(string numContract, string firstName, string namePerson, string secondName, string dateBirth)
        {
            string queryDog = " select COUNT(*) from Договор where LOWER(RTRIM(LTRIM(НомерДоговора))) = LOWER(RTRIM(LTRIM('" + numContract + "'))) " +
                              " and id_льготник in (select id_льготник from Льготник " +
                              " where LOWER(RTRIM(LTRIM(Фамилия))) = LOWER(RTRIM(LTRIM('" + firstName + "'))) and LOWER(RTRIM(LTRIM(Имя))) = LOWER(RTRIM(LTRIM('" + namePerson + "'))) and LOWER(RTRIM(LTRIM(Отчество))) = LOWER(RTRIM(LTRIM('" + secondName + "'))) and ДатаРождения =  '" + Время.Дата(dateBirth) + "' and ФлагПроверки = 'True') ";

            return 0;
        }
    }
}
