namespace AbstractSanitaryView
{
    partial class FormBasic
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.клиентыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.запчастиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.услугиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.складыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сантехникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пополнитьСкладToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчётыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.прайсУслугToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загруженностьСкладовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заказыКлиентовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonCreateOrdering = new System.Windows.Forms.Button();
            this.buttonTakeOrderingInWork = new System.Windows.Forms.Button();
            this.buttonOrderingReady = new System.Windows.Forms.Button();
            this.buttonPayOrdering = new System.Windows.Forms.Button();
            this.buttonRef = new System.Windows.Forms.Button();
            this.письмаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справочникиToolStripMenuItem,
            this.пополнитьСкладToolStripMenuItem,
            this.отчётыToolStripMenuItem,
            this.письмаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1099, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.клиентыToolStripMenuItem,
            this.запчастиToolStripMenuItem,
            this.услугиToolStripMenuItem,
            this.складыToolStripMenuItem,
            this.сантехникиToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // клиентыToolStripMenuItem
            // 
            this.клиентыToolStripMenuItem.Name = "клиентыToolStripMenuItem";
            this.клиентыToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.клиентыToolStripMenuItem.Text = "Клиенты";
            this.клиентыToolStripMenuItem.Click += new System.EventHandler(this.клиентыToolStripMenuItem_Click);
            // 
            // запчастиToolStripMenuItem
            // 
            this.запчастиToolStripMenuItem.Name = "запчастиToolStripMenuItem";
            this.запчастиToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.запчастиToolStripMenuItem.Text = "Запчасти";
            this.запчастиToolStripMenuItem.Click += new System.EventHandler(this.запчастиToolStripMenuItem_Click);
            // 
            // услугиToolStripMenuItem
            // 
            this.услугиToolStripMenuItem.Name = "услугиToolStripMenuItem";
            this.услугиToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.услугиToolStripMenuItem.Text = "Услуги";
            this.услугиToolStripMenuItem.Click += new System.EventHandler(this.услугиToolStripMenuItem_Click);
            // 
            // складыToolStripMenuItem
            // 
            this.складыToolStripMenuItem.Name = "складыToolStripMenuItem";
            this.складыToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.складыToolStripMenuItem.Text = "Склады";
            this.складыToolStripMenuItem.Click += new System.EventHandler(this.складыToolStripMenuItem_Click);
            // 
            // сантехникиToolStripMenuItem
            // 
            this.сантехникиToolStripMenuItem.Name = "сантехникиToolStripMenuItem";
            this.сантехникиToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.сантехникиToolStripMenuItem.Text = "Сантехники";
            this.сантехникиToolStripMenuItem.Click += new System.EventHandler(this.сантехникиToolStripMenuItem_Click);
            // 
            // пополнитьСкладToolStripMenuItem
            // 
            this.пополнитьСкладToolStripMenuItem.Name = "пополнитьСкладToolStripMenuItem";
            this.пополнитьСкладToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.пополнитьСкладToolStripMenuItem.Text = "Пополнить склад";
            this.пополнитьСкладToolStripMenuItem.Click += new System.EventHandler(this.пополнитьСкладToolStripMenuItem_Click);
            // 
            // отчётыToolStripMenuItem
            // 
            this.отчётыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.прайсУслугToolStripMenuItem,
            this.загруженностьСкладовToolStripMenuItem,
            this.заказыКлиентовToolStripMenuItem});
            this.отчётыToolStripMenuItem.Name = "отчётыToolStripMenuItem";
            this.отчётыToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.отчётыToolStripMenuItem.Text = "Отчёты";
            // 
            // прайсУслугToolStripMenuItem
            // 
            this.прайсУслугToolStripMenuItem.Name = "прайсУслугToolStripMenuItem";
            this.прайсУслугToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.прайсУслугToolStripMenuItem.Text = "Прайс услуг";
            this.прайсУслугToolStripMenuItem.Click += new System.EventHandler(this.прайсУслугToolStripMenuItem_Click);
            // 
            // загруженностьСкладовToolStripMenuItem
            // 
            this.загруженностьСкладовToolStripMenuItem.Name = "загруженностьСкладовToolStripMenuItem";
            this.загруженностьСкладовToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.загруженностьСкладовToolStripMenuItem.Text = "Загруженность складов";
            this.загруженностьСкладовToolStripMenuItem.Click += new System.EventHandler(this.загруженностьСкладовToolStripMenuItem_Click);
            // 
            // заказыКлиентовToolStripMenuItem
            // 
            this.заказыКлиентовToolStripMenuItem.Name = "заказыКлиентовToolStripMenuItem";
            this.заказыКлиентовToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.заказыКлиентовToolStripMenuItem.Text = "Заказы клиентов";
            this.заказыКлиентовToolStripMenuItem.Click += new System.EventHandler(this.заказыКлиентовToolStripMenuItem_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView.Location = new System.Drawing.Point(0, 24);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(873, 277);
            this.dataGridView.TabIndex = 3;
            // 
            // buttonCreateOrdering
            // 
            this.buttonCreateOrdering.Location = new System.Drawing.Point(911, 40);
            this.buttonCreateOrdering.Name = "buttonCreateOrdering";
            this.buttonCreateOrdering.Size = new System.Drawing.Size(149, 23);
            this.buttonCreateOrdering.TabIndex = 4;
            this.buttonCreateOrdering.Text = "Создать заказ";
            this.buttonCreateOrdering.UseVisualStyleBackColor = true;
            this.buttonCreateOrdering.Click += new System.EventHandler(this.buttonCreateOrdering_Click);
            // 
            // buttonTakeOrderingInWork
            // 
            this.buttonTakeOrderingInWork.Location = new System.Drawing.Point(911, 87);
            this.buttonTakeOrderingInWork.Name = "buttonTakeOrderingInWork";
            this.buttonTakeOrderingInWork.Size = new System.Drawing.Size(149, 23);
            this.buttonTakeOrderingInWork.TabIndex = 5;
            this.buttonTakeOrderingInWork.Text = "Отдать на выполнение";
            this.buttonTakeOrderingInWork.UseVisualStyleBackColor = true;
            this.buttonTakeOrderingInWork.Click += new System.EventHandler(this.buttonTakeOrderingInWork_Click);
            // 
            // buttonOrderingReady
            // 
            this.buttonOrderingReady.Location = new System.Drawing.Point(911, 135);
            this.buttonOrderingReady.Name = "buttonOrderingReady";
            this.buttonOrderingReady.Size = new System.Drawing.Size(149, 23);
            this.buttonOrderingReady.TabIndex = 6;
            this.buttonOrderingReady.Text = "Заказ готов";
            this.buttonOrderingReady.UseVisualStyleBackColor = true;
            this.buttonOrderingReady.Click += new System.EventHandler(this.buttonOrderingReady_Click);
            // 
            // buttonPayOrdering
            // 
            this.buttonPayOrdering.Location = new System.Drawing.Point(911, 186);
            this.buttonPayOrdering.Name = "buttonPayOrdering";
            this.buttonPayOrdering.Size = new System.Drawing.Size(149, 23);
            this.buttonPayOrdering.TabIndex = 7;
            this.buttonPayOrdering.Text = "Заказ оплачен";
            this.buttonPayOrdering.UseVisualStyleBackColor = true;
            this.buttonPayOrdering.Click += new System.EventHandler(this.buttonPayOrdering_Click);
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(911, 235);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(149, 23);
            this.buttonRef.TabIndex = 8;
            this.buttonRef.Text = "Обновить список";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // письмаToolStripMenuItem
            // 
            this.письмаToolStripMenuItem.Name = "письмаToolStripMenuItem";
            this.письмаToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.письмаToolStripMenuItem.Text = "Письма";
            this.письмаToolStripMenuItem.Click += new System.EventHandler(this.письмаToolStripMenuItem_Click);
            // 
            // FormBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 301);
            this.Controls.Add(this.buttonRef);
            this.Controls.Add(this.buttonPayOrdering);
            this.Controls.Add(this.buttonOrderingReady);
            this.Controls.Add(this.buttonTakeOrderingInWork);
            this.Controls.Add(this.buttonCreateOrdering);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip1);
            this.Name = "FormBasic";
            this.Text = "Ремонт сантехники";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem клиентыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem запчастиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem услугиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem складыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сантехникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пополнитьСкладToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonCreateOrdering;
        private System.Windows.Forms.Button buttonTakeOrderingInWork;
        private System.Windows.Forms.Button buttonOrderingReady;
        private System.Windows.Forms.Button buttonPayOrdering;
        private System.Windows.Forms.Button buttonRef;
        private System.Windows.Forms.ToolStripMenuItem отчётыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem прайсУслугToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загруженностьСкладовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заказыКлиентовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem письмаToolStripMenuItem;
    }
}