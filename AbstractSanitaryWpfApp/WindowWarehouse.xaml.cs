using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowWarehouse.xaml
    /// </summary>
    public partial class WindowWarehouse : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public WindowWarehouse()
        {
            InitializeComponent();
            Loaded += WindowWarehouse_Load;
        }

        private void WindowWarehouse_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var warehouse = Task.Run(() => APIClient.GetRequestData<WarehouseViewModel>("api/Warehouse/Get/" + id.Value)).Result;
                    textBoxName.Text = warehouse.WarehouseName;
                    dataGridViewWarehouse.ItemsSource = warehouse.WarehouseParts;
                    dataGridViewWarehouse.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWarehouse.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewWarehouse.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewWarehouse.Columns[3].Width = DataGridLength.Auto;
                    
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Warehouse/UpdElement", new WarehouseBindingModel
                {
                    Id = id.Value,
                    WarehouseName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Warehouse/AddElement", new WarehouseBindingModel
                {
                    WarehouseName = name
                }));
            }

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
