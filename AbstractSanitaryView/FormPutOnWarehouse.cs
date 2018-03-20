using AbstractSanitaryService.Interfaces;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;
using AbstractSanitaryService.ViewModels;
using AbstractSanitaryService.BindingModels;

namespace AbstractSanitaryView
{
    public partial class FormPutOnWarehouse : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IWarehouseService serviceW;

        private readonly IPartService serviceP;

        private readonly IBasicService serviceB;

        public FormPutOnWarehouse(IWarehouseService serviceW, IPartService serviceP, IBasicService serviceB)
        {
            InitializeComponent();
            this.serviceW = serviceW;
            this.serviceP = serviceP;
            this.serviceB = serviceB;
        }

        private void FormPutOnWarehouse_Load(object sender, EventArgs e)
        {
            try
            {
                List<PartViewModel> listC = serviceP.GetList();
                if (listC != null)
                {
                    comboBoxPart.DisplayMember = "PartName";
                    comboBoxPart.ValueMember = "Id";
                    comboBoxPart.DataSource = listC;
                    comboBoxPart.SelectedItem = null;
                }
                List<WarehouseViewModel> listS = serviceW.GetList();
                if (listS != null)
                {
                    comboBoxWarehouse.DisplayMember = "WarehouseName";
                    comboBoxWarehouse.ValueMember = "Id";
                    comboBoxWarehouse.DataSource = listS;
                    comboBoxWarehouse.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxPart.SelectedValue == null)
            {
                MessageBox.Show("Выберите запчасть", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceB.PutPartOnWarehouse(new WarehousePartBindingModel
                {
                    PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
                    WarehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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
