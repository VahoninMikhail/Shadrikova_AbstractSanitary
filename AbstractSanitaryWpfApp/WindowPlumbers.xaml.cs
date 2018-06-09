using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                List<PlumberViewModel> list = Task.Run(() => APIClient.GetRequestData<List<PlumberViewModel>>("api/Plumber/GetList")).Result;
                if (list != null)
                {
                    dataGridViewPlumbers.ItemsSource = list;
                    dataGridViewPlumbers.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewPlumbers.Columns[1].Width = DataGridLength.Auto;
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
                    Task task = Task.Run(() => APIClient.PostRequestData("api/Plumber/DelElement", new CustomerBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
