using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<PlumberViewModel> list = Task.Run(() => APIClient.GetRequestData<List<PlumberViewModel>>("api/Plumber/GetList")).Result;
                if (list != null)
                {
                    comboBoxPlumber.DisplayMember = "PlumberFIO";
                    comboBoxPlumber.ValueMember = "Id";
                    comboBoxPlumber.DataSource = list;
                    comboBoxPlumber.SelectedItem = null;
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
            if (comboBoxPlumber.SelectedValue == null)
            {
                MessageBox.Show("Выберите сантехника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int plumberId = Convert.ToInt32(comboBoxPlumber.SelectedValue);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Basic/TakeOrderingInWork", new OrderingBindingModel
                {
                    Id = id.Value,
                    PlumberId = plumberId
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Заказ передан в работу. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
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
