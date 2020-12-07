using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Строка отчёта по статистике заключённых договоров в поликлиннике.
    /// </summary>
    public class StatisticaHospital
    {
        /// <summary>
        /// Ячейка A.
        /// </summary>
        public string НомерПП { get; set; }

        /// <summary>
        /// Наименование поликлинники. Ячейка B.
        /// </summary>
        public string Поликлинника { get; set; }

        ///// <summary>
        ///// Сумма.
        ///// </summary>
        //public decimal Сумма { get; set; }

        ///// <summary>
        ///// Количество договоров.
        ///// </summary>
        //public int Количество { get; set; }
        /// <summary>
        /// Ячейка C.
        /// </summary>
        public int КоличествоПоликлинник { get; set; }

        /// <summary>
        /// Ячейка D.
        /// </summary>
        public int ПропускнаяСпособность { get; set; }

        /// <summary>
        /// Ячейка E.
        /// </summary>
        public decimal ПотребностьДенежныхсредств { get; set; }

        /// <summary>
        /// Ячейка F.
        /// </summary>
        public string ЧисленностьЛьготниковСостоящихНаУчётеВсего { get; set; }

        /// <summary>
        /// Ячейка G.
        /// </summary>
        public string ЧисленностьГражданОставшихсяНаУчёте2013 { get; set; }

        /// <summary>
        /// Ячейка H.
        /// </summary>
        public string ЧисленностьГражданВставшиеНаУчёте2014 { get; set; }

        /// <summary>
        /// Ячейка I.
        /// </summary>
        //public int ЗаключённыеДоговораКоличество2013 { get; set; }

        /// <summary>
        /// Ячейка J.
        /// </summary>
        //public decimal ЗаключённыеДоговораСумма2013 { get; set; }

        /// <summary>
        /// Ячейка I.
        /// </summary>
        public int ЗаключённыеДоговораНеОплатаКоличество2013 { get; set; }

        /// <summary>
        /// Ячейка J. Договора которые заключили в 2013 г. но по котором не прошла оплата на 1.01.2014 г.
        /// </summary>
        public decimal ЗаключённыеДоговораНеОплатаСумма2013 { get; set; }

        /// <summary>
        /// Ячейка K. Договора которые заключили в 2013 г. но по котором не прошла оплата на 1.01.2014 г.
        /// </summary>
        public decimal ДоговораЗаключённыеКоличество2014 { get; set; }

        /// <summary>
        /// Ячейка L.
        /// </summary>
        public decimal ДоговораЗаключённыеСумма2014 { get; set; }

        /// <summary>
        /// Ячейка M.
        /// </summary>
        public int ВсегоКоличествоДоговоров { get; set; }

        /// <summary>
        /// Ячейка N.
        /// </summary>
        public int ВсегоКоличествоДоговоровЗа2013 { get; set; }

        /// <summary>
        /// Ячейка O.
        /// </summary>
        public int ВсегоКоличествоДоговоровЗа2014 { get; set; }

        /// <summary>
        /// Ячейка P.
        /// </summary>
        public decimal ВсегоВыплачено { get; set; }


        /// <summary>
        /// Ячейка Q.
        /// </summary>
        public decimal СуммаВыплаченнаяПоДоговорам2013 { get; set; }

        /// <summary>
        /// Ячейка R.
        /// </summary>
        public decimal СуммаВыплаченнаяПоДоговорам2014 { get; set; }

        /// <summary>
        /// Ячейка S.
        /// </summary>
        public string Лимиты2014 { get; set; }

    }
}
