using System;
using System.Collections.Generic;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Описывает строку НаименованиеРайона
    /// </summary>
    public class НаименованиеРайона
    {
        private int id;
        private string name;
        private bool flag;
        private int idTerrOrgan;

        /// <summary>
        /// id терр органа
        /// </summary>
        public int IdТеррОргана
        {
            get
            {
                return idTerrOrgan;
            }
            set
            {
                idTerrOrgan = value;
            }
        }


        
        public bool Flag
        {
            get
            {
                return flag;
            }
            set
            {
                flag = value;
            }
        }

        public string Наименование
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Наименование терр органа
        /// </summary>
        public string НазваниеТеррОргана { get; set; }


    }
}
