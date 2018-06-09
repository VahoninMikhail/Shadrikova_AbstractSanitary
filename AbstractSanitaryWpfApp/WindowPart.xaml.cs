using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowPart.xaml
    /// </summary>
    public partial class WindowPart : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public WindowPart()
        {
            InitializeComponent();
            Loaded += WindowPart_Load;
        }

        private void WindowPart_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var part = Task.Run(() => APIClient.GetRequestData<PartViewModel>("api/Part/Get/" + id.Value)).Result;
                    textBoxName.Text = part.PartName;
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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Part/UpdElement", new PartBindingModel
                {
                    Id = id.Value,
                    PartName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Part/AddElement", new PartBindingModel
                {
                    PartName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
