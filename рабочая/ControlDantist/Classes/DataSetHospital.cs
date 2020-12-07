using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Класс для хранения данных о поликлиннике необходимых для выполнения редактирования наименования услуг и их стоимости.
    /// </summary>
    public class DataSetHospital
    {
        /// <summary>
        ///  id поликлинники.
        /// </summary>
        public int IdHospital { get; set; }

        /// <summary>
        /// Список из таблицы КлассификаторУслуг.
        /// </summary>
        public List<КлассификаторУслуг> ТаблицаКлассификаторУслуг { get; set; }

        /// <summary>
        /// Список из таблицы ВидУслуг.
        /// </summary>
        public List<ВидУслуг> ТаблицаВидУслуг { get; set; }

        /// <summary>
        /// Хранит строку подключения к БД.
        /// </summary>
        public string StringConnection { get; set; }
    }
}
