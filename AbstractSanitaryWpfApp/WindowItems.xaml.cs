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
    /// Логика взаимодействия для WindowItems.xaml
    /// </summary>
    public partial class WindowItems : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IItemService service;

        public WindowItems(IItemService service)
        {
            InitializeComponent();
            Loaded += WindowItems_Load;
            this.service = service;
        }

        private void WindowItems_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<ItemViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewItems.ItemsSource = list;
                    dataGridViewItems.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewItems.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewItems.Columns[3].Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowItem>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedItem != null)
            {
                var form = Container.Resolve<WindowItem>();
                form.ID = ((ItemViewModel)dataGridViewItems.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((ItemViewModel)dataGridViewItems.SelectedItem).Id;
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
