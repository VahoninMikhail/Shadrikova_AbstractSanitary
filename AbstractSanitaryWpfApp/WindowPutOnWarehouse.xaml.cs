using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowPutOnWarehouse.xaml
    /// </summary>
    public partial class WindowPutOnWarehouse : Window
    {
        public WindowPutOnWarehouse()
        {
            InitializeComponent();
            Loaded += WindowPutOnWarehouse_Load;
        }

        private void WindowPutOnWarehouse_Load(object sender, EventArgs e)
        {
            try
            {
                var responseP = APIClient.GetRequest("api/Part/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<PartViewModel> list = APIClient.GetElement<List<PartViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxPart.DisplayMemberPath = "PartName";
                        comboBoxPart.SelectedValuePath = "Id";
                        comboBoxPart.ItemsSource = list;
                        comboBoxPart.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }
                var responseW = APIClient.GetRequest("api/Warehouse/GetList");
                if (responseW.Result.IsSuccessStatusCode)
                {
                    List<WarehouseViewModel> list = APIClient.GetElement<List<WarehouseViewModel>>(responseW);
                    if (list != null)
                    {
                        comboBoxWarehouse.DisplayMemberPath = "WarehouseName";
                        comboBoxWarehouse.SelectedValuePath = "Id";
                        comboBoxWarehouse.ItemsSource = list;
                        comboBoxWarehouse.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxPart.SelectedItem == null)
            {
                MessageBox.Show("Выберите запчасть", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxWarehouse.SelectedItem == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                var response = APIClient.PostRequest("api/Basic/PutPartOnWarehouse", new WarehousePartBindingModel
                {
                    PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
                    WarehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
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
