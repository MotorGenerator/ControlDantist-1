using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Repository;
using ControlDantist.BalanceContract;

namespace ControlDantist
{
    public partial class FormLimitMoney : Form
    {

        /// <summary>
        /// Флаг редактирования лимитов денежных средств (false - Добавлять денежные средства, true - редактировать).
        /// </summary>
        private bool FlagEDitLK = false;

        /// <summary>
        /// Режим работы формы - true - добавление записи, false - редактирование записи.
        /// </summary>
        public bool FlgInsert { get; set; }

        public FormLimitMoney()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormLimitMoney_Load(object sender, EventArgs e)
        {
            // Загружаем данные в форму.
            LoadForm();
        }

        private void LoadForm()
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";

            var year = dateTimePicker1.Value.Year;

            // Узнаем можем добавлять новый лимит или только редактирование.
            YearLimit yearLimit = new YearLimit(year);
            bool flarEdit = yearLimit.ValidateEdit();

            // Разрешение на добавление лимита в льготные категории.
            DisplayButtonAdd(flarEdit);

            // Заполнить данными DataGrid.
            BalanceDisplay balanceDisplay = new BalanceDisplay();
            this.dataGridView1.DataSource = balanceDisplay.CreateList(year);

            // Форматирование DataGrid.
            DisplayLK();

            Year year1 = new Year();
            year1.Year1 = year;

            // Выведим лимит.
            UnitDate unitDate = new UnitDate();
            Year selectYear = unitDate.YearRepository.Select(year1);

            // Если выбран год который не записан в БД.
            if (selectYear == null)
            {
                Year nullYear = new Year();
                nullYear.intYear = 0;
                selectYear = nullYear;
            }

            var limitYear  = unitDate.LimitMoneyRepository.SelectToYear(selectYear.intYear);

            if(limitYear.Count() > 0)
            {
                this.label4.Text = limitYear.Sum(w => w.Limit).ToString("c");
            }
        }

        /// <summary>
        /// Форматирование DataGrid.
        /// </summary>
        private void DisplayLK()
        {
            this.dataGridView1.Columns["Ids"].Visible = false;
            this.dataGridView1.Columns["NameLK"].Width = 180;
            this.dataGridView1.Columns["NameLK"].HeaderText = "Льготная категория";
            this.dataGridView1.Columns["SumMoney"].Width = 180;
            this.dataGridView1.Columns["SumMoney"].HeaderText = "Сумма лимита";
            this.dataGridView1.Columns["IdLimitedMoney"].Visible = false;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            FormSelectLimitMoney formSelectLimitMoney = new FormSelectLimitMoney();

            // Форма в режиме добавления новой записи.
            formSelectLimitMoney.FlagEditLimit = false;

            // Передадим выбранный год.
            formSelectLimitMoney.Year = this.dateTimePicker1.Value.Year;
            DialogResult dialogResult = formSelectLimitMoney.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                // Обновим данные на форме.
                LoadForm();
            }

        }

        // Добавление новых записей или редактирование.
        private void DisplayButtonAdd(bool flag)
        {
            if(flag == true)
            {
                this.btnSave.Enabled = false;
            }
            else
            {
                this.btnSave.Enabled = true;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Ключи для редактирования.
            string keys = this.dataGridView1.CurrentRow.Cells[0].Value.ToString();

            // Деньги.
            string money = this.dataGridView1.CurrentRow.Cells[2].Value.ToString();

            FormSelectLimitMoney formSelectLimitMoney = new FormSelectLimitMoney();

            formSelectLimitMoney.Money = money;

            formSelectLimitMoney.Keys = keys;

            formSelectLimitMoney.IdMoney = Convert.ToInt16(this.dataGridView1.CurrentRow.Cells[3].Value);

            // Форма в режиме добавления новой записи.
            formSelectLimitMoney.FlagEditLimit = true;

            // Передадим выбранный год.
            formSelectLimitMoney.Year = this.dateTimePicker1.Value.Year;
            DialogResult dialogResult = formSelectLimitMoney.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                LoadForm();
            }


        }
    }
}
