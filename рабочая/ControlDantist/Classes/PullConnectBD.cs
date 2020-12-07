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
        public Dictionary<string,string> GetPull(bool flagConnect)
        {
            //Объявим коллекцию для храенения строк подключения
            Dictionary<string, string> pul = new Dictionary<string, string>();

            if (flagConnect == false)
            {
                //добавим Волжский район
                string conVolg = ConfigurationSettings.AppSettings["conVolg"].ToString().Trim();
                pul.Add("Волжский", conVolg);

                //Добавим Заводской район
                string conZav = ConfigurationSettings.AppSettings["conZav"].ToString().Trim();
                pul.Add("Заводской", conZav);

                //Добавим Кировский район
                string conKir = ConfigurationSettings.AppSettings["conKir"].ToString().Trim();
                pul.Add("Кировский", conKir);

                //Добавим Октябрьский район
                string conOkt = ConfigurationSettings.AppSettings["conOkt"].ToString().Trim();
                pul.Add("Октябрьский", conOkt);

                //Добавим Ленинский район
                string conLen = ConfigurationSettings.AppSettings["conLen"].ToString().Trim();
                pul.Add("Ленинский", conLen);

                //Добавим Фрунзенский район
                string conFrun = ConfigurationSettings.AppSettings["conFrun"].ToString().Trim();
                pul.Add("Фрунзенский", conFrun);

                ////Добавим Красноармейский район
                //string conKras = ConfigurationSettings.AppSettings["conKras"].ToString().Trim();
                //pul.Add("Красноармейский", conKras);

                ////Добавим Саратовский район
                //string conSar = ConfigurationSettings.AppSettings["conSar"].ToString().Trim();
                //pul.Add("Саратовский", conSar);

                ////Добавим Татищевский район
                //string conTat = ConfigurationSettings.AppSettings["conTat"].ToString().Trim();
                //pul.Add("Татищевский", conTat);

                ////Добавим Укатериновский район
                //string conEkat = ConfigurationSettings.AppSettings["conEkat"].ToString().Trim();
                //pul.Add("Екатериновский", conEkat);

                ////Добавим Калининский район
                //string conKalin = ConfigurationSettings.AppSettings["conKalin"].ToString().Trim();
                //pul.Add("Калининский", conKalin);

                ////Добавим Лысогорский район
                //string conLisGor = ConfigurationSettings.AppSettings["conLisGor"].ToString().Trim();
                //pul.Add("Лысогорский", conLisGor);

                ////Добавим Петровский район
                //string conPetrovsk = ConfigurationSettings.AppSettings["conPetrovsk"].ToString().Trim();
                //pul.Add("Петровский", conPetrovsk);

                ////Добавим Новые Бурасы район
                //string conNewBur = ConfigurationSettings.AppSettings["conNewBur"].ToString().Trim();
                //pul.Add("НовыеБурасы", conNewBur);

                ////Добавим Аткарск район
                //string conAtcarsk = ConfigurationSettings.AppSettings["conAtcarsk"].ToString().Trim();
                //pul.Add("Аткарск", conAtcarsk);

                ////Добавим Аткарск район
                //string conBasar = ConfigurationSettings.AppSettings["conBasar"].ToString().Trim();
                //pul.Add("Базарный", conBasar);

                ////Добавим Аткарск район
                //string conBaltai = ConfigurationSettings.AppSettings["conBaltai"].ToString().Trim();
                //pul.Add("Балтай", conBaltai);

                ////Воскресенск ТЕСТ.
                //string conАркадакский = ConfigurationSettings.AppSettings["conVoskresensk"].ToString().Trim();
                //pul.Add("Воскресенск", conАркадакский);

                ////////==========Раионы области ==================================
                //string conАлГай = ConfigurationSettings.AppSettings["Александрово-Гайский"].ToString().Trim();
                //pul.Add("Александрово-Гайский", conАлГай);

                //string conАркадак = ConfigurationSettings.AppSettings["Аркадакский"].ToString().Trim();
                //pul.Add("Аркадакский", conАркадак);

                //string conБалаковский = ConfigurationSettings.AppSettings["Балаковский"].ToString().Trim();
                //pul.Add("Балаковский", conБалаковский);


                //string conБалашовский = ConfigurationSettings.AppSettings["Балашовский"].ToString().Trim();
                //pul.Add("Балашовский", conБалашовский);


                //string conВольский = ConfigurationSettings.AppSettings["Вольский"].ToString().Trim();
                //pul.Add("Вольский", conВольский);


                //string conДергачевский = ConfigurationSettings.AppSettings["Дергачевский"].ToString().Trim();
                //pul.Add("Дергачевский", conДергачевский);


                //string conДуховницкий = ConfigurationSettings.AppSettings["Духовницкий"].ToString().Trim();
                //pul.Add("Духовницкий", conДуховницкий);


                //string conЕршовский = ConfigurationSettings.AppSettings["Ершовский"].ToString().Trim();
                //pul.Add("Ершовский", conЕршовский);

                //string conИвантеевский = ConfigurationSettings.AppSettings["Ивантеевский"].ToString().Trim();
                //pul.Add("Ивантеевский", conИвантеевский);

                //string conКраснокутский = ConfigurationSettings.AppSettings["Краснокутский"].ToString().Trim();
                //pul.Add("Краснокутский", conКраснокутский);

                //string conКраснопартизанский = ConfigurationSettings.AppSettings["Краснопартизанский"].ToString().Trim();
                //pul.Add("Краснопартизанский", conКраснопартизанский);

                //string conМарксовский = ConfigurationSettings.AppSettings["Марксовский"].ToString().Trim();
                //pul.Add("Марксовский", conМарксовский);

                //string conНовоузенский = ConfigurationSettings.AppSettings["Новоузенский"].ToString().Trim();
                //pul.Add("Новоузенский", conНовоузенский);

                //string conОзинский = ConfigurationSettings.AppSettings["Озинский"].ToString().Trim();
                //pul.Add("Озинский", conОзинский);


                //string conПерелюбский = ConfigurationSettings.AppSettings["Перелюбский"].ToString().Trim();
                //pul.Add("Перелюбский", conПерелюбский);

                //string conПитерский = ConfigurationSettings.AppSettings["Питерский"].ToString().Trim();
                //pul.Add("Питерский", conПитерский);

                //string conПугачёвский = ConfigurationSettings.AppSettings["Пугачёвский"].ToString().Trim();
                //pul.Add("Пугачёвский", conПугачёвский);

                //string conРовеенский = ConfigurationSettings.AppSettings["Ровеенский"].ToString().Trim();
                //pul.Add("Ровеенский", conРовеенский);

                //string conРомановский = ConfigurationSettings.AppSettings["Романовский"].ToString().Trim();
                //pul.Add("Романовский", conРомановский);

                //string conРтищевский = ConfigurationSettings.AppSettings["Ртищевский"].ToString().Trim();
                //pul.Add("Ртищевский", conРтищевский);

                //string conСамойловский = ConfigurationSettings.AppSettings["Самойловский"].ToString().Trim();
                //pul.Add("Самойловский", conСамойловский);

                //string conСоветский = ConfigurationSettings.AppSettings["Советский"].ToString().Trim();
                //pul.Add("Советский", conСоветский);

                //string conТурковский = ConfigurationSettings.AppSettings["Турковский"].ToString().Trim();
                //pul.Add("Турковский", conТурковский);

                //string conФедоровский = ConfigurationSettings.AppSettings["Федоровский"].ToString().Trim();
                //pul.Add("Федоровский", conФедоровский);

                //string conХвалынский = ConfigurationSettings.AppSettings["Хвалынский"].ToString().Trim();
                //pul.Add("Хвалынский", conХвалынский);

                //string conЭнгельский = ConfigurationSettings.AppSettings["Энгельский"].ToString().Trim();
                //pul.Add("Энгельский", conЭнгельский);

            }


            return pul;
        }
    }
}
