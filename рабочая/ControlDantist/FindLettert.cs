using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ControlDantist.Find;

namespace ControlDantist
{
    public partial class FindLettert : Form
    {
        public FindLettert()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                FindPersonForNumberLetter findPersonForNumberLetter = new FindPersonForNumberLetter(this.textBox1.Text, this.textBox2.Text, this.textBox3.Text);

                this.dataGridView1.DataSource = findPersonForNumberLetter.GetNumberLetter();

                DisplayDataGrid();
            }
            else
            {
                MessageBox.Show("Заполните поля поиска","Внимание",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void DisplayDataGrid()
        {
            this.dataGridView1.Columns["NumberLatter"].HeaderText = "Номер письма";
            this.dataGridView1.Columns["DateLetter"].HeaderText = "Дата письма";
        }
    }
}
