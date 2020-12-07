using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace ControlDantist.Classes
{
    /// <summary>
    /// Применим паттерн адаптер.
    /// </summary>
    public class FindContractForNumber : IFindContract
    {
        private SqlDataReader read;

        public FindContractForNumber(SqlDataReader read)
        {
            this.read = read;
        }

        /// <summary>
        /// Преобразуем одно представление данных к другому.
        /// </summary>
        /// <returns></returns>
        public List<ValideContract> Adapter()
        {
            List<ValideContract> list = new List<ValideContract>();

            while (read.Read())
            {
                ValideContract it = new ValideContract();

                it.id_договор = read["id_договор"].ToString().Trim();

                it.IdContract = Convert.ToInt32(read["id_договор"]);

                it.НомерДоговора = read["НомерДоговора"].ToString().Trim();
                it.Фамилия = read["Фамилия"].ToString().Trim();
                it.Имя = read["Имя"].ToString().Trim();
                it.Отчество = read["Отчество"].ToString().Trim();
                it.ЛьготнаяКатегория = read["ЛьготнаяКатегория"].ToString().Trim();
                it.Сумма = Convert.ToDecimal(read["Сумма"]).ToString("c").Trim();

                //DataRow row = tab.NewRow();
                //row["id_договор"] = read["id_договор"].ToString().Trim();
                //row["НомерДоговора"] = read["НомерДоговора"].ToString().Trim();
                //row["Фамилия"] = read["Фамилия"].ToString().Trim();
                //row["Имя"] = read["Имя"].ToString().Trim();
                //row["Отчество"] = read["Отчество"].ToString().Trim();
                //row["ЛьготнаяКатегория"] = read["ЛьготнаяКатегория"].ToString().Trim();
                //row["Сумма"] = Convert.ToDecimal(read["Сумма"]).ToString("c").Trim();

                if (read["ДатаЗаписиДоговора"] != DBNull.Value)//
                {
                    it.Дата = Convert.ToDateTime(read["ДатаЗаписиДоговора"]).ToShortDateString().Trim();
                }
                if (read["logWrite"] != DBNull.Value)
                {
                    it.КтоЗаписал = read["logWrite"].ToString().Trim();
                }
                if (read["НомерАкта"] != DBNull.Value)
                {
                    it.НомерАкта = read["НомерАкта"].ToString().Trim();
                }
                if (read["ДатаПодписания"] != DBNull.Value)
                {
                    //row["ДатаПодписания"] = Convert.ToDateTime(read["ДатаПодписания"]).ToShortDateString().Trim(); 
                    it.ДатаПодписания = Convert.ToDateTime(read["ДатаПодписания"]).ToShortDateString().Trim();
                }

                if (read["flagАнулирован"] != DBNull.Value)
                {
                    it.flagАнулирован = Convert.ToBoolean(read["flagАнулирован"].ToString());
                }

                //if (read.FieldCount == 12)
                //{
                    //if (read.Equals("flag2019AddWrite") == true)
                    //{
                        if (read["flag2019AddWrite"] != DBNull.Value)
                        {
                            //row["flag2019AddWrite"] = Convert.ToBoolean(read["flag2019AddWrite"].ToString());
                            it.flag2019AddWrite = Convert.ToBoolean(read["flag2019AddWrite"].ToString());
                        }
                    //}
                //}

                if(read["Год"] != null)
                {
                    it.Год = Convert.ToInt32(read["Год"]);
                }



                //tab.Rows.Add(row);
                list.Add(it);
            }

            return list;
        }
    }
}
