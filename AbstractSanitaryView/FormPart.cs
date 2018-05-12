using System;
using System.Windows.Forms;
using AbstractSanitaryService.ViewModels;
using AbstractSanitaryService.BindingModels;
using System.Threading.Tasks;

namespace AbstractSanitaryView
{
    public partial class FormPart : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormPart()
        {
            InitializeComponent();
        }

        private void FormPart_Load(object sender, EventArgs e)
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
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
