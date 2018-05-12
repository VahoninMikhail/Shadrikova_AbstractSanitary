using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractSanitaryView
{
    public partial class FormCreateOrdering : Form
    {
        public FormCreateOrdering()
        {
            InitializeComponent();
        }

        private void FormCreateOrdering_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<CustomerViewModel>>("api/Customer/GetList")).Result;
                if (listC != null)
                {
                    comboBoxCustomer.DisplayMember = "CustomerFIO";
                    comboBoxCustomer.ValueMember = "Id";
                    comboBoxCustomer.DataSource = listC;
                    comboBoxCustomer.SelectedItem = null;
                }

                List<ItemViewModel> listI = Task.Run(() => APIClient.GetRequestData<List<ItemViewModel>>("api/Item/GetList")).Result;
                if (listI != null)
                {
                    comboBoxItem.DisplayMember = "ItemName";
                    comboBoxItem.ValueMember = "Id";
                    comboBoxItem.DataSource = listI;
                    comboBoxItem.SelectedItem = null;
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

        private void CalcSum()
        {
            if (comboBoxItem.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxItem.SelectedValue);
                    ItemViewModel product = Task.Run(() => APIClient.GetRequestData<ItemViewModel>("api/Item/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)product.Price).ToString();
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
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxCustomer.SelectedValue == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxItem.SelectedValue == null)
            {
                MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int customerId = Convert.ToInt32(comboBoxCustomer.SelectedValue);
            int itemId = Convert.ToInt32(comboBoxItem.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIClient.PostRequestData("api/Basic/CreateOrdering", new OrderingBindingModel
            {
                CustomerId = customerId,
                ItemId = itemId,
                Count = count,
                Sum = sum
            }));

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
