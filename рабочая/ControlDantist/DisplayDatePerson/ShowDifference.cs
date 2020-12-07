using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Data;

namespace ControlDantist.DisplayDatePerson
{
    /// <summary>
    /// Показывает разницу в услугах.
    /// </summary>
    public class ShowDifference
    {
        private int idContract = 0;
        public ShowDifference(int idContract)
        {
            this.idContract = idContract;
        }

        public List<ServicesError> Display()
        {
           DataTable dataTable =  ТаблицаБД.GetTableSQL(Query(), "РазницаУслуг");

            List<ServicesError> listName = new List<ServicesError>();

            if (dataTable.Rows.Count > 0)
            {

                var list = dataTable.AsEnumerable().Select(m => new ServicesError()
                {
                    NameServices = m.Field<string>("НаименованиеУслуги"),
                    MoneyServices = m.Field<decimal>("цена").ToString("c")
                }).ToList();

                listName.AddRange(list);

            }

            return listName;

        }

        public List<ServicesError> DisplayErrorServer()
        {
            DataTable dataTable = ТаблицаБД.GetTableSQL(QueryServer(), "РазницаУслуг");

            List<ServicesError> listName = new List<ServicesError>();

            if (dataTable.Rows.Count > 0)
            {

                var list = dataTable.AsEnumerable().Select(m => new ServicesError()
                {
                    NameServices = m.Field<string>("НаименованиеУслуги"),
                    MoneyServices = m.Field<decimal>("цена").ToString("c")
                }).ToList();

                listName.AddRange(list);

            }

            return listName;

        }



        private string Query()
        {
           
            //string query = "select Tab2.[НаименованиеУслуги],Tab2.Цена from ( " +
            //                "select[ВидУслуги],Цена from ВидУслуг " +
            //                "inner join( " +
            //                "select MAX(Поликлинника.id_поликлинника) as idHospital, Поликлинника.ИНН from Договор " +
            //                "inner join Поликлинника " +
            //                "ON Поликлинника.id_поликлинника = Договор.id_поликлинника " +
            //                " where Договор.id_договор = "+ this.idContract +" " +
            //                " group by ИНН) as Tab1 " +
            //                " ON Tab1.idHospital = ВидУслуг.id_поликлинника) as Tab1 " +
            //                " right outer join(select[НаименованиеУслуги], [цена] from Договор " +
            //                " inner join УслугиПоДоговору ON Договор.id_договор = УслугиПоДоговору.id_договор " +
            //                " where Договор.id_договор = " + this.idContract + " and idFileRegistProgect is not null ) as Tab2 " + // and ФлагВозвратНаДоработку = 0
            //                " ON LTRIM(RTRIM(LOWER(REPLACE(Tab1.ВидУслуги, ' ', '')))) = LTRIM(RTRIM(LOWER(REPLACE(Tab2.НаименованиеУслуги, ' ', '')))) " +
            //                " and Tab1.Цена = Tab2.[цена] " +
            //                " where Tab1.[ВидУслуги] is null and Tab1.Цена is null ";

            string query = @"select Tab2.[НаименованиеУслуги],Tab2.Цена from (select[ВидУслуги],Цена from ВидУслуг
                            INNER JOIN(
                            select IdHospital from ПоликлинникиИнн
                            inner join (select MAX(id_поликлинника) as IdHospital,ИНН from Поликлинника
                            group by ИНН) as TabHosp
                            ON ПоликлинникиИнн.F3 = TabHosp.ИНН
                            inner join(select Поликлинника.ИНН from Поликлинника
                            inner join Договор
                            ON Поликлинника.id_поликлинника = Договор.id_поликлинника
                            where Договор.id_договор = " + this.idContract + ") as Hosp2 " +
                            @"ON TabHosp.ИНН = Hosp2.ИНН) AS TabHosp
                            ON ВидУслуг.id_поликлинника = TabHosp.IdHospital) as Tab1
                            right outer join(select[НаименованиеУслуги], [цена] from Договор
                            inner join УслугиПоДоговору
                            ON Договор.id_договор = УслугиПоДоговору.id_договор
                            where Договор.id_договор = " + this.idContract + " and idFileRegistProgect is not null) " +
                            @"as Tab2
                            ON LTRIM(RTRIM(LOWER(REPLACE(Tab1.ВидУслуги, ' ', '')))) = LTRIM(RTRIM(LOWER(REPLACE(Tab2.НаименованиеУслуги, ' ', ''))))
                            and Tab1.Цена = Tab2.[цена]
                            where Tab1.[ВидУслуги] is null 
                            and Tab1.Цена is null 
                            ";

            return query;
        }

        private string QueryServer()
        {
            string query = "select Tab1.[ВидУслуги],Tab1.Цена from ( " +
                            "select[ВидУслуги],Цена from ВидУслуг " +
                            "inner join( " +
                            "select MAX(Поликлинника.id_поликлинника) as idHospital, Поликлинника.ИНН from Договор " +
                            "inner join Поликлинника " +
                            "ON Поликлинника.id_поликлинника = Договор.id_поликлинника " +
                            " where Договор.id_договор = " + this.idContract + " " +
                            " group by ИНН) as Tab1 " +
                            " ON Tab1.idHospital = ВидУслуг.id_поликлинника) as Tab1 " +
                            " left outer join(select[НаименованиеУслуги], [цена] from Договор " +
                            " inner join УслугиПоДоговору ON Договор.id_договор = УслугиПоДоговору.id_договор " +
                            " where Договор.id_договор = " + this.idContract + " and idFileRegistProgect is not null) as Tab2 " +
                            " ON LTRIM(RTRIM(LOWER(REPLACE(Tab1.ВидУслуги, ' ', '')))) = LTRIM(RTRIM(LOWER(REPLACE(Tab2.НаименованиеУслуги, ' ', '')))) " +
                            " and Tab1.Цена = Tab2.[цена] " +
                            " where Tab1.[ВидУслуги] is null and Tab1.Цена is null ";

            return query;
        }
    }
}
