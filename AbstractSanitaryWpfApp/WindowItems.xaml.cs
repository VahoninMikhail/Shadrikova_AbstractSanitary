using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowItems.xaml
    /// </summary>
    public partial class WindowItems : Window
    {
        public WindowItems()
        {
            InitializeComponent();
            Loaded += WindowItems_Load;
        }

        private void WindowItems_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Item/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ItemViewModel> list = APIClient.GetElement<List<ItemViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewItems.ItemsSource = list;
                        dataGridViewItems.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewItems.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewItems.Columns[3].Visibility = Visibility.Hidden;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new WindowItem();
            if (form.ShowDialog() == true)
            LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedItem != null)
            {
                var form = new WindowItem();
                form.Id = ((ItemViewModel)dataGridViewItems.SelectedItem).Id;
                if (form.ShowDialog() == true)
                LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((ItemViewModel)dataGridViewItems.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Item/DelElement", new CustomerBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
