using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ControlDantist.Classes
{
    public class ContractsForLetter
    {
        StringBuilder buildQuery;



        public ContractsForLetter()
        {
            buildQuery = new StringBuilder();

            //string query = " SELECT dbo.Договор.НомерДоговора, dbo.РайонОбласти.NameRegion, dbo.НаселённыйПункт.Наименование,   dbo.Льготник.Фамилия, dbo.Льготник.Имя, dbo.Льготник.Отчество, dbo.Льготник.ДатаРождения, dbo.Льготник.улица, " +
            string query = " SELECT dbo.Договор.НомерДоговора, dbo.НаселённыйПункт.Наименование,   dbo.Льготник.Фамилия, dbo.Льготник.Имя, dbo.Льготник.Отчество, dbo.Льготник.ДатаРождения, dbo.Льготник.улица, " +
                           " dbo.Льготник.НомерДома,  dbo.Льготник.корпус, dbo.Льготник.НомерКвартиры, " +
                           " dbo.Льготник.СерияДокумента, dbo.Льготник.НомерДокумента,  " +
                           " dbo.ЛьготнаяКатегория.ЛьготнаяКатегория, dbo.Договор.СуммаАктаВыполненныхРабот, " +
                           " dbo.Льготник.id_район " +
                           //", dbo.РайонОбласти.idRegion  " + 
                           " FROM         dbo.Льготник " +
                           " INNER JOIN dbo.ЛьготнаяКатегория  " +
                           " ON dbo.Льготник.id_льготнойКатегории = dbo.ЛьготнаяКатегория.id_льготнойКатегории  " +
                           "  INNER JOIN dbo.Договор " +
                           " ON dbo.Льготник.id_льготник = dbo.Договор.id_льготник " +
                           //" INNER JOIN dbo.РайонОбласти  " +
                           //" ON dbo.РайонОбласти.idRegion = dbo.Льготник.id_район " +
                           " INNER JOIN dbo.НаселённыйПункт " +
                           " ON dbo.НаселённыйПункт.id_насПункт = dbo.Льготник.id_насПункт " +
                           " where dbo.Договор.ФлагПроверки = 'True' " +
                           " and СуммаАктаВыполненныхРабот is not null ";

           buildQuery.Append(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strNumContract"></param>
        /// <returns></returns>
        public List<ItemLetterToMinistr> GetPersons(string strNumContract)
        {
            string query = " and dbo.Договор.ФлагПроверки = 'True' "+ 
                           //" and dbo.Договор.ФлагНаличияАкта = 'False' " +
                           " and СуммаАктаВыполненныхРабот is not null " +
                           "and LOWER(LTRIM(RTRIM(dbo.Договор.НомерДоговора))) in ("+ strNumContract +") ";

            buildQuery.Append(query);

            string strQuery = buildQuery.ToString();

            DataTable tabReestr = ТаблицаБД.GetTableSQL(strQuery, "TableLetter");

            //List<IItemLetter> list = new List<IItemLetter>();

            List<ItemLetterToMinistr> list = new List<ItemLetterToMinistr>();

            foreach (DataRow row in tabReestr.Rows)
            {
                ItemLetterToMinistr item = new ItemLetterToMinistr();

                if (row["ДатаРождения"] != DBNull.Value)
                {
                    item.ДатаРождения = row["ДатаРождения"].ToString().Trim();
                }

                if (row["Имя"] != DBNull.Value)
                {
                    item.Имя = row["Имя"].ToString().Trim();
                }

                if (row["НомерКвартиры"] != DBNull.Value)
                {
                    item.Квартира = row["НомерКвартиры"].ToString().Trim();
                }

                if (row["корпус"] != DBNull.Value)
                {
                    item.Корпус = row["корпус"].ToString().Trim();
                }

                if (row["Наименование"] != DBNull.Value)
                {
                    item.НаселенныйПункт = row["Наименование"].ToString().Trim();
                }

                if (row["НомерДокумента"] != DBNull.Value)
                {
                    item.НомерДокумента = row["НомерДокумента"].ToString().Trim();
                }

                if (row["НомерДома"] != DBNull.Value)
                {
                    item.НомерДома = row["НомерДома"].ToString().Trim();
                }

                if (row["Отчество"] != DBNull.Value)
                {
                    item.Отчество = row["Отчество"].ToString().Trim();
                }

                if (row["СерияДокумента"] != DBNull.Value)
                {
                    item.СерияДокумента = row["СерияДокумента"].ToString().Trim();
                }

                // Так как мы еще пока не знаем какая сумма будет проставлена.
                item.СуммаАкта = 0.0m;


                if (row["улица"] != DBNull.Value)
                {
                    item.Улица = row["улица"].ToString().Trim();
                }

                if (row["Фамилия"] != DBNull.Value)
                {
                    item.Фамилия = row["Фамилия"].ToString().Trim();
                }

                //if (row["NameRegion"] != DBNull.Value)
                //{
                //    item.РайонОбласти = row["NameRegion"].ToString().Trim();
                //}

                if (row["НомерДокумента"] != DBNull.Value)
                {
                    item.НомерДокумента = row["НомерДокумента"].ToString().Trim();
                }

                if (row["НомерДоговора"] != DBNull.Value)
                {
                    item.НомерДоговора = row["НомерДоговора"].ToString().Trim();
                }


                if (row["id_район"] != DBNull.Value)
                {
                    item.IdRegion = Convert.ToInt32(row["id_район"]);
                }

                list.Add(item);
            }

            return list;
        }
    }
}
