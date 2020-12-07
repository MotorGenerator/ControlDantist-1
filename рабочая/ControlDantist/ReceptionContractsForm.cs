using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ControlDantist.Repository;
using System.Windows.Forms;
using ControlDantist.ValidateRegistrProject;
using DantistLibrary;
using ControlDantist.Classes;

namespace ControlDantist
{
    public partial class ReceptionContractsForm : Form
    {
        private UnitDate unitDate;

        /// <summary>
        /// ID поликлинники.
        /// </summary>
        public int IdHospital { get; set; }

        /// <summary>
        /// Номер сопроводительного письма.
        /// </summary>
        public string NumberLetter { get; set; }

        /// <summary>
        /// Дата входящего письма.
        /// </summary>
        public DateTime DateLetter { get; set; }

        /// <summary>
        /// Закрывать форму или нет.
        /// </summary>
        private bool flagNotClose = true;

        public bool SaveProjectFIle { get; set; }

        ///// <summary>
        ///// Флаг указывает что записываем реестр проектов договоров в БД.
        ///// </summary>
        //public bool FlagWriteRegistrBase { get; set; }

        public ReceptionContractsForm()
        {
            InitializeComponent();

            this.unitDate = new UnitDate();

            this.btnYes.Text = "Открыть";
        }

        public ReceptionContractsForm(UnitDate unitDate)
        {
            InitializeComponent();

            this.unitDate = unitDate;
        }

        private void ReceptionContractsForm_Load(object sender, EventArgs e)
        {

            // Получим список поликлинник.
            this.comboBox1.DataSource = unitDate.ПоликлинникаИННRepository.SelectAll();
            comboBox1.DisplayMember = "F2";
            comboBox1.ValueMember = "F1";

            //// Включили  живой поиск элементов.
            //this.comboBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            //this.comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //this.comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;

            if (this.SaveProjectFIle == true)
            {
                this.Controls.Remove(this.dataGridView1 as Control);

                this.containerNumberLetter.Visible = true;
                this.groupBox1.Visible = true;

                this.Size = new System.Drawing.Size(772, 172);

                this.containerNumberLetter.Location = new Point(20, 65);
                this.groupBox1.Location = new Point(177, 65);
                this.label1.Location = new Point(153, 74);

                this.btnYes.Location = new Point(588,90);
                this.btnCancel.Location = new Point(669, 90);
            }
            else
            {
                this.Size = new System.Drawing.Size(772, 537);

                //this.containerNumberLetter.Location = new Point(20, 41);
                //this.groupBox1.Location = new Point(177, 41);
                //this.label1.Location = new Point(153, 44);
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
              // Значение ID выбранной поликлинники.
            this.IdHospital = Convert.ToInt16(this.comboBox1.SelectedValue);

            // Номер письма.
            this.NumberLetter = this.textBox1.Text;

            // Дата письма.
            this.DateLetter = this.dateTimePicker1.Value;

            var testFlag = this.flagNotClose;

            if (ValidateInput(this.IdHospital, this.NumberLetter) == false)
            {
                // Запретим закрытие формы.
                flagNotClose = true;
            }
            else
            {
                flagNotClose = false;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Разрешим закрытие формы.
            flagNotClose = false;
            this.Close();
        }

        private bool ValidateInput(int idHospital, string numberLetter)
        {
            bool flagValidate = false;

            int id = Convert.ToInt32(this.comboBox1.SelectedValue);

            if (id > 0 && numberLetter.Length > 0)
            {
                flagValidate = true;
            }
            else
            {
                // Проверку не прошли.
                flagValidate = false;
            }

            //if (id > 0 && numberLetter.Length > 0)
            //{
            //    flagNotClose = false;
            //}
            //else
            //{
            //    flagNotClose = true;
            //}

            return flagValidate;
        }

        private void ReceptionContractsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagNotClose == true)
            {
                e.Cancel = true;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

            int idHospital;
            bool result = int.TryParse(this.comboBox1.SelectedValue.ToString(), out idHospital);

            if (result == true)
            {

                UnitDate unitDate = new UnitDate();

                IQueryable<RegistrProject> registrProject = unitDate.ProjectRegistrFilesRepository.GetRegistr(idHospital);

                this.dataGridView1.DataSource = registrProject.ToList();

                // Скроем лишние поля.
                this.dataGridView1.Columns["IdProjectRegistr"].Visible = false;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Получим id записи.
            UnitDate unitDate = new UnitDate();

            FiltrRepositoryДоговор filtrRepositoryДоговор = new FiltrRepositoryДоговор(unitDate);

            // Получим id файла реестра проектов договоров.
            int idFile = Convert.ToInt32(this.dataGridView1.CurrentRow.Cells["IdProjectRegistr"].Value);

            // Получим статус ожидания провреки.
            bool flagValid = Convert.ToBoolean(this.dataGridView1.CurrentRow.Cells["StatusValide"].Value);

            if (flagValid == true)
            {
                MessageBox.Show("Реестр уже прошёл проверку.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                //this.Close();

                //// Завершим проверку.
                //return;

            }

            // Открываем проекты договорв находящихся в статусе ожидающих проверку.
            var договорs = unitDate.ViewЛьготникДоговорРеестрRepository.Select(idFile);

            // Скачаем файл и посмотрим льгоотную категорию.
            Dictionary<string, Unload> unload = unitDate.ProjectRegistrFilesRepository.SelectFiles(idFile);

            //Создадим список объектов для хранения результатов проверки проектов договоров
            Dictionary<string, ValidateContract> listValContr = new Dictionary<string, ValidateContract>();

            string льготнаяКатегория = string.Empty;

            if (unload != null)
            {
                льготнаяКатегория = unload.FirstOrDefault().Value.ЛьготнаяКатегория;

                //договорs.Count()

                if (договорs.Count() > 0)
                {
                    // Получим ИНН поликлинники.
                    string инн = unload.Values.FirstOrDefault().Поликлинника.Rows[0]["инн"].ToString();

                    // Проверим по базам ЭСРН.
                    EsrnValidate esrnValidate = new EsrnValidate(договорs, льготнаяКатегория);

                    var result = esrnValidate.ValidateList();

                    // Для тест.
                    result.Select(w => w.Value.flagValidEsrn = true);



                    this.Hide();
                    //Выведим результат проверки на экран
                    FormValidOutEsrn formOutVal = new FormValidOutEsrn(result, инн);
                    formOutVal.IdFileRegistr = idFile;

                    formOutVal.Show();

                    formOutVal.WindowState = FormWindowState.Normal;

                    this.Close();

                }
                else
                {
                    MessageBox.Show("Нетданных в базе по реестру");
                }
            }
            else
            {
                MessageBox.Show("Нет файла в реестре");
            }

            return;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

            //using (SqlConnection cn = new SqlConnection(connStr))
            //{
            //    string fillOrg = "Select Название from Организации where ID not in (33)";

            //    DataTable table = new DataTable();

            //    SqlDataAdapter da = new SqlDataAdapter(fillOrg, cn);

            //    da.Fill(table);

            //    comboBox1.DataSource = table;
            //    comboBox1.DisplayMember = "Название";
            //    comboBox1.SelectedIndex = -1;
            //}

            //// Получим список поликлинник.
            //this.comboBox1.DataSource = FiltrHospital(this.textBox1.Text);
            //comboBox1.DisplayMember = "F2";
            //comboBox1.ValueMember = "F1";
        }

        private DataTable FiltrHospital(string nameHospital)
        {
            string query = "select F1,F2 from [dbo].[ПоликлинникиИнн] " +
                           "where F2 like '%"+ nameHospital +"%' ";

            return ТаблицаБД.GetTableSQL(query, "ПоликлинникаИНН");
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            // Получим список поликлинник.
            this.comboBox1.DataSource = FiltrHospital(this.comboBox1.Text);
            comboBox1.DisplayMember = "F2";
            comboBox1.ValueMember = "F1";
        }
    }
}
