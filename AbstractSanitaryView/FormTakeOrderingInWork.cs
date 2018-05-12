using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AbstractSanitaryView
{
    public partial class FormTakeOrderingInWork : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormTakeOrderingInWork()
        {
            InitializeComponent();
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
                var response = APIClient.GetRequest("api/Plumber/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<PlumberViewModel> list = APIClient.GetElement<List<PlumberViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxPlumber.DisplayMember = "PlumberFIO";
                        comboBoxPlumber.ValueMember = "Id";
                        comboBoxPlumber.DataSource = list;
                        comboBoxPlumber.SelectedItem = null;
                    }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxPlumber.SelectedValue == null)
            {
                MessageBox.Show("Выберите сантехника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Basic/TakeOrderingInWork", new OrderingBindingModel
                {
                    Id = id.Value,
                    PlumberId = Convert.ToInt32(comboBoxPlumber.SelectedValue)
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
