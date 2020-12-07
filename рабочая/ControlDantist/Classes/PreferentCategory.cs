using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlDantist.Classes
{
    /// <summary>
    /// Вспомогательный класс содержащий льготные категории из сервера и из файла.
    /// </summary>
    public class PreferentCategory
    {
        /// <summary>
        /// Льготная категория из базы данных.
        /// </summary>
        public string PCategoryServer { get; set; }

        /// <summary>
        /// Льготная категория из файла.
        /// </summary>
        public string PActegoryFile { get; set; }
    }
}
