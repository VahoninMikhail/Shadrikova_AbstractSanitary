using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowParts.xaml
    /// </summary>
    public partial class WindowParts : Window
    {
        public WindowParts()
        {
            InitializeComponent();
            Loaded += WindowParts_Load;
        }

        private void WindowParts_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Part/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<PartViewModel> list = APIClient.GetElement<List<PartViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewParts.ItemsSource = list;
                        dataGridViewParts.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewParts.Columns[1].Width = DataGridLength.Auto;
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
            var form = new WindowPart();
            if (form.ShowDialog() == true)
            LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewParts.SelectedItem != null)
            {
                var form = new WindowPart();
                form.Id = ((PartViewModel)dataGridViewParts.SelectedItem).Id;
                if (form.ShowDialog() == true)
                LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewParts.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((PartViewModel)dataGridViewParts.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Part/DelElement", new CustomerBindingModel { Id = id });
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
