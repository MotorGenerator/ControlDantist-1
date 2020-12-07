namespace ControlDantist
{
    partial class FormInv
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbОтсутствиеАкта = new System.Windows.Forms.RadioButton();
            this.rbНаличиеАкта = new System.Windows.Forms.RadioButton();
            this.chbАкт = new System.Windows.Forms.CheckBox();
            this.chbЛК = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbHjspital = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(288, 295);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 17;
            this.btnOk.Text = "Печать";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(369, 295);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Отмена";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbОтсутствиеАкта);
            this.groupBox2.Controls.Add(this.rbНаличиеАкта);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(18, 170);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(424, 73);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Акт выполненных работ";
            // 
            // rbОтсутствиеАкта
            // 
            this.rbОтсутствиеАкта.AutoSize = true;
            this.rbОтсутствиеАкта.Location = new System.Drawing.Point(7, 43);
            this.rbОтсутствиеАкта.Name = "rbОтсутствиеАкта";
            this.rbОтсутствиеАкта.Size = new System.Drawing.Size(211, 17);
            this.rbОтсутствиеАкта.TabIndex = 1;
            this.rbОтсутствиеАкта.TabStop = true;
            this.rbОтсутствиеАкта.Text = "отсутствие акта выполненных работ";
            this.rbОтсутствиеАкта.UseVisualStyleBackColor = true;
            // 
            // rbНаличиеАкта
            // 
            this.rbНаличиеАкта.AutoSize = true;
            this.rbНаличиеАкта.Location = new System.Drawing.Point(7, 20);
            this.rbНаличиеАкта.Name = "rbНаличиеАкта";
            this.rbНаличиеАкта.Size = new System.Drawing.Size(196, 17);
            this.rbНаличиеАкта.TabIndex = 0;
            this.rbНаличиеАкта.TabStop = true;
            this.rbНаличиеАкта.Text = "наличие акта выполненных работ";
            this.rbНаличиеАкта.UseVisualStyleBackColor = true;
            // 
            // chbАкт
            // 
            this.chbАкт.AutoSize = true;
            this.chbАкт.Location = new System.Drawing.Point(18, 146);
            this.chbАкт.Name = "chbАкт";
            this.chbАкт.Size = new System.Drawing.Size(260, 17);
            this.chbАкт.TabIndex = 14;
            this.chbАкт.Text = "Наличие / отсутствие акт выполненных работ";
            this.chbАкт.UseVisualStyleBackColor = true;
            this.chbАкт.CheckedChanged += new System.EventHandler(this.chbАкт_CheckedChanged);
            // 
            // chbЛК
            // 
            this.chbЛК.AutoSize = true;
            this.chbЛК.Location = new System.Drawing.Point(18, 108);
            this.chbЛК.Name = "chbЛК";
            this.chbЛК.Size = new System.Drawing.Size(129, 17);
            this.chbЛК.TabIndex = 12;
            this.chbЛК.Text = "Льготная категория";
            this.chbЛК.UseVisualStyleBackColor = true;
            this.chbЛК.CheckedChanged += new System.EventHandler(this.chbЛК_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(9, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 60);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Диапазон дат";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(221, 19);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(133, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(196, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "до";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(52, 19);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(133, 20);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "от";
            // 
            // cmbHjspital
            // 
            this.cmbHjspital.FormattingEnabled = true;
            this.cmbHjspital.Location = new System.Drawing.Point(93, 12);
            this.cmbHjspital.Name = "cmbHjspital";
            this.cmbHjspital.Size = new System.Drawing.Size(349, 21);
            this.cmbHjspital.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Поликлинника";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(154, 106);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(175, 21);
            this.comboBox1.TabIndex = 18;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(20, 295);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(187, 23);
            this.btnPrint.TabIndex = 19;
            this.btnPrint.Text = "Статистика не оплаченных";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 23);
            this.button1.TabIndex = 20;
            this.button1.Text = "Баланс не оплаченных";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormInv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 330);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chbАкт);
            this.Controls.Add(this.chbЛК);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbHjspital);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormInv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Инвентаризация";
            this.Load += new System.EventHandler(this.FormInv_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbОтсутствиеАкта;
        private System.Windows.Forms.RadioButton rbНаличиеАкта;
        private System.Windows.Forms.CheckBox chbАкт;
        private System.Windows.Forms.CheckBox chbЛК;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbHjspital;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button button1;
    }
}