using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowBasic.xaml
    /// </summary>
    public partial class WindowBasic : Window
    {
        public WindowBasic()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<OrderingViewModel> list = Task.Run(() => APIClient.GetRequestData<List<OrderingViewModel>>("api/Basic/GetList")).Result;
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
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void заказчикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowCustomers();
            form.ShowDialog();
        }

        private void запчастиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowParts();
            form.ShowDialog();
        }

        private void услугиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowItems();
            form.ShowDialog();
        }

        private void складыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowWarehouses();
            form.ShowDialog();
        }

        private void сантехникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowPlumbers();
            form.ShowDialog();
        }

        private void пополнитьСкладToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowPutOnWarehouse();
            form.ShowDialog();
        }

        private void buttonCreateOrdering_Click(object sender, EventArgs e)
        {
            var form = new WindowCreateOrdering();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeOrderingInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                var form = new WindowTakeOrderingInWork();
                form.Id = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonOrderingReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                int id = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Basic/FinishOrdering", new OrderingBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonPayOrdering_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                int id = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Basic/PayOrdering", new OrderingBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заказа изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсУслугToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };

            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveItemPrice", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void загруженностьСкладовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveWarehousesLoad", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowCustomerOrderings();
            form.ShowDialog();
        }
    }
}
