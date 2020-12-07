using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ControlDantist
{
    public partial class FormReestr : Form
    {
        private string номер–еестра;
        private string дата–еестра;
        private string дата—чЄт‘актуры;
        private string номер—чЄт‘актуры;

        /// <summary>
        /// Ќомер счЄт фактуры
        /// </summary>
        public string Ќомер—чЄт‘актуры
        {
            get
            {
                return номер—чЄт‘актуры;
            }
            set
            {
                номер—чЄт‘актуры = value;
            }
        }

        /// <summary>
        /// ƒата счЄт фактуры
        /// </summary>
        public string ƒата—чЄт‘актуры
        {
            get
            {
                return дата—чЄт‘актуры;
            }
            set
            {
                дата—чЄт‘актуры = value;
            }
        }

        /// <summary>
        /// ’ранит дату реестра
        /// </summary>
        public string ƒата–еестра
        {
            get
            {
                return дата–еестра;
            }
            set
            {
                дата–еестра = value;
            }
        }
              

        /// <summary>
        /// ’ранит номер реестра
        /// </summary>
        public string Ќомер–еестра
        {
            get
            {
                return номер–еестра;
            }
            set
            {
                номер–еестра = value;
            }
        }
                


        public FormReestr()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //«апишем в свойства формы номера реестра и счЄт фактуры и даты создани€

            //«апишем дату реестра
            this.ƒата–еестра = this.dateTimePicker1.Value.ToShortDateString().Trim();

            //«апишем номер рееестра
            this.Ќомер–еестра = this.textBox1.Text.Trim();

            //«апишем номер счЄт-фактуры
            this.Ќомер—чЄт‘актуры = this.textBox2.Text.Trim();

            this.ƒата—чЄт‘актуры = this.dateTimePicker2.Value.ToShortDateString().Trim();


        }

       
    }
}