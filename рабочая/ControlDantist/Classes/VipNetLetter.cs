using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace ControlDantist.Classes
{
    public class VipNetLetter
    {
        // Переменная для хранения id района области.
        private int _idRegion = 0;

        // Переменная для хранения названия файла письма.
        string nameFileRegistr = string.Empty;

        public VipNetLetter(int idRegion, string fileName)
        {
            _idRegion = idRegion;
            nameFileRegistr = fileName;
        }

        public void CreateLetter(out string nameRegion)
        {

            nameRegion = string.Empty;
            // Получим все данные для написания письма.

            string query = "SELECT dbo.NameOrganizationVipNet.NameOrganization,dbo.NameOrganizationVipNet.Chief, " +
                           " dbo.РайонОбласти.NameRegion  FROM NameOrganizationVipNet " +
                           " INNER JOIN dbo.РайонОбласти " +
                           " ON dbo.РайонОбласти.idRegion = dbo.NameOrganizationVipNet.idRegion " +
                           " where dbo.РайонОбласти.idRegion = "+ _idRegion +" ";

            DataTable tabLetter = ТаблицаБД.GetTableSQL(query, "NameOrganizationVipNet");

            if (tabLetter.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Нет данных для формирования письма VipNet");

                return;
            }

            // Получим исходные данные для письма.
            string Region = tabLetter.Rows[0]["NameRegion"].ToString().Trim();

            nameRegion = Region;

            string NameOrganization = tabLetter.Rows[0]["NameOrganization"].ToString().Trim();

            string Chief = tabLetter.Rows[0]["Chief"].ToString().Trim();

            //if (Directory.Exists(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet") == false)
            //{
            //    // Создадим папку.
            //    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet");

            //    DirectoryInfo dirInfo = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet");

            //    foreach (FileInfo file in dirInfo.GetFiles())
            //    {
            //        file.Delete();
            //    }
            //}


            try
            {

                //DirectoryInfo dirInfo = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet");

                //foreach (FileInfo file in dirInfo.GetFiles())
                //{
                //    file.Delete();
                //}

                //Скопируем шаблон в папку Документы
                FileInfo fn = new FileInfo(System.Windows.Forms.Application.StartupPath + @"\Шаблон\Шаблон VipNet.doc");
                //fn.CopyTo(System.Windows.Forms.Application.StartupPath + @"\Документы\" + fName + ".doc", true);
                fn.CopyTo(System.Windows.Forms.Application.StartupPath + @"\Документы VipNet\Шаблон VipNet.doc_" + Region + ".doc", true);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Возможно у вас уже открыт договор с этим льготником. Закройте этот договор.");
                return;
            }

            string filName = System.Windows.Forms.Application.StartupPath + @"\Документы VipNet\Шаблон VipNet.doc_" + Region + ".doc";



            //System.Diagnostics.Process.Start("C:/asdasd.xls");

            //Создаём новый Word.Application
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

            //Загружаем документ
            Microsoft.Office.Interop.Word.Document doc = null;

            object fileName = filName;
            object falseValue = false;
            object trueValue = true;
            object missing = Type.Missing;
            object writePasswordDocument = "12A86Asd";

            // Откроем существующий документ.
            doc = app.Documents.Open(ref fileName, ref missing, ref trueValue,
            ref missing, ref missing, ref missing, ref missing, ref writePasswordDocument,
            ref missing, ref missing, ref missing, ref missing, ref trueValue,
            ref missing, ref missing, ref missing);

            //Наименование района области.
            object wdrepl1 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt1 = "Region";
            object newtxt1 = (object)Region;
            //object frwd = true;
            object frwd1 = false;
            doc.Content.Find.Execute(ref searchtxt1, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd1, ref missing, ref missing, ref newtxt1, ref wdrepl1, ref missing, ref missing,
            ref missing, ref missing);

            // Название реестра.
            object wdrepl37 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt37 = "NameFile";
            object newtxt37 = (object)nameFileRegistr;
            //object frwd = true;
            object frwd37 = false;
            doc.Content.Find.Execute(ref searchtxt37, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd37, ref missing, ref missing, ref newtxt37, ref wdrepl37, ref missing, ref missing,
            ref missing, ref missing);

            //Наименование орагнизации получателя.
            object wdrepl3 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt3 = "uspn";
            object newtxt3 = (object)NameOrganization;
            //object frwd = true;
            object frwd3 = false;
            doc.Content.Find.Execute(ref searchtxt3, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd3, ref missing, ref missing, ref newtxt3, ref wdrepl3, ref missing, ref missing,
            ref missing, ref missing);

            // ФИО руководителя организации.
            object wdrepl4 = WdReplace.wdReplaceAll;
            //object searchtxt = "GreetingLine";
            object searchtxt4 = "chief";
            object newtxt4 = (object)Chief;
            //object frwd = true;
            object frwd4 = false;
            doc.Content.Find.Execute(ref searchtxt4, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd4, ref missing, ref missing, ref newtxt4, ref wdrepl4, ref missing, ref missing,
            ref missing, ref missing);


            app.Visible = true;
            // Закроем текущий документ.
            //doc.Close();

            // Закроем процесс Word.
           // app.Quit();

            //doc = null;

        }
    }
}
