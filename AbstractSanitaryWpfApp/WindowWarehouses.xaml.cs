using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowWarehouses.xaml
    /// </summary>
    public partial class WindowWarehouses : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IWarehouseService service;

        public WindowWarehouses(IWarehouseService service)
        {
            InitializeComponent();
            Loaded += WindowWarehouses_Load;
            this.service = service;
        }

        private void WindowWarehouses_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<WarehouseViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewWarehouses.ItemsSource = list;
                    dataGridViewWarehouses.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewWarehouses.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowWarehouse>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewWarehouses.SelectedItem != null)
            {
                var form = Container.Resolve<WindowWarehouse>();
                form.ID = ((WarehouseViewModel)dataGridViewWarehouses.SelectedItem).Id;
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
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((WarehouseViewModel)dataGridViewWarehouses.SelectedItem).Id;
                    try
                    {
                        service.DelElement(id);
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
