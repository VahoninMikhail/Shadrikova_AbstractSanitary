using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AbstractSanitaryService.ViewModels;
using AbstractSanitaryService.BindingModels;

namespace AbstractSanitaryView
{
    public partial class FormPutOnWarehouse : Form
    {
        public FormPutOnWarehouse()
        {
            InitializeComponent();
        }

        private void FormPutOnWarehouse_Load(object sender, EventArgs e)
        {
            try
            {
                var responseP = APIClient.GetRequest("api/Part/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<PartViewModel> list = APIClient.GetElement<List<PartViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxPart.DisplayMember = "PartName";
                        comboBoxPart.ValueMember = "Id";
                        comboBoxPart.DataSource = list;
                        comboBoxPart.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }
                var responseW = APIClient.GetRequest("api/Warehouse/GetList");
                if (responseW.Result.IsSuccessStatusCode)
                {
                    List<WarehouseViewModel> list = APIClient.GetElement<List<WarehouseViewModel>>(responseW);
                    if (list != null)
                    {
                        comboBoxWarehouse.DisplayMember = "WarehouseName";
                        comboBoxWarehouse.ValueMember = "Id";
                        comboBoxWarehouse.DataSource = list;
                        comboBoxWarehouse.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
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
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Basic/PutPartOnWarehouse", new WarehousePartBindingModel
                {
                    PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
                    WarehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
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
