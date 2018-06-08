using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowWarehouses.xaml
    /// </summary>
    public partial class WindowWarehouses : Window
    {
        public WindowWarehouses()
        {
            InitializeComponent();
            Loaded += WindowWarehouses_Load;
        }

        private void WindowWarehouses_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Warehouse/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<WarehouseViewModel> list = APIClient.GetElement<List<WarehouseViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewWarehouses.ItemsSource = list;
                        dataGridViewWarehouses.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewWarehouses.Columns[1].Width = DataGridLength.Auto;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new WindowWarehouse();
            if (form.ShowDialog() == true)
            LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWarehouses.SelectedItem != null)
            {
                var form = new WindowWarehouse();
                form.Id = ((WarehouseViewModel)dataGridViewWarehouses.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewWarehouses.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((WarehouseViewModel)dataGridViewWarehouses.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Warehouse/DelElement", new CustomerBindingModel { Id = id });
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
