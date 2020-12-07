using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.SqlClient;

namespace ControlDantist.Classes
{
    public static class GetPersonsПолучатеиСоцПоддержки
    {
        public static List<PersonRecipient> GetPersons(string номерРеестра, int год)
        {
            // Проверим номер реестра и год на null и на пустую строку.

            if (номерРеестра == null || номерРеестра == "" || год == null || год == 0)
            {
                throw new ExceptionYearNumber();
            }
            
            string query = "select [РайонОбласти] " +
                           ",[Наименование] " +
                           ",[Фамилия] " +
                           ",[Имя] " +
                           ",[Отчество] " +
                           ",[ДатаРождения] " +
                           ",[улица] " +
                           ",[НомерДома] " +
                           ",[корпус] " +
                           ",[НомерКвартиры] " +
                           ",[СерияДокумента] " +
                           ",[НомерДокумента] " +
                           ",[ЛьготнаяКатегория] " + 
                           ",СуммаАктаВыполненныхРабот " +
                           "from dbo.ViewListRecepients " +
                           "where НомерРеестра = '" + номерРеестра + "' and YEAR(ДатаРеестра) = "+ год +" " +
                           "order by Наименование ASC ";

// Оставим вдруг будем доробатывать.            
//            SELECT     dbo.НаименованиеРайона.РайонОбласти, dbo.ЛьготнаяКатегория.ЛьготнаяКатегория, dbo.НаселённыйПункт.Наименование, 
//                      dbo.Льготник.Фамилия, dbo.Льготник.Имя, dbo.Льготник.Отчество, dbo.Льготник.ДатаРождения, dbo.Льготник.улица, dbo.Льготник.НомерДома, 
//                      dbo.Льготник.корпус, dbo.Льготник.НомерКвартиры, dbo.Льготник.СерияДокумента, dbo.Льготник.НомерДокумента, 
//                      dbo.Договор.НомерДоговора, dbo.Договор.ДатаДоговора, dbo.Договор.ДатаАктаВыполненныхРабот, 
//                      dbo.Договор.СуммаАктаВыполненныхРабот, dbo.АктВыполненныхРабот.НомерАкта, dbo.Договор.НомерРеестра, 
//                      dbo.Договор.ДатаРеестра
//FROM         dbo.Льготник INNER JOIN
//                      dbo.НаименованиеРайона ON dbo.НаименованиеРайона.id_район = dbo.Льготник.id_район INNER JOIN
//                      dbo.НаселённыйПункт ON dbo.НаселённыйПункт.id_насПункт = dbo.Льготник.id_насПункт INNER JOIN
//                      dbo.ЛьготнаяКатегория ON dbo.ЛьготнаяКатегория.id_льготнойКатегории = dbo.Льготник.id_льготнойКатегории INNER JOIN
//                      dbo.Договор ON dbo.Договор.id_льготник = dbo.Льготник.id_льготник INNER JOIN
//                      dbo.АктВыполненныхРабот ON dbo.АктВыполненныхРабот.id_договор = dbo.Договор.id_договор
//GROUP BY dbo.НаименованиеРайона.РайонОбласти, dbo.ЛьготнаяКатегория.ЛьготнаяКатегория, dbo.НаселённыйПункт.Наименование, 
//                      dbo.Льготник.Фамилия, dbo.Льготник.Имя, dbo.Льготник.Отчество, dbo.Льготник.ДатаРождения, dbo.Льготник.улица, dbo.Льготник.НомерДома, 
//                      dbo.Льготник.корпус, dbo.Льготник.НомерКвартиры, dbo.Льготник.СерияДокумента, dbo.Льготник.НомерДокумента, 
//                      dbo.Договор.НомерДоговора, dbo.Договор.ДатаДоговора, dbo.Договор.ДатаАктаВыполненныхРабот, 
//                      dbo.Договор.СуммаАктаВыполненныхРабот, dbo.АктВыполненныхРабот.НомерАкта, dbo.Договор.НомерРеестра, 
//                      dbo.Договор.ДатаРеестра
//HAVING      (dbo.Льготник.Фамилия = N'Шарова') AND (dbo.Льготник.Имя = N'Нина')

            string sConnection = string.Empty;
            sConnection = ConfigurationSettings.AppSettings["connect"];

            // Получим подключение к БД.
            SqlConnection con = new SqlConnection(sConnection);

            // Откроем соединение.
            con.Open();

            SqlDataAdapter daAdapter = new SqlDataAdapter(query, con);

            DataSet dataSet = new DataSet();
            daAdapter.Fill(dataSet, "EmpCustShip");

            DataTable ordersQuery = dataSet.Tables["EmpCustShip"];

            // Не пойму почему не работает AsEnumerable()

            // Список для хранения льготников.
            List<PersonRecipient> list = new List<PersonRecipient>();
            
            foreach (DataRow it in ordersQuery.Rows)
            {

                PersonRecipient person = new PersonRecipient();
                person.РайонОбласти = it[0].ToString().Trim();
                person.НаселённыйПункт = it[1].ToString().Trim();
                person.Фамилия = it[2].ToString().Trim();
                person.Имя = it[3].ToString().Trim();
                person.Отчество = it[4].ToString().Trim();
                person.ДатаРождения = Convert.ToDateTime(it[5]);
                person.Улица = it[6].ToString().Trim();
                person.НомерДома = it[7].ToString().Trim();
                person.Корпус = it[8].ToString().Trim();
                person.НомерКвартиры = it[9].ToString().Trim();
                person.СерияДокумента = it[10].ToString().Trim();
                person.НомерДокумента = it[11].ToString().Trim();
                person.ЛьготнаяКатегория = it[12].ToString().Trim();
                person.СуммаВыполненныхРабот = it[13].ToString().Trim();

                list.Add(person);
            }
           
            return list;           
        }
    }
}
