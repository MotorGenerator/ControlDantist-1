namespace ControlDantist
{
    partial class FormValidOutEsrn
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
            this.components = new System.ComponentModel.Container();
            this.button4 = new System.Windows.Forms.Button();
            this.btnSum = new System.Windows.Forms.Button();
            this.btnExel = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnDist = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dataError = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataCorrect = new System.Windows.Forms.DataGridView();
            this.btnLK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtДокумент = new System.Windows.Forms.TextBox();
            this.txtАдрес = new System.Windows.Forms.TextBox();
            this.txtЛьготник = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.сохранитьДоговорToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаУслугToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.проверкаЭСРНToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.снятьГалочкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataError)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataCorrect)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(373, 243);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(167, 23);
            this.button4.TabIndex = 34;
            this.button4.Text = "Выделить все без проверки";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            // 
            // btnSum
            // 
            this.btnSum.Location = new System.Drawing.Point(546, 243);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(75, 23);
            this.btnSum.TabIndex = 32;
            this.btnSum.Text = "Подсчёт";
            this.btnSum.UseVisualStyleBackColor = true;
            this.btnSum.Visible = false;
            // 
            // btnExel
            // 
            this.btnExel.Location = new System.Drawing.Point(627, 243);
            this.btnExel.Name = "btnExel";
            this.btnExel.Size = new System.Drawing.Size(100, 23);
            this.btnExel.TabIndex = 31;
            this.btnExel.Text = "Проверить excel";
            this.btnExel.UseVisualStyleBackColor = true;
            this.btnExel.Visible = false;
            // 
            // btnCheck
            // 
            this.btnCheck.Enabled = false;
            this.btnCheck.Location = new System.Drawing.Point(256, 15);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(123, 23);
            this.btnCheck.TabIndex = 2;
            this.btnCheck.Text = "Направлен запрос";
            this.btnCheck.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(123, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(127, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Не подтверждённые";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 15);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Подтверждённые";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnCheck);
            this.groupBox3.Controls.Add(this.button3);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Location = new System.Drawing.Point(19, 552);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(388, 44);
            this.groupBox3.TabIndex = 30;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Список договоров";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCount.ForeColor = System.Drawing.Color.Green;
            this.lblCount.Location = new System.Drawing.Point(22, 245);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(56, 16);
            this.lblCount.TabIndex = 29;
            this.lblCount.Text = "Итого:";
            // 
            // btnDist
            // 
            this.btnDist.Location = new System.Drawing.Point(733, 243);
            this.btnDist.Name = "btnDist";
            this.btnDist.Size = new System.Drawing.Size(101, 23);
            this.btnDist.TabIndex = 28;
            this.btnDist.Text = "Выделить всё";
            this.btnDist.UseVisualStyleBackColor = true;
            this.btnDist.Click += new System.EventHandler(this.btnDist_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(638, 557);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 27;
            this.button1.Text = "Записать договора";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataError
            // 
            this.dataError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataError.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataError.Location = new System.Drawing.Point(6, 20);
            this.dataError.Name = "dataError";
            this.dataError.Size = new System.Drawing.Size(376, 174);
            this.dataError.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.dataError);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox2.Location = new System.Drawing.Point(452, 349);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(388, 201);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ошибочные данные";
            // 
            // dataCorrect
            // 
            this.dataCorrect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataCorrect.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataCorrect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataCorrect.Location = new System.Drawing.Point(9, 21);
            this.dataCorrect.Name = "dataCorrect";
            this.dataCorrect.Size = new System.Drawing.Size(418, 173);
            this.dataCorrect.TabIndex = 0;
            // 
            // btnLK
            // 
            this.btnLK.Location = new System.Drawing.Point(414, 567);
            this.btnLK.Name = "btnLK";
            this.btnLK.Size = new System.Drawing.Size(85, 23);
            this.btnLK.TabIndex = 33;
            this.btnLK.Text = "Статистика";
            this.btnLK.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.dataCorrect);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.Green;
            this.groupBox1.Location = new System.Drawing.Point(13, 349);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 201);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Верные данные";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 301);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Адрес:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "ФИО льготника:";
            // 
            // txtДокумент
            // 
            this.txtДокумент.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtДокумент.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtДокумент.Location = new System.Drawing.Point(142, 318);
            this.txtДокумент.Multiline = true;
            this.txtДокумент.Name = "txtДокумент";
            this.txtДокумент.ReadOnly = true;
            this.txtДокумент.Size = new System.Drawing.Size(698, 24);
            this.txtДокумент.TabIndex = 21;
            // 
            // txtАдрес
            // 
            this.txtАдрес.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtАдрес.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtАдрес.Location = new System.Drawing.Point(142, 295);
            this.txtАдрес.Multiline = true;
            this.txtАдрес.Name = "txtАдрес";
            this.txtАдрес.ReadOnly = true;
            this.txtАдрес.Size = new System.Drawing.Size(698, 24);
            this.txtАдрес.TabIndex = 20;
            // 
            // txtЛьготник
            // 
            this.txtЛьготник.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtЛьготник.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtЛьготник.Location = new System.Drawing.Point(142, 272);
            this.txtЛьготник.Multiline = true;
            this.txtЛьготник.Name = "txtЛьготник";
            this.txtЛьготник.ReadOnly = true;
            this.txtЛьготник.Size = new System.Drawing.Size(698, 24);
            this.txtЛьготник.TabIndex = 19;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(765, 557);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Закрыть";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // сохранитьДоговорToolStripMenuItem
            // 
            this.сохранитьДоговорToolStripMenuItem.Name = "сохранитьДоговорToolStripMenuItem";
            this.сохранитьДоговорToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.сохранитьДоговорToolStripMenuItem.Text = "Сохранить Договор";
            // 
            // проверкаУслугToolStripMenuItem
            // 
            this.проверкаУслугToolStripMenuItem.Name = "проверкаУслугToolStripMenuItem";
            this.проверкаУслугToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.проверкаУслугToolStripMenuItem.Text = "Проверка Услуг";
            // 
            // проверкаЭСРНToolStripMenuItem
            // 
            this.проверкаЭСРНToolStripMenuItem.Name = "проверкаЭСРНToolStripMenuItem";
            this.проверкаЭСРНToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.проверкаЭСРНToolStripMenuItem.Text = "Проверка ЭСРН";
            // 
            // снятьГалочкиToolStripMenuItem
            // 
            this.снятьГалочкиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.проверкаЭСРНToolStripMenuItem,
            this.проверкаУслугToolStripMenuItem,
            this.сохранитьДоговорToolStripMenuItem});
            this.снятьГалочкиToolStripMenuItem.Name = "снятьГалочкиToolStripMenuItem";
            this.снятьГалочкиToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.снятьГалочкиToolStripMenuItem.Text = "Снять галочки";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.снятьГалочкиToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 26);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 324);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Удостоверение:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Location = new System.Drawing.Point(0, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(853, 230);
            this.dataGridView1.TabIndex = 17;
            this.dataGridView1.TabStop = false;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // FormValidOutEsrn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 601);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnSum);
            this.Controls.Add(this.btnExel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.btnDist);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnLK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtДокумент);
            this.Controls.Add(this.txtАдрес);
            this.Controls.Add(this.txtЛьготник);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormValidOutEsrn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormValidOutEsrn";
            this.Load += new System.EventHandler(this.FormValidOutEsrn_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataError)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataCorrect)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnSum;
        private System.Windows.Forms.Button btnExel;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnDist;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataError;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataCorrect;
        private System.Windows.Forms.Button btnLK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtДокумент;
        private System.Windows.Forms.TextBox txtАдрес;
        private System.Windows.Forms.TextBox txtЛьготник;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolStripMenuItem сохранитьДоговорToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверкаУслугToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem проверкаЭСРНToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem снятьГалочкиToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}