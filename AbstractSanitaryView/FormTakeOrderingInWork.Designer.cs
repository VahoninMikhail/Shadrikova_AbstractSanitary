namespace AbstractSanitaryView
{
    partial class FormTakeOrderingInWork
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.comboBoxPlumber = new System.Windows.Forms.ComboBox();
            this.labelPlumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(228, 33);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 14;
            this.buttonCancel.Text = "Отмена";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(130, 33);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 13;
            this.buttonSave.Text = "Сохранить";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // comboBoxPlumber
            // 
            this.comboBoxPlumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlumber.FormattingEnabled = true;
            this.comboBoxPlumber.Location = new System.Drawing.Point(86, 6);
            this.comboBoxPlumber.Name = "comboBoxPlumber";
            this.comboBoxPlumber.Size = new System.Drawing.Size(217, 21);
            this.comboBoxPlumber.TabIndex = 12;
            // 
            // labelPlumber
            // 
            this.labelPlumber.AutoSize = true;
            this.labelPlumber.Location = new System.Drawing.Point(12, 9);
            this.labelPlumber.Name = "labelPlumber";
            this.labelPlumber.Size = new System.Drawing.Size(63, 13);
            this.labelPlumber.TabIndex = 11;
            this.labelPlumber.Text = "Сантехник:";
            // 
            // FormTakeOrderingInWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 67);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.comboBoxPlumber);
            this.Controls.Add(this.labelPlumber);
            this.Name = "FormTakeOrderingInWork";
            this.Text = "Отдать заказ на выполнение";
            this.Load += new System.EventHandler(this.FormTakeOrderingInWork_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ComboBox comboBoxPlumber;
        private System.Windows.Forms.Label labelPlumber;
    }
}