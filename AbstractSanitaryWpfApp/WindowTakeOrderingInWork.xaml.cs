using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
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
                var response = APIClient.GetRequest("api/Plumber/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<PlumberViewModel> list = APIClient.GetElement<List<PlumberViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxPlumber.DisplayMemberPath = "PlumberFIO";
                        comboBoxPlumber.SelectedValuePath = "Id";
                        comboBoxPlumber.ItemsSource = list;
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
                var response = APIClient.PostRequest("api/Basic/TakeOrderingInWork", new OrderingBindingModel
                {
                    Id = id.Value,
                    PlumberId = ((PlumberViewModel)comboBoxPlumber.SelectedItem).Id,
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
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
