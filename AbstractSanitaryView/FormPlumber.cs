using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AbstractSanitaryView
{
    public partial class FormPlumber : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormPlumber()
        {
            InitializeComponent();
        }

        private void FormPlumber_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Plumber/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var plumber = APIClient.GetElement<PlumberViewModel>(response);
                        textBoxFIO.Text = plumber.PlumberFIO;
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Plumber/UpdElement", new PlumberBindingModel
                    {
                        Id = id.Value,
                        PlumberFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Plumber/AddElement", new PlumberBindingModel
                    {
                        PlumberFIO = textBoxFIO.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
