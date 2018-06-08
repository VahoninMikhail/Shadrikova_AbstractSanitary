using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowPlumbers.xaml
    /// </summary>
    public partial class WindowPlumbers : Window
    {
        public WindowPlumbers()
        {
            InitializeComponent();
            Loaded += WindowPlumbers_Load;
        }

        private void WindowPlumbers_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Plumber/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<PlumberViewModel> list = APIClient.GetElement<List<PlumberViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewPlumbers.ItemsSource = list;
                        dataGridViewPlumbers.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewPlumbers.Columns[1].Width = DataGridLength.Auto;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new WindowPlumber();
            if (form.ShowDialog() == true)
            LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewPlumbers.SelectedItem != null)
            {
                var form = new WindowPlumber();
                form.Id = ((PlumberViewModel)dataGridViewPlumbers.SelectedItem).Id;
                if (form.ShowDialog() == true)
                LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewPlumbers.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((PlumberViewModel)dataGridViewPlumbers.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Plumber/DelElement", new CustomerBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
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
