using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Возвращает название госпиталя
    /// </summary>
    public class NameHosp
    {
        /// <summary>
        /// id наименования поликлинники
        /// </summary>
        private string cod;
        public NameHosp(string кодПоликлинники)
        {
            cod = кодПоликлинники;
        }

        /// <summary>
        /// Возвращает наименование поликлинники.
        /// </summary>
        /// <returns></returns>
        public string GetNameHosp()
        {
            string name = string.Empty;
            switch (cod)
            {
                case "1":
                    name = "МУЗ 'Стоматологическая поликлиника № 1'  комитета здравоохранения администрации муниципального образования «Город Саратов»";
                    break;
                case "2":
                    name = "МАУЗ «Стоматологическая поликлиника № 2»  ";
                    break;
                case "3":
                    name = "МАУЗ 'Стоматологическая поликлиника № 3' ";
                    break;
                case "5":
                    name = "МУЗ 'Стоматологическая поликлиника № 5'  комитета здравоохранения администрации муниципального образования «Город Саратов»";
                    break;
                case "6":
                    name = "МУЗ 'Стоматологическая поликлиника № 6' комитета здравоохранения администрации муниципального образования «Город Саратов»";
                    break;
                case "8":
                    name = "МУЗ 'Стоматологическая поликлиника № 8'комитета здравоохранения администрации муниципального образования «Город Саратов»";
                    break;
                case "20":
                    name = "ГУЗ  «Городская поликлиника №20»";
                    break;
                case "ОГВВ":
                    name = "ГУЗ «Областной госпиталь для ветеранов войн» Управления делами Правительства области";
                    break;
                case "ЕЦРБ":
                    name = "ГУЗ СО «Екатериновская центральная районная больница»";
                    break;
                case "ЛЦРБ":
                    name = "ГУЗ СО «Лысогорская центральная районная больница»";
                    break;
                case "СпКмрСО":
                    name = "МУП «Стоматологическая поликлиника Красноармейского муниципального района Саратовской области»";
                    break;
                case "Калиниская ЦРБ":
                    name = "ГУЗ СО «Калининская центральная районная больница»";
                    break;
                case "АЦРБ":
                    name = "ГУЗ СО «Аткарская центральная районная больница»";
                    break;
                case "ЗАТО Светлый":
                    name = "ГУЗ СО «Медико-санитарная часть городского округа ЗАТО Светлый»";
                    break;
                case "ПЦРБ":
                    name = "ГУЗ СО «Петровская центральная районная больница»";
                    break;
                case "БЦРБ":
                    name = "ГБУЗ СО «Балтайская центральная районная больница»";
                    break;
                case "БКЦРБ":
                    name = "ГУЗ СО «Базарно-Карабулакская центральная районная больница»";
                    break;
                case "НБЦРБ":
                    name = "ГУЗ СО «Новобурасская центральная районная больница»";
                    break;
                case "Татищевская ЦРБ":
                    name = "ГУЗ СО «Татищевская центральная районная больница»";
                    break;
                case "Лысые горы цены 01.10.2013":
                    name = "ГУЗ СО «Лысогорская центральная районная больница»";
                    break;
                //case "1":
                //name = "1 стоматология";
                //break;
            }

            return name;
        }

        /// <summary>
        /// Возвращает количество договоров на 30.08.2013 г.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            int sum = 0;
            switch (cod)
            {
                case "1":
                    sum = 751;
                    //sum = 753;
                    break;
                case "2":
                    sum = 1595;
                    break;
                case "3":
                    sum = 916;
                    break;
                case "5":
                    sum = 670;
                    break;
                case "6":
                    sum = 659;
                    break;
                case "8":
                    sum = 434;
                    break;
                case "20":
                    sum = 0;
                    break;
                case "ОГВВ":
                    sum = 83;
                    break;
                case "ЕЦРБ":
                    sum = 80;
                    break;
                case "ЛЦРБ":
                    sum = 52;
                    break;
                case "СпКмрСО":
                    sum = 248;
                    break;
                case "Калиниская ЦРБ":
                    sum = 221;
                    break;
                case "АЦРБ":
                    sum = 115;
                    break;
                case "ЗАТО Светлый":
                    sum = 42;
                    break;
                case "ПЦРБ":
                    sum = 188;
                    break;
                case "БЦРБ":
                    sum = 79;
                    break;
                case "БКЦРБ":
                    sum = 171;
                    break;
                case "НБЦРБ":
                    sum = 107;
                    break;


            }
            return sum;
        }


        /// <summary>
        /// Возвращает сумму на 30.08.2013 г.
        /// </summary>
        /// <returns></returns>
        public decimal GetSum()
        {
            decimal name = 0.0m;
            switch (cod)
            {
                case "1":
                    name = 6084020m;
                    break;
                case "2":
                    name = 19931630m;
                    break;
                case "3":
                    name = 9689750m;
                    break;
                case "5":
                    name = 8591790m;
                    break;
                case "6":
                    name = 8885560m;
                    break;
                case "8":
                    name = 4362220m;
                    break;
                case "20":
                    name = 0.0m;
                    break;
                case "ОГВВ":
                    name = 428660m;
                    break;
                case "ЕЦРБ":
                    name = 551370m;
                    break;
                case "ЛЦРБ":
                    //name = 682460m;
                    name = 749100m;
                    break;
                case "СпКмрСО":
                    //name = 2623600m;
                    name = 2707290m;
                    break;
                case "Калиниская ЦРБ":
                    name = 1498790m;
                    break;
                case "АЦРБ":
                    name = 714000m;
                    break;
                case "ЗАТО Светлый":
                    name = 670250m;
                    break;
                case "ПЦРБ":
                    name = 1282260m;
                    break;
                case "БЦРБ":
                    name = 415740m;
                    break;
                case "БКЦРБ":
                    name = 1540140m;
                    break;
                case "НБЦРБ":
                    name = 693630m;
                    break;
                //case "1":
                //name = "1 стоматология";
                //break;
                //case "1":
                //name = "1 стоматология";
                //break;
            }

            return name;
        }
    }
}
