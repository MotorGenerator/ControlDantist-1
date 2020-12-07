using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Classes;
using ControlDantist.BalanceContract;
using ControlDantist.UnpaidStatistic;

namespace ControlDantist
{
    public partial class FormInv : Form
    {
        // Источник данных.
        private DataClasses4DataContext dc;

        // Переменная для хранения данных для печати.
        private List<ViewИнвентаризация> listPrint;

        public FormInv()
        {
            InitializeComponent();

            dc = new DataClasses4DataContext();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormInv_Load(object sender, EventArgs e)
        {
            // Загрузим список поликлинник.
            this.cmbHjspital.DataSource = dc.ПоликлинникиИнн.Select(w => w).ToList();
            cmbHjspital.DisplayMember = "F2";
            cmbHjspital.ValueMember = "F3";

            // Загрузим список льготных категорий.
            this.comboBox1.DataSource = dc.ЛьготнаяКатегория.Select(w => w.ЛьготнаяКатегория1).ToList();
            comboBox1.DisplayMember = "ЛьготнаяКатегория";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            

            string инн = dc.ПоликлинникиИнн.Where(x => x.F2 == this.cmbHjspital.Text.Trim()).Select(x => x).First().F3.Value.ToString().Trim();
            
            // Если сняты галочки с льготной категории и с наличия акта.
            if (chbЛК.Checked == false && chbАкт.Checked == false)
            {

                // Выводим отчёт только поликлиннику и диапазон дат.
                listPrint = dc.ViewИнвентаризация.Where(w => w.ИНН.Trim() == инн.Trim() && w.ДатаЗаписиДоговора >= this.dateTimePicker1.Value && w.ДатаЗаписиДоговора <= this.dateTimePicker2.Value).Select(w => w).ToList();

            }

            // Если выбрана льготная категория.
            if (chbЛК.Checked == true && chbАкт.Checked == false)
            {
                // Выводим поликлиннику с диапазоном дат и с льготной категорией.
                listPrint = dc.ViewИнвентаризация.Where(w => w.ИНН.Trim() == инн.Trim() && w.ДатаЗаписиДоговора >= this.dateTimePicker1.Value && w.ДатаЗаписиДоговора <= this.dateTimePicker2.Value && w.ЛьготнаяКатегория.Trim().ToLower() == this.comboBox1.Text.Trim().ToLower()).Select(w => w).ToList();

            }

            // Если выбрана льготная категория.
            if (chbЛК.Checked == false && chbАкт.Checked == true)
            {
                // Выводим поликлиннику с диапазоном дат и c наличием акта.
                if (rbНаличиеАкта.Checked == true)
                {
                    listPrint = dc.ViewИнвентаризация.Where(w => w.ИНН.Trim() == инн.Trim() && w.ДатаЗаписиДоговора >= this.dateTimePicker1.Value && w.ДатаЗаписиДоговора <= this.dateTimePicker2.Value  && w.ФлагНаличияАкта == true).Select(w => w).ToList();
                }

                // Выводим поликлиннику с диапазоном дат и без акта.
                if (rbОтсутствиеАкта.Checked == true)
                {
                    listPrint = dc.ViewИнвентаризация.Where(w => w.ИНН.ToLower().Trim() == инн.ToLower().Trim() && w.ДатаЗаписиДоговора >= this.dateTimePicker1.Value && w.ДатаЗаписиДоговора <= this.dateTimePicker2.Value  && w.ФлагНаличияАкта == false).Select(w => w).ToList();
                }
            }

            // Если отмечены все галочки.
            if (chbЛК.Checked == true && chbАкт.Checked == true)
            {
                // Выводим поликлиннику с диапазоном дат и c наличием акта.
                if (rbНаличиеАкта.Checked == true)
                {
                    listPrint = dc.ViewИнвентаризация.Where(w => w.ИНН.Trim() == инн.Trim() && w.ДатаЗаписиДоговора >= this.dateTimePicker1.Value && w.ДатаЗаписиДоговора <= this.dateTimePicker2.Value  && w.ЛьготнаяКатегория.Trim().ToLower() == this.comboBox1.Text.Trim().ToLower() && w.ФлагНаличияАкта == true).Select(w => w).ToList();
                }

                // Выводим поликлиннику с диапазоном дат и без акта.
                if (rbОтсутствиеАкта.Checked == true)
                {
                    listPrint = dc.ViewИнвентаризация.Where(w => w.ИНН.Trim() == инн.Trim() && w.ДатаЗаписиДоговора >= this.dateTimePicker1.Value && w.ДатаЗаписиДоговора <= this.dateTimePicker2.Value  && w.ЛьготнаяКатегория.Trim().ToLower() == this.comboBox1.Text.Trim().ToLower() && w.ФлагНаличияАкта == false).Select(w => w).ToList();
                }
            }

            
            // Если данных для отчёта нет тогда выходим из процедуры.
            if (listPrint.Count == 0)
            {

                MessageBox.Show("Для отчёта нет данных","Внимание",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);

                return;
            }

            // Выведим на бумагу.
            GenerateExcelFile excel = new GenerateExcelFile();

            // Создадим книгу.
            excel.ExcelWorkbook();

            // Создадим лист.
            excel.ExcelWorksheet(1);

            string льготнаяКатегория = string.Empty;
            string наличиеАкта = string.Empty;

            if(chbЛК.Checked == true)
            {

               льготнаяКатегория = "Льготная категория - " +  this.comboBox1.Text.Trim();
            }

            if(rbНаличиеАкта.Checked == true)
            {
                наличиеАкта = "с актом выполненных работ";
            }

            if(this.rbОтсутствиеАкта.Checked == true)
            {
                наличиеАкта = "без акта выполненных работ";
            }

            string cellA1 = "a1";
            excel.ExcelCellsAddValueFontBold(cellA1, "Отчёт по - " + this.cmbHjspital.Text.Trim() + " в диапазоне от " + this.dateTimePicker1.Value.ToShortDateString() + " до " + this.dateTimePicker2.Value.ToShortDateString() + " " + льготнаяКатегория + " " + наличиеАкта);

            // Выведим шапку таблицы.
            excel.ExcelCellsAddValueFontBold("a3", "N п/п");
            excel.ExcelCellsAddValueFontBold("b3", "Фамилия");
            excel.ExcelCellsAddValueFontBold("c3", "Имя");
            excel.ExcelCellsAddValueFontBold("d3", "Отчество");
            excel.ExcelCellsAddValueFontBold("e3", "Номер договора");
            excel.ExcelCellsAddValueFontBold("f3", "Дата договора");
            excel.ExcelCellsAddValueFontBold("g3", "Номер акта");
            excel.ExcelCellsAddValueFontBold("h3", "Дата акта");
            excel.ExcelCellsAddValueFontBold("i3", "Льготная категория");
            excel.ExcelCellsAddValueFontBold("j3", "Стоимость");
            excel.ExcelCellsAddValueFontBold("k3", "Дата записи договора");
            excel.ExcelCellsAddValueFontBold("l3", "Сумма проекта договора");

            // Индекс элемента в списке данных для отчёта.
            int k = 0;

            // Счётчик порядкового номера.
            int count = 4;

            // Номер по порядку.
            int countPP = 1;

            foreach (var item in listPrint)
            {
                // Сгенерируем содержимое письма.
                for (int i = 1; i <= 12; i++)
                {

                    if (i == 1)
                    {
                        string cellA = "a" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellA, countPP,"@");
                    }

                    if (i == 2)
                    {
                        string cellB = "b" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellB, item.Фамилия.Do(w => w, "").Trim(),"@");
                    }

                    if (i == 3)
                    {
                        string cellB = "c" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellB, item.Имя.Do(w => w, "").Trim(),"@");
                    }

                    if (i == 4)
                    {
                        string cellB = "d" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellB, item.Отчество.Do(w => w, "").Trim(),"@");
                    }

                    if (i == 5)
                    {


                        string cellB = "e" + count.ToString();
                        excel.ExcelCellsAddValueFormat(cellB, item.НомерДоговора.Trim(),"@");
                    }

                    if (i == 6)
                    {
                        string cellB = "f" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.ДатаДоговора.Value.ToShortDateString().Do(w => w, ""));
                    }


                    if (i == 7)
                    {
                        string cellB = "g" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.НомерАкта.Do(w => w, "").ToString().Trim());
                    }

                    if (i == 8)
                    {
                        string cellB = "h" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.ДатаАктаВыполненныхРабот.Value.ToShortDateString().Do(w => w, ""));
                    }

                    if (i == 9)
                    {
                        string cellB = "i" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.ЛьготнаяКатегория.Do(w => w, "").ToString().Trim());
                    }


                    if (i == 10)
                    {
                        string cellB = "j" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.СуммаАктаВыполненныхРабот.Value.ToString("c"));
                    }

                    if (i == 11)
                    {
                        string cellB = "k" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.ДатаЗаписиДоговора.Value.ToShortDateString().Do(w=>w,""));
                    }

                    if (i == 12)
                    {
                        string cellB = "l" + count.ToString();
                        excel.ExcelCellsAddValue(cellB, item.СуммаДоговора.Value.ToString("c")); 
                    }

                    k++;
                    
                }

                count++;

                countPP++;
            }

            // Выведим общее количество договоров и общую сумму.

            count++;

            string cellИтого = "a" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтого, "Итого : " + listPrint.Count.ToString());

            if (this.rbОтсутствиеАкта.Checked == true)
            {
                string cellСумма = "j" + count.ToString();
                excel.ExcelCellsAddValueFontBold(cellСумма, "Сумма : " + listPrint.Sum(w => w.СуммаДоговора));
            }
            else if (this.rbНаличиеАкта.Checked == true)
            {
                string cellСумма = "j" + count.ToString();
                excel.ExcelCellsAddValueFontBold(cellСумма, "Сумма : " + listPrint.Sum(w => w.СуммаАктаВыполненныхРабот));                
            }


            decimal? number = 0.0m;
           
           
                // Выводим сумму по катрегориям. 
                int j = 1;
                foreach (var kat in comboBox1.Items)
                {
                    // Вместо нуля пишем пустую строку
                    if (kat.ToString().Trim() == "0")
                    { j++; continue; }

                    //var number = (from s in listPrint
                    //              where s.ЛьготнаяКатегория == kat.ToString()
                    //              select s.СуммаДоговора).Sum();

                    if (this.rbОтсутствиеАкта.Checked == true)
                    {
                        number = (from s in listPrint
                                  where s.ЛьготнаяКатегория == kat.ToString()
                                  select s.СуммаДоговора).Sum();
                    }
                    else if(this.rbНаличиеАкта.Checked == true )
                    {
                        number = (from s in listPrint
                                  where s.ЛьготнаяКатегория == kat.ToString()
                                  select s.СуммаАктаВыполненныхРабот).Sum();
                    }

                    int kolvo = listPrint.Count(p => p.ЛьготнаяКатегория == kat.ToString());

                    string cellСумма1 = "j" + (count + j).ToString();
                    string kolvo1 = "i" + (count + j).ToString();

                    excel.ExcelCellsAddValueFontBold(cellСумма1, kat.ToString().Trim() + ". Сумма : " + number.Value.ToString("c"));
                    excel.ExcelCellsAddValueFontBold(kolvo1, " Итого : " + kolvo);

                    j++;
                }
            
        }

        private void chbЛК_CheckedChanged(object sender, EventArgs e)
        {
            if (chbЛК.Checked == true)
            {
                this.comboBox1.Enabled = true;
            }
            else
            {
                this.comboBox1.Enabled = false;
            }
        }

       

        private void chbАкт_CheckedChanged(object sender, EventArgs e)
        {
            if (chbАкт.Checked == true)
            {
                this.groupBox2.Enabled = true;
            }
            else
            {
                this.groupBox2.Enabled = false;
            }
        }

        private IEnumerable<ReportBalanceStatistic> ReestrStatisticAdd(List<ReportBalanceStatistic> listReestr, string hospital, string preferentCategory)
        {
            // Дата начала отчетного периода.
            string dateStart = Время.Дата(this.dateTimePicker1.Value.Date.ToShortDateString());

            // Дата откончания отчетного периода.
            string dateEnd = Время.Дата(this.dateTimePicker2.Value.Date.ToShortDateString());

            // Возвращает баланс по неоплаченным договорам.
            QueryBalance qb = new QueryBalance(hospital, preferentCategory, dateStart, dateEnd);
            return qb.GenerateList();

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Список для хранения данных отчета.
            List<ReportBalanceStatistic> listReestr = new List<ReportBalanceStatistic>();

            //List<СуммаДоговоровБезАктов> listExcel = new List<СуммаДоговоровБезАктов>();
            List<СуммаКоличествоДоговоров> listExcel = new List<СуммаКоличествоДоговоров>();

            // Получим список поликлинник.
            RepositroyrHospital repozHosp = new RepositroyrHospital();
            foreach (var hosp in repozHosp.GetListHospital())
            {
                List<ReportBalanceStatistic> items = new List<ReportBalanceStatistic>();

                СуммаКоличествоДоговоров item = new СуммаКоличествоДоговоров();
                item.НаименованиеПоликлинники = hosp.Наименование;

                // Запишем в реестр ВВС.
                var ВВС =  ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ВетеранВоеннойСлужбы);

                if (ВВС.FirstOrDefault() != null)
                {
                    item.ВетераныВоеннойСлужбыКОличество = ВВС.First().КоличествоДоговоров;
                    item.ВетераныВоеннойСлужбыСумма = ВВС.First().СуммаДоговоров;
                }

                // Запишев в реестр ветеранов труда.
                var ВТ = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ВетеранТруда);

                if (ВТ.FirstOrDefault() != null)
                {
                    item.ВетераныТрудаКоличество = ВТ.First().КоличествоДоговоров;
                    item.ВетераныТрудаСумма = ВТ.First().СуммаДоговоров;
                }

                // Запишев в реестр ветеранов труда Саратовской области.
                var ВТСО = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ВетеранТрудаСаратовскойОбласти);

                if (ВТСО.FirstOrDefault() != null)
                {
                    item.ВетераныТрудаСаратовскойОбластиКОличество = ВТСО.First().КоличествоДоговоров;
                    item.ВетераныТрудаСаратовскойОбластиСумма = ВТСО.First().СуммаДоговоров;
                }

                // Запишев в реестр реабелитированных.
                var РЛ = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.РеабелитированныеЛица);

                if (РЛ.FirstOrDefault() != null)
                {
                    //var query = from v in ВТСО
                    //            where v != null
                    //            select v.КоличествоДоговоров;

                    item.РеабелитированныеКоличество = РЛ.First().КоличествоДоговоров;
                    item.РеабелитированныеСумма = РЛ.First().СуммаДоговоров;
                }

                // Запишев в реестр труженников тыла.
                var TT = ReestrStatisticAdd(listReestr, hosp.Наименование, PreferencCategory.ТруженикТыла);

                if (TT.FirstOrDefault() != null)
                {
                    item.ТруженникиТылаКоличество = TT.First().КоличествоДоговоров;
                    item.ТруженникиТылаСумма = TT.First().СуммаДоговоров;
                }

                items.AddRange(ВВС);
                items.AddRange(ВТ);
                items.AddRange(ВТСО);
                items.AddRange(РЛ);
                items.AddRange(TT);

                // Всего количество и сумма.
                item.ВсегоСумма = items.Sum(w=>w.СуммаДоговоров);

                // Сумма количество договоров.
                item.ВсегоКоличество = items.Sum(w=>w.КоличествоДоговоров);
               
                listExcel.Add(item);

                string asd = "";
            }
            
            
            // Выведим всё в Excel.
            // Выведим на бумагу.
            GenerateExcelFile excel = new GenerateExcelFile();

            // Создадим книгу.
            excel.ExcelWorkbook();

            // Создадим лист.
            excel.ExcelWorksheet(1);


            string cellA1 = "a1";
            excel.ExcelCellsAddValueFontBold(cellA1, "Отчёт по суммам не оплаченных договоров  в диапазоне от " + this.dateTimePicker1.Value.ToShortDateString() + " до " + this.dateTimePicker2.Value.ToShortDateString() + " " );

            // Выведим шапку таблицы.
            excel.ExcelCellsAddValueFontBold("a3", "Наименование учреждения здравоохранения");
            excel.ExcelCellsAddValueFontBold("b3", "Ветераны труда");
            excel.ExcelCellsAddValueFontBold("c3", "Ветераны труда кол-во договоров");
            excel.ExcelCellsAddValueFontBold("d3", "Ветераны военной службы");
            excel.ExcelCellsAddValueFontBold("e3", "Ветераны военной службы - кол-во договоров");
            excel.ExcelCellsAddValueFontBold("f3", "Ветераны труда Саратовской области");
            excel.ExcelCellsAddValueFontBold("g3", "Ветераны труда Саратовской области кол-во договоров");
            excel.ExcelCellsAddValueFontBold("h3", "Труженники тыла");
            excel.ExcelCellsAddValueFontBold("i3", "Труженники тыла кол-во договоров");
            excel.ExcelCellsAddValueFontBold("j3", "Реабилитированные");
            excel.ExcelCellsAddValueFontBold("k3", "Реабилитированные кол-во договоров");
            excel.ExcelCellsAddValueFontBold("l3", "Всего");
            excel.ExcelCellsAddValueFontBold("l3", "Всего кол-во договоров");

            // Индекс элемента в списке данных для отчёта.
            int k = 0;

            // Счётчик порядкового номера.
            int count = 4;

            // Номер по порядку.
            int countPP = 1;

            foreach (var item in listExcel)
            {
                // Сгенерируем содержимое письма.
                for (int i = 1; i <= 7; i++)
                {
                    
                    if (i == 1)
                    {
                        string cellA = "a" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellA, item.НаименованиеПоликлинники, "@", 100);

                    }

                    if (i == 2)
                    {
                        string cellB = "b" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellB, item.ВетераныТрудаСумма, "#,##0.00$",20);

                        string cellC = "c" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellC,item.ВетераныТрудаКоличество.ToString());//, "#,##0.00$", 20);
                    }

                    if (i == 3)
                    {
                        string cellD = "d" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellD, item.ВетераныВоеннойСлужбыСумма, "#,##0.00$", 20);

                        string cellE = "e" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellE, item.ВетераныВоеннойСлужбыКОличество.ToString());//, "#,##0.00$", 20);
                    }

                    if (i == 4)
                    {
                        string cellF = "f" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellF, item.ВетераныТрудаСаратовскойОбластиСумма, "#,##0.00$",20);

                        string cellG = "g" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellG, item.ВетераныТрудаСаратовскойОбластиКОличество.ToString());
                    }

                    if (i == 5)
                    {
                        string cellH = "h" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellH, item.ТруженникиТылаСумма, "#,##0.00$",20);

                        string cellI = "i" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellI, item.ТруженникиТылаКоличество.ToString());
                    }

                    if (i == 6)
                    {
                        string cellJ = "j" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellJ, item.РеабелитированныеСумма, "#,##0.00$",20);

                        string cellK = "k" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellK, item.РеабелитированныеКоличество.ToString());
                    }


                    if (i == 7)
                    {
                        string cellL = "l" + count.ToString();
                        excel.ExcelCellsAddValueFormatWidth(cellL, item.ВсегоСумма, "#,##0.00$",20);

                        string cellM = "m" + count.ToString();
                        excel.ExcelCellsAddValueFontBold(cellM, item.ВсегоКоличество.ToString());
                    }


                    k++;

                }

                count++;

                countPP++;
            }

            // Выведим общее количество договоров и общую сумму.

            count++;

            string cellИтогоA = "a" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоA, "Всего :", "@", 100);


            string cellИтогоB = "b" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоB, listExcel.Sum(w => w.ВетераныТрудаСумма), "#,##0.00$", 20);

            string cellИтогоC = "c" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоC, listExcel.Sum(w => w.ВетераныТрудаКоличество));

            string cellИтогоD = "d" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоD, listExcel.Sum(w => w.ВетераныВоеннойСлужбыСумма), "#,##0.00$", 20);

            string cellИтогоE = "e" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоE, listExcel.Sum(w => w.ВетераныВоеннойСлужбыКОличество));

            string cellИтогоF = "f" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоF, listExcel.Sum(w => w.ВетераныТрудаСаратовскойОбластиСумма), "#,##0.00$", 20);

            string cellИтогоG = "g" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоG, listExcel.Sum(w => w.ВетераныТрудаСаратовскойОбластиКОличество));

            string cellИтогоH = "h" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоH, listExcel.Sum(w => w.ТруженникиТылаСумма), "#,##0.00$", 20);

            string cellИтогоI = "i" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоI, listExcel.Sum(w => w.ТруженникиТылаКоличество));

            string cellИтогоJ = "j" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоJ, listExcel.Sum(w => w.РеабелитированныеСумма), "#,##0.00$", 20);

            string cellИтогоK = "k" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоK, listExcel.Sum(w => w.РеабелитированныеКоличество));

            string cellИтогоL = "l" + count.ToString();
            excel.ExcelCellsAddValueFormatWidthBold(cellИтогоL, listExcel.Sum(w => w.ВсегоСумма), "#,##0.00$", 20);

            string cellИтогоM = "m" + count.ToString();
            excel.ExcelCellsAddValueFontBold(cellИтогоM, listExcel.Sum(w => w.ВсегоКоличество));


            //string cellСумма = "j" + count.ToString();
            //excel.ExcelCellsAddValueFontBold(cellСумма, "Сумма : " + listPrint.Sum(w => w.СуммаАктаВыполненныхРабот));
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Дата начала отчета.
            string dateStartReport = this.dateTimePicker1.Value.ToShortDateString();

            string dateEndReport = this.dateTimePicker2.Value.ToShortDateString();

            QueryBD queryBalance = new QueryBD(Время.Дата(dateStartReport));

            queryBalance.GenerateReport(dateStartReport, dateEndReport);
        }
    }
}
