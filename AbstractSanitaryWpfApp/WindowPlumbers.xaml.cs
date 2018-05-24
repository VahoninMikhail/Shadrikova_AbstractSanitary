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
    /// Логика взаимодействия для WindowPlumbers.xaml
    /// </summary>
    public partial class WindowPlumbers : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IPlumberService service;

        public WindowPlumbers(IPlumberService service)
        {
            InitializeComponent();
            Loaded += WindowPlumbers_Load;
            this.service = service;
        }

        private void WindowPlumbers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                List<PlumberViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewPlumbers.ItemsSource = list;
                    dataGridViewPlumbers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPlumbers.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowPlumber>();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPlumbers.SelectedItem != null)
            {
                var form = Container.Resolve<WindowPlumber>();
                form.ID = ((PlumberViewModel)dataGridViewPlumbers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPlumbers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((PlumberViewModel)dataGridViewPlumbers.SelectedItem).Id;
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
