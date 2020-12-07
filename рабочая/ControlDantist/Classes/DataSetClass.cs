using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControlDantist.Classes;
using System.Data;
using System.Data.OleDb;
using System.Data.Linq;

namespace ControlDantist.Classes
{
    public class DataSetClass
    {
        private string sConnect = string.Empty;
        public DataSetClass(string sConn)
        {
            if (sConn != null)
            {
                if (sConn.Length > 0)
                {
                    sConnect = sConn;
                }
                else if(sConn.Length == 0)
                {
                    throw new ExceptionUser("Строка содержит значение NULL");
                }
            }
            else
            {
                throw new ExceptionUser("Строка содержит значение NULL");
            }
        }

        /// <summary>
        /// Выгружает данные из 3-х таблиц: Поликлинники, КлассификаторУслуг, ВидУслуг.
        /// </summary>
        /// <returns></returns>
        public DataSetHospital GetDataHospital()
        {
            // Экземпляр источника данных.
            DataSetHospital dh = new DataSetHospital();
            dh.StringConnection = sConnect;

            using (OleDbConnection con = new OleDbConnection(sConnect))
            {
                // Подключимся к БД.
                con.Open();
                Console.WriteLine("Подключение прошло успешно!");

                // Получим id поликлинники.
                string queryIdHosp = "select id_поликлинника from Поликлинника";

                OleDbCommand comHosp = new OleDbCommand(queryIdHosp, con);
                OleDbDataReader readHosp = comHosp.ExecuteReader();

                // Запишем id поликлинники в наш класс источник данных.
                while (readHosp.Read())
                {
                    dh.IdHospital = Convert.ToInt32(readHosp["id_поликлинника"]);
                }

                

                // Получим Содержание таблицы классификатор услуг.
                List<КлассификаторУслуг> listKL = new List<КлассификаторУслуг>();

                string queryKL = "select id_кодУслуги,КлассификаторУслуги from КлассификаторУслуги";

                OleDbCommand comKL = new OleDbCommand(queryKL, con);
                OleDbDataReader readKL = comKL.ExecuteReader();

                while (readKL.Read())
                {
                    КлассификаторУслуг kl = new КлассификаторУслуг();
                    kl.Id_КодУслуг = Convert.ToInt32(readKL["id_кодУслуги"]);
                    kl.КлассификаторУслуг1 = readKL["КлассификаторУслуги"].ToString().Trim();

                    listKL.Add(kl);
                }

                // Запишем в источник данных таблицу классификатор услуг.
                dh.ТаблицаКлассификаторУслуг = listKL;

                List<ВидУслуг> listVY = new List<ВидУслуг>();
                
                // Получим содержание таблицы ВидУслуг.
                string queryVY = "select id_услуги,ВидУслуги,Цена,id_поликлинника,НомерПоПеречню,Выбрать,id_кодУслуги,ТехЛист from ВидУслуги";

               
                OleDbCommand comVY = new OleDbCommand(queryVY, con);
                OleDbDataReader readVY = comVY.ExecuteReader();

                while (readVY.Read())
                {
                    ВидУслуг vy = new ВидУслуг();
                    vy.Id_ВидУслуг = Convert.ToInt32(readVY["id_услуги"]);
                    vy.Id_КодУслуги = Convert.ToInt32(readVY["id_кодУслуги"]);
                    vy.Id_Поликлинники = Convert.ToInt32(readVY["id_поликлинника"]);
                    vy.ВидУслуги1 = readVY["ВидУслуги"].ToString().Trim();
                    vy.Цена = Convert.ToDecimal(readVY["Цена"]);
                    vy.НомерПоПеречню = readVY["НомерПоПеречню"].ToString().Trim();
                    vy.Выбрать = Convert.ToBoolean(readVY["Выбрать"]);
                    vy.ТехЛист = readVY["ТехЛист"].ToString().Trim();

                    listVY.Add(vy);
                }

                // Запишем в источник данных таблицу ВидУслуг.
                dh.ТаблицаВидУслуг = listVY;

            
            }


            return dh;
        }
                
    }
}
