using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using ControlDantist.Classes;
using System.Text.RegularExpressions;

namespace ControlDantist.ValidPersonRegion
{
    public class PersonAddress
    {
        private int? idRegion = 0;

        /// <summary>
        /// Id района области.
        /// </summary>
        public int IdRegion { get; set; }

        public PersonAddress(int? regionCode)
        {
            if (regionCode != null)
            {
                idRegion = regionCode;
            }
            else
            {
                idRegion = 0;
            }
        }

        /// <summary>
        /// Получаем название района обасти по таблице НаименованиеРайона.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="transact"></param>
        /// <returns></returns>
        public string GetNameRegion(SqlConnection con, SqlTransaction transact)
        {

            // Переменная для хранения названия района области.
            string NameRegion = string.Empty;

           
            // Флаг указывающий, что проверка прошла успешно.
            bool flagValid = false;

            //string query =  "select РайонОбласти from НаименованиеРайона " +
            //                "where id_район = ( " +
            //                "select id_район from Льготник " +
            //                "where id_льготник in ( " +
            //                " select id_льготник from Договор " +
            //                " where НомерДоговора = '" + numContracr + "') ) ";

            string query = "select РайонОбласти from НаименованиеРайона " +
                            "where id_район = "+ this.idRegion +" ";
                            

            DataTable tab = ТаблицаБД.GetTableSQL(query, "Район",con,transact);

            // В случае если не найден район.
            if (tab.Rows.Count == 0)
            {
                string queryRegion = "select NameRegion from dbo.РайонОбласти " +
                                     "where idRegion = "+ this.idRegion +" ";

                DataTable tabRegion = ТаблицаБД.GetTableSQL(queryRegion, "Район", con, transact);

                if (tabRegion.Rows.Count > 0)
                {
                    NameRegion = tabRegion.Rows[0]["NameRegion"].ToString().Trim();

                    this.IdRegion = (int)this.idRegion;

                    return NameRegion;
                }
                else
                {
                    this.idRegion = 0;
                    return "";
                }
            }

            // Получим наименвание района которое изначально пришло с проектом договора.
            string nameRegionFile = tab.Rows[0]["РайонОбласти"].ToString();

            //Тест стереть.
            //nameRegionFile = "Саратовская обл. Балаковскый р-н";

            // Распарсим строку.
            string iStr = Regex.Replace(nameRegionFile, "[ ]+", " ");

            // Разобъем строку на массив строк.
            string[] nameRegions = iStr.Split(' ');

            int id_Region = 0;

            foreach (string name in nameRegions)
            {

               // Получим длинну строки.
                int lengthNAme = name.Length;

                if (Valid(name) != true)
                {

                    // Разделим строку пополам и округлим в большую сторону.
                    int l = Segmentation(lengthNAme);

                    for(int i = l; i <= lengthNAme; i++)
                    {
                        var asd = i;

                        string nameRegion = name.Substring(0, i);

                        //string regionName = GetRegion(con, transact, name, out flagValid, out id_Region);
                        string regionName = GetRegion(con, transact, nameRegion, out flagValid, out id_Region);

                        if (flagValid == true)
                        {
                            NameRegion = regionName;

                            this.IdRegion = id_Region;

                            return NameRegion;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            this.idRegion = 0;
            return "";
        }


        /// <summary>
        /// Получаем название района обасти по таблице РайонОбласти.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="transact"></param>
        /// <returns></returns>
        private string GetRegion(SqlConnection con, SqlTransaction transact, string nameRegion, out bool flagValid, out int idRegion)
        {
            flagValid = false;

            idRegion = 0;

            string query = "select idRegion,NameRegion from РайонОбласти " +
                           "where NameRegion like '" + nameRegion + "%' ";

            DataTable tab = ТаблицаБД.GetTableSQL(query, "Район", con, transact);

            if (tab.Rows.Count > 0)
            {
                flagValid = true;

                // Вернём id района области.
                idRegion = Convert.ToInt32(tab.Rows[0]["idRegion"]);

                // Вернем наименование области.
                return tab.Rows[0]["NameRegion"].ToString();
            }
            else
            {
                flagValid = false;
                return "";
            }
        }

        private bool Valid(string name)
        {
            bool flagError = false;

            if (name.ToLower().Trim() == "Саратовская".Trim().ToLower())
            {
                flagError = true;
            }
            else if (name.ToLower().Trim() == "область".Trim().ToLower())
            {
                flagError = true;
            }
            else if (name.ToLower().Trim() == "обл".Trim().ToLower())
            {
                flagError = true;
            }
            else if (name.ToLower().Trim() == "обл.".Trim().ToLower())
            {
                flagError = true;
            }

            return flagError;
        }

        /// <summary>
        /// Деление
        /// </summary>
        /// <returns></returns>
        private int Segmentation(int lengthString)
        {

            double pages_double = lengthString / 2; // kol = 614
            int pages = Convert.ToInt16(Math.Ceiling(pages_double));

            return pages;
        }






    }
}
