using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractSanitaryView
{
    public partial class FormTakeOrderingInWork : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IPlumberService servicePl;

        private readonly IBasicService serviceB;

        private int? id;

        public FormTakeOrderingInWork(IPlumberService servicePl, IBasicService serviceB)
        {
            InitializeComponent();
            this.servicePl = servicePl;
            this.serviceB = serviceB;
        }

        private void FormTakeOrderingInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                List<PlumberViewModel> listI = servicePl.GetList();
                if (listI != null)
                {
                    comboBoxPlumber.DisplayMember = "PlumberFIO";
                    comboBoxPlumber.ValueMember = "Id";
                    comboBoxPlumber.DataSource = listI;
                    comboBoxPlumber.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxPlumber.SelectedValue == null)
            {
                MessageBox.Show("Выберите сантехника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceB.TakeOrderingInWork(new OrderingBindingModel
                {
                    Id = id.Value,
                    PlumberId = Convert.ToInt32(comboBoxPlumber.SelectedValue)
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
