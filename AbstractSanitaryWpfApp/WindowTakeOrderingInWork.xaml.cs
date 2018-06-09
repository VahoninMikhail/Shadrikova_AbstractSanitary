using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowTakeOrderingInWork.xaml
    /// </summary>
    public partial class WindowTakeOrderingInWork : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public WindowTakeOrderingInWork()
        {
            InitializeComponent();
            Loaded += WindowTakeOrderingInWork_Load;
        }

        private void WindowTakeOrderingInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<PlumberViewModel> list = Task.Run(() => APIClient.GetRequestData<List<PlumberViewModel>>("api/Plumber/GetList")).Result;
                if (list != null)
                {
                    comboBoxPlumber.DisplayMemberPath = "PlumberFIO";
                    comboBoxPlumber.SelectedValuePath = "Id";
                    comboBoxPlumber.ItemsSource = list;
                    comboBoxPlumber.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxPlumber.SelectedItem == null)
            {
                MessageBox.Show("Выберите сантехника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                task.ContinueWith((prevTask) => MessageBox.Show("Заказ передан в работу. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                    TaskContinuationOptions.OnlyOnRanToCompletion);
                task.ContinueWith((prevTask) =>
                {
                    var ex = (Exception)prevTask.Exception;
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }, TaskContinuationOptions.OnlyOnFaulted);

                Close();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
