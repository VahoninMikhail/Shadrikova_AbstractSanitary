using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowPutOnWarehouse.xaml
    /// </summary>
    public partial class WindowPutOnWarehouse : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IWarehouseService serviceWarehouse;

        private readonly IPartService servicePart;

        private readonly IBasicService serviceBasic;

        public WindowPutOnWarehouse(IWarehouseService serviceW, IPartService serviceP, IBasicService serviceB)
        {
            InitializeComponent();
            Loaded += WindowPutOnWarehouse_Load;
            this.serviceWarehouse = serviceW;
            this.servicePart = serviceP;
            this.serviceBasic = serviceB;
        }

        private void WindowPutOnWarehouse_Load(object sender, EventArgs e)
        {
            try
            {
                List<PartViewModel> listPart = servicePart.GetList();
                if (listPart != null)
                {
                    comboBoxPart.DisplayMemberPath = "PartName";
                    comboBoxPart.SelectedValuePath = "Id";
                    comboBoxPart.ItemsSource = listPart;
                    comboBoxPart.SelectedItem = null;
                }
                List<WarehouseViewModel> listWarehouse = serviceWarehouse.GetList();
                if (listWarehouse != null)
                {
                    comboBoxWarehouse.DisplayMemberPath = "WarehouseName";
                    comboBoxWarehouse.SelectedValuePath = "Id";
                    comboBoxWarehouse.ItemsSource = listWarehouse;
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
                serviceBasic.PutPartOnWarehouse(new WarehousePartBindingModel
                {
                    PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
                    WarehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
