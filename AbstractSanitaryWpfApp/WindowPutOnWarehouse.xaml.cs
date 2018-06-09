using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<PartViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<PartViewModel>>("api/Part/GetList")).Result;
                if (listP != null)
                {
                    comboBoxPart.DisplayMemberPath = "PartName";
                    comboBoxPart.SelectedValuePath = "Id";
                    comboBoxPart.ItemsSource = listP;
                    comboBoxPart.SelectedItem = null;
                }
                List<WarehouseViewModel> listW = Task.Run(() => APIClient.GetRequestData<List<WarehouseViewModel>>("api/Warehouse/GetList")).Result;
                if (listW != null)
                {
                    comboBoxWarehouse.DisplayMemberPath = "WarehouseName";
                    comboBoxWarehouse.SelectedValuePath = "Id";
                    comboBoxWarehouse.ItemsSource = listW;
                    comboBoxWarehouse.SelectedItem = null;
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
                int partId = Convert.ToInt32(comboBoxPart.SelectedValue);
                int warehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Basic/PutPartOnWarehouse", new WarehousePartBindingModel
                {
                    PartId = partId,
                    WarehouseId = warehouseId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Склад пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
