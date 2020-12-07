using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ControlDantist.FilesRegistr
{
    public class FileRegistrCopy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">имя файла реестра</param>
        /// <param name="NewFIleName">Новое название файла реестра на сервере</param>
        /// <returns></returns>
        public bool FIleRegistrToServer(string fileName, string NewFIleName,string newFileRegistrPostCopy)
        {
            string patch = @"\\10.159.102.100\DantistRepositoryFiles\";

            string fileNameNow = patch + NewFIleName;

            if(File.Exists(fileNameNow) == true)
            {
                File.Delete(fileNameNow);

            }

            //File.Move(fileName, fileNameNow);

            // Скопируем файл на сервер.
            File.Copy(fileName, fileNameNow);

            // Переименуем файл откуда мы его копирования (например флэшка).
            File.Move(fileName, newFileRegistrPostCopy);

            // Проеверим файл на сервере есть или нет.
            return FIleRegistrServerExists(fileNameNow);
        }

        private bool FIleRegistrServerExists(string fileName)
        {
            bool flagExists = false;

            if(File.Exists(fileName))
            {
                flagExists = true;
            }

            return flagExists;
        }
    }
}
