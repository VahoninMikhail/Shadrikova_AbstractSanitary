using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Net.Http;
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
                    var response = APIClient.GetRequest("api/Warehouse/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var warehouse = APIClient.GetElement<WarehouseViewModel>(response);
                        textBoxName.Text = warehouse.WarehouseName;
                        dataGridViewWarehouse.ItemsSource = warehouse.WarehouseParts;
                        dataGridViewWarehouse.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewWarehouse.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewWarehouse.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewWarehouse.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
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
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Warehouse/UpdElement", new WarehouseBindingModel
                    {
                        Id = id.Value,
                        WarehouseName = textBoxName.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Warehouse/AddElement", new WarehouseBindingModel
                    {
                        WarehouseName = textBoxName.Text
                    });
                }
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
