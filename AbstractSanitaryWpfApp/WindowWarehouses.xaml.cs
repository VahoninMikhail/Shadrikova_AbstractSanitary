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
                List<WarehouseViewModel> list = Task.Run(() => APIClient.GetRequestData<List<WarehouseViewModel>>("api/Warehouse/GetList")).Result;
                if (list != null)
                {
                    dataGridViewWarehouses.ItemsSource = list;
                    dataGridViewWarehouses.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWarehouses.Columns[1].Width = DataGridLength.Auto;
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
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Warehouse/DelElement", new CustomerBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
