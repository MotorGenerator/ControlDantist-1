using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ControlDantist.ErrorHandlid
{
    public class WriteErrorFileText : IWriteError
    {
        // Переменная для ххранения пути к файлу.
        private string writePath = string.Empty;

        // Переменная для хранения сообщения об ошибки которое пишется в текстовый файл.
        private string textExeption = string.Empty;

        public WriteErrorFileText(string writePath, string textExeption)
        {
            this.writePath = writePath;
            this.textExeption = textExeption;
        }

        public void Write()
        {
            using (StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(textExeption);
            }
        }
    }
}
