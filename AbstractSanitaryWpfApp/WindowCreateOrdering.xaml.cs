using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
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
                var responseC = APIClient.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<CustomerViewModel> list = APIClient.GetElement<List<CustomerViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                        comboBoxCustomer.SelectedValuePath = "Id";
                        comboBoxCustomer.ItemsSource = list;
                        comboBoxCustomer.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseC));
                }
                var responseI = APIClient.GetRequest("api/Item/GetList");
                if (responseI.Result.IsSuccessStatusCode)
                {
                    List<ItemViewModel> list = APIClient.GetElement<List<ItemViewModel>>(responseI);
                    if (list != null)
                    {
                        comboBoxItem.DisplayMemberPath = "ItemName";
                        comboBoxItem.SelectedValuePath = "Id";
                        comboBoxItem.ItemsSource = list;
                        comboBoxItem.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseI));
                }
            }
            catch (Exception ex)
            {
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
                    var responseI = APIClient.GetRequest("api/Item/Get/" + id);
                    if (responseI.Result.IsSuccessStatusCode)
                    {
                        ItemViewModel mebel = APIClient.GetElement<ItemViewModel>(responseI);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)mebel.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(responseI));
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                var response = APIClient.PostRequest("api/Basic/CreateOrdering", new OrderingBindingModel
                {
                    CustomerId = Convert.ToInt32(comboBoxCustomer.SelectedItem),
                    ItemId = Convert.ToInt32(comboBoxItem.SelectedItem),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
