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
    /// Логика взаимодействия для WindowBasic.xaml
    /// </summary>
    public partial class WindowBasic : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IBasicService service;

        public WindowBasic(IBasicService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void LoadData()
        {
            try
            {
                List<OrderingViewModel> list = service.GetList();
                if (list != null)
                {
                    dataGridViewBasic.ItemsSource = list;
                    dataGridViewBasic.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewBasic.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewBasic.Columns[3].Visibility = Visibility.Hidden;
                    dataGridViewBasic.Columns[5].Visibility = Visibility.Hidden;
                    dataGridViewBasic.Columns[1].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void заказчикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowCustomers>();
            form.ShowDialog();
        }

        private void запчастиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowParts>();
            form.ShowDialog();
        }

        private void услугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowItems>();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowWarehouses>();
            form.ShowDialog();
        }

        private void сантехникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowPlumbers>();
            form.ShowDialog();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowPutOnWarehouse>();
            form.ShowDialog();
        }

        private void buttonCreateOrdering_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<WindowCreateOrdering>();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeOrderingInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                var form = Container.Resolve<WindowTakeOrderingInWork>();
                form.ID = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonOrderingReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                int id = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                try
                {
                    service.FinishOrdering(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonPayOrdering_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                int id = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                try
                {
                    service.PayOrdering(id);
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
