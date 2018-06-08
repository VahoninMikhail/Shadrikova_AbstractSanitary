using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
                var response = APIClient.GetRequest("api/Basic/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<OrderingViewModel> list = APIClient.GetElement<List<OrderingViewModel>>(response);
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
                try
                {
                    var response = APIClient.PostRequest("api/Basic/FinishOrdering", new OrderingBindingModel
                    {
                        Id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
        }

        private void buttonPayOrdering_Click(object sender, EventArgs e)
        {
            if (dataGridViewBasic.SelectedItem != null)
            {
                int id = ((OrderingViewModel)dataGridViewBasic.SelectedItem).Id;
                try
                {
                    var response = APIClient.PostRequest("api/Basic/PayOrdering", new OrderingBindingModel
                    {
                        Id = id
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        LoadData();
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
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveItemPrice", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }

        private void загруженностьСкладовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveWarehousesLoad", new ReportBindingModel
                    {
                        FileName = sfd.FileName
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        }

        private void заказыКлиентовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new WindowCustomerOrderings();
            form.ShowDialog();
        }
    }
}
