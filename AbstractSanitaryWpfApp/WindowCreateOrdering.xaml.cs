using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowCreateOrdering.xaml
    /// </summary>
    public partial class WindowCreateOrdering : Window
    {
        public WindowCreateOrdering()
        {
            InitializeComponent();
            Loaded += WindowCreateOrdering_Load;
            comboBoxItem.SelectionChanged += comboBoxItem_SelectedIndexChanged;
            comboBoxItem.SelectionChanged += new SelectionChangedEventHandler(comboBoxItem_SelectedIndexChanged);
        }

        private void WindowCreateOrdering_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<CustomerViewModel>>("api/Customer/GetList")).Result;
                if (listC != null)
                {
                    comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                    comboBoxCustomer.SelectedValuePath = "Id";
                    comboBoxCustomer.ItemsSource = listC;
                    comboBoxItem.SelectedItem = null;
                }

                List<ItemViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<ItemViewModel>>("api/Item/GetList")).Result;
                if (listP != null)
                {
                    comboBoxItem.DisplayMemberPath = "ItemName";
                    comboBoxItem.SelectedValuePath = "Id";
                    comboBoxItem.ItemsSource = listP;
                    comboBoxItem.SelectedItem = null;
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

        private void CalcSum()
        {
            if (comboBoxItem.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((ItemViewModel)comboBoxItem.SelectedItem).Id;
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
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxCustomer.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxItem.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
