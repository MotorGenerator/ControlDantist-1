using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using ControlDantist.Repozirories;
using Microsoft.Office.Interop.Word;

namespace ControlDantist.ReportVipNet
{
    public class WordPageVipNet
    {
        private int idRegion = 0;
        
        public WordPageVipNet(int idRegion)
        {
            this.idRegion = idRegion;
        }

        public void CreateLetter(string fileName)
        {
            // Копируем в папку указанную в диологовом окне шаблон письма VipNet.
            DirectoryInfo dir = new DirectoryInfo(fileName);
            string dirPatch = dir.FullName;

            
            // Получаем путь к указаной диреткории.
            string pathDirectory = Path.GetDirectoryName(fileName);//.GetFileNameWithoutExtension(fileName);

            // Название файла без расширения.
            string nameFile = Path.GetFileNameWithoutExtension(fileName);

            string fileNameDoc = pathDirectory + "/" + nameFile;

            try
            {
                UnitWork unit = new UnitWork();
                var organization = unit.OrganizationVipNet.GetOrganization(this.idRegion);

                if (organization != null)
                {

                    //Скопируем шаблон в папку Документы
                    FileInfo fn = new FileInfo(System.Windows.Forms.Application.StartupPath + @"\Шаблон\Шаблон VipNet.doc");
                    //fn.CopyTo(System.Windows.Forms.Application.StartupPath + @"\Документы\" + fName + ".doc", true);
                    fn.CopyTo(fileNameDoc + ".doc", true);

                    ////Создаём новый Word.Application
                    Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();

                    //Загружаем документ
                    Microsoft.Office.Interop.Word.Document doc = null;

                    object fileNameOb = fileNameDoc + ".doc";
                    object falseValue = false;
                    object trueValue = true;
                    object missing = Type.Missing;

                    doc = app.Documents.Open(ref fileNameOb, ref missing, ref trueValue,
                    ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing);

                    //зададим левый отступ
                    doc.PageSetup.LeftMargin = 40f;

                    ////Номер договора
                    object wdrepl = WdReplace.wdReplaceAll;
                    //object searchtxt = "GreetingLine";
                    object searchtxt = "Region";
                    object newtxt = (object)organization.NameOrganization.Trim();
                    //object frwd = true;
                    object frwd = false;
                    doc.Content.Find.Execute(ref searchtxt, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd, ref missing, ref missing, ref newtxt, ref wdrepl, ref missing, ref missing,
                    ref missing, ref missing);

                    object wdrepl5 = WdReplace.wdReplaceAll;//39
                    //object searchtxt = "GreetingLine";
                    object searchtxt5 = "NameFile";
                    object newtxt5 = (object)nameFile + ".xls";
                    //object frwd = true;
                    object frwd5 = false;
                    doc.Content.Find.Execute(ref searchtxt5, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd5, ref missing, ref missing, ref newtxt5, ref wdrepl5, ref missing, ref missing,
                    ref missing, ref missing);


                    object wdrepl6 = WdReplace.wdReplaceAll;//39
                    //object searchtxt = "GreetingLine";
                    object searchtxt6 = "uspn";
                    object newtxt6 = (object)organization.NameOrganization.Trim();
                    //object frwd = true;
                    object frwd6 = false;
                    doc.Content.Find.Execute(ref searchtxt6, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd6, ref missing, ref missing, ref newtxt6, ref wdrepl6, ref missing, ref missing,
                    ref missing, ref missing);

                    string fio = organization.Chief.ToString().Trim();

                    object wdrepl7 = WdReplace.wdReplaceAll;//39
                    //object searchtxt = "GreetingLine";
                    object searchtxt7 = "chief";
                    object newtxt7 = (object)fio;
                    //object frwd = true;
                    object frwd7 = false;
                    doc.Content.Find.Execute(ref searchtxt7, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd7, ref missing, ref missing, ref newtxt7, ref wdrepl7, ref missing, ref missing,
                    ref missing, ref missing);

                    object wdrepl8 = WdReplace.wdReplaceAll;//39
                    //object searchtxt = "GreetingLine";
                    object searchtxt8 = "dateToday";

                    // Установим текущую дату.
                    DateTime dateTime = DateTime.Now;

                    // Установим текущую дату.
                    object newtxt8 = (object)dateTime.ToShortDateString();
                    //object frwd = true;
                    object frwd8 = false;
                    doc.Content.Find.Execute(ref searchtxt8, ref missing, ref missing, ref missing, ref missing, ref missing, ref frwd8, ref missing, ref missing, ref newtxt8, ref wdrepl8, ref missing, ref missing,
                    ref missing, ref missing);

                    //uspn 

                    app.Visible = true;

                }
            }
            catch
            {
                MessageBox.Show("Документ с таки именем уже существует");
            }
        }
    }
}
