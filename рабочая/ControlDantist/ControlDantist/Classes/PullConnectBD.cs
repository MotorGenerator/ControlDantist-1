using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Пул строк подключения к ЭСРН
    /// </summary>
    class PullConnectBD
    {
        public Dictionary<string,string> GetPull()
        {
            //Объявим коллекцию для храенения строк подключения
            Dictionary<string, string> pul = new Dictionary<string, string>();

            //добавим Волжский район
            string conVolg = ConfigurationSettings.AppSettings["conVolg"].ToString().Trim();
            pul.Add("Волжский",conVolg);

            //Добавим Заводской район
            string conZav = ConfigurationSettings.AppSettings["conZav"].ToString().Trim();
            pul.Add("Заводской",conZav);

            //Добавим Кировский район
            string conKir = ConfigurationSettings.AppSettings["conKir"].ToString().Trim();
            pul.Add("Кировский",conKir);

            //Добавим Октябрьский район
            string conOkt = ConfigurationSettings.AppSettings["conOkt"].ToString().Trim();
            pul.Add("Октябрьский",conOkt);

            //Добавим Ленинский район
            string conLen = ConfigurationSettings.AppSettings["conLen"].ToString().Trim();
            pul.Add("Ленинский",conLen);

            //Добавим Фрунзенский район
            string conFrun = ConfigurationSettings.AppSettings["conFrun"].ToString().Trim();
            pul.Add("Фрунзенский",conFrun);

            //Добавим Красноармейский район
            string conKras = ConfigurationSettings.AppSettings["conKras"].ToString().Trim();
            pul.Add("Красноармейский",conKras);

            //Добавим Саратовский район
            string conSar = ConfigurationSettings.AppSettings["conSar"].ToString().Trim();
            pul.Add("Саратовский",conSar);

            //Добавим Татищевский район
            string conTat = ConfigurationSettings.AppSettings["conTat"].ToString().Trim();
            pul.Add("Татищевский",conTat);

            //Добавим Укатериновский район
            string conEkat = ConfigurationSettings.AppSettings["conEkat"].ToString().Trim();
            pul.Add("Екатериновский",conEkat);

            //Добавим Калининский район
            string conKalin = ConfigurationSettings.AppSettings["conKalin"].ToString().Trim();
            pul.Add("Калининский",conKalin);

            //Добавим Лысогорский район
            string conLisGor = ConfigurationSettings.AppSettings["conLisGor"].ToString().Trim();
            pul.Add("Лысогорский",conLisGor);

            return pul;
        }
    }
}
