using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    public class ListRegion
    {
        private Dictionary<int, string> list;

        public ListRegion()
        {
            list = new Dictionary<int, string>();
            list.Add(0, "--Район не выбран--"); // Пока оставим.
            list.Add(1, "Аткарский");
            list.Add(2, "Аркадакский");
            list.Add(3, "Александровогайский");
            list.Add(4, "Базарно-Карабулакский");
            list.Add(5, "Балаковский");
            list.Add(6, "Балашовский");
            list.Add(7, "Балтайский");
            list.Add(8, "Вольский");
            list.Add(9, "Воскресенский");
            list.Add(10, "Дергачевский");
            list.Add(11, "Духовницкий");
            list.Add(12, "Екатериновский");
            list.Add(13, "Ершовский");
            list.Add(14, "Ивантеевский");
            list.Add(15, "Калининский");
            list.Add(16, "Краснокутский");
            list.Add(17, "Краснопартизанский");
            list.Add(18, "Красноармейский");
            list.Add(19, "Лысогорский");
            list.Add(20, "Марксовский");
            list.Add(21, "Новобурасский");
            list.Add(22, "Новоузенский");
            list.Add(23, "Озинский");
            list.Add(24, "Перелюбский");
            list.Add(25, "Петровский");
            list.Add(26, "Питерский");
            list.Add(27, "Пугачевский");
            list.Add(28, "Ровенский");
            list.Add(29, "Романовский");
            list.Add(30, "Ртищевский");
            list.Add(31, "Самойловский");
            list.Add(32, "Саратовский");
            list.Add(33, "Советский");
            list.Add(34, "Татищевский");
            list.Add(36, "Турковский");
            list.Add(37, "Федоровский");
            list.Add(38, "Хвалынский");
            list.Add(39, "Энгельский, г. Энгельс");
            list.Add(40, "Саратов");
            list.Add(41, "Заводской");
            list.Add(42, "Кировский");
            list.Add(43, "Ленинский");
            list.Add(44, "Октябрьский");
            list.Add(45, "Фрунзенский");
            list.Add(46, "Волжский");

        }

        public Dictionary<int, string> Regions()
        {
            return list;
        }

        /// <summary>
        /// Поиск района по обалсти.
        /// </summary>
        /// <param name="idRegion"></param>
        /// <returns></returns>
        public string FindRegion(int idRegion)
        {

            return list[idRegion].Trim();
            //string RegionName = string.Empty;

            //foreach (int id in list.Keys)
            //{
            //    if (id == idRegion)
            //    {


            //        break;
            //    }
            //}

            //return RegionName;
        }
    }
}
