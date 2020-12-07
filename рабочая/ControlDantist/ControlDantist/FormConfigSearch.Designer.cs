namespace ControlDantist
{
    partial class FormConfigSearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbFIO = new System.Windows.Forms.RadioButton();
            this.rbFIO2 = new System.Windows.Forms.RadioButton();
            this.rbFIO3 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rbFIO4 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbFIO5 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(282, 232);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(202, 232);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.rbFIO5);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.rbFIO4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.rbFIO3);
            this.groupBox2.Controls.Add(this.rbFIO2);
            this.groupBox2.Controls.Add(this.rbFIO);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 214);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // rbFIO
            // 
            this.rbFIO.AutoSize = true;
            this.rbFIO.Location = new System.Drawing.Point(7, 20);
            this.rbFIO.Name = "rbFIO";
            this.rbFIO.Size = new System.Drawing.Size(204, 17);
            this.rbFIO.TabIndex = 0;
            this.rbFIO.TabStop = true;
            this.rbFIO.Text = "Фамилия Имя Отчество льготника";
            this.rbFIO.UseVisualStyleBackColor = true;
            // 
            // rbFIO2
            // 
            this.rbFIO2.AutoSize = true;
            this.rbFIO2.Location = new System.Drawing.Point(7, 44);
            this.rbFIO2.Name = "rbFIO2";
            this.rbFIO2.Size = new System.Drawing.Size(195, 17);
            this.rbFIO2.TabIndex = 1;
            this.rbFIO2.TabStop = true;
            this.rbFIO2.Text = "ФИО льготника и дата рождения";
            this.rbFIO2.UseVisualStyleBackColor = true;
            // 
            // rbFIO3
            // 
            this.rbFIO3.AutoSize = true;
            this.rbFIO3.Location = new System.Drawing.Point(7, 71);
            this.rbFIO3.Name = "rbFIO3";
            this.rbFIO3.Size = new System.Drawing.Size(14, 13);
            this.rbFIO3.TabIndex = 2;
            this.rbFIO3.TabStop = true;
            this.rbFIO3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 26);
            this.label1.TabIndex = 3;
            this.label1.Text = "ФИО льготника, дата рождения, серия и номер \r\nдокумента дающие право на льготу";
            // 
            // rbFIO4
            // 
            this.rbFIO4.AutoSize = true;
            this.rbFIO4.Location = new System.Drawing.Point(7, 107);
            this.rbFIO4.Name = "rbFIO4";
            this.rbFIO4.Size = new System.Drawing.Size(14, 13);
            this.rbFIO4.TabIndex = 4;
            this.rbFIO4.TabStop = true;
            this.rbFIO4.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(248, 39);
            this.label2.TabIndex = 5;
            this.label2.Text = "ФИО льготника, дата рождения, серия, номер \r\nи дата выдачи документа дающие \r\nпра" +
                "во на льготу";
            // 
            // rbFIO5
            // 
            this.rbFIO5.AutoSize = true;
            this.rbFIO5.Location = new System.Drawing.Point(7, 153);
            this.rbFIO5.Name = "rbFIO5";
            this.rbFIO5.Size = new System.Drawing.Size(14, 13);
            this.rbFIO5.TabIndex = 6;
            this.rbFIO5.TabStop = true;
            this.rbFIO5.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(251, 52);
            this.label3.TabIndex = 7;
            this.label3.Text = "ФИО льготника, дата рождения, серия, номер, \r\nдата выдачи документа дающие \r\nправ" +
                "о на льготу, серия и номер паспорта\r\nгражданина РФ\r\n";
            // 
            // FormConfigSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 265);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormConfigSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Конфигурация поиска льготника в ЭСРН";
            this.Load += new System.EventHandler(this.FormConfigSearch_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbFIO2;
        private System.Windows.Forms.RadioButton rbFIO;
        private System.Windows.Forms.RadioButton rbFIO3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbFIO4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbFIO5;
    }
}