using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AbstractSanitaryService.ViewModels;
using AbstractSanitaryService.BindingModels;
using System.Threading.Tasks;

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
                List<PartViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<PartViewModel>>("api/Part/GetList")).Result;
                if (listP != null)
                {
                    comboBoxPart.DisplayMember = "PartName";
                    comboBoxPart.ValueMember = "Id";
                    comboBoxPart.DataSource = listP;
                    comboBoxPart.SelectedItem = null;
                }

                List<WarehouseViewModel> listW = Task.Run(() => APIClient.GetRequestData<List<WarehouseViewModel>>("api/Warehouse/GetList")).Result;
                if (listW != null)
                {
                    comboBoxWarehouse.DisplayMember = "WarehouseName";
                    comboBoxWarehouse.ValueMember = "Id";
                    comboBoxWarehouse.DataSource = listW;
                    comboBoxWarehouse.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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
                int partId = Convert.ToInt32(comboBoxPart.SelectedValue);
                int warehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Basic/PutPartOnWarehouse", new WarehousePartBindingModel
                {
                    PartId = partId,
                    WarehouseId = warehouseId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Склад пополнен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);

                Close();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
