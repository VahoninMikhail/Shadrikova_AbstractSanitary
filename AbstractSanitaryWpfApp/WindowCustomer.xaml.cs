using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowCustomer.xaml
    /// </summary>
    public partial class WindowCustomer : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public WindowCustomer()
        {
            InitializeComponent();
            Loaded += WindowCustomer_Load;
        }

        private void WindowCustomer_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Customer/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var customer = APIClient.GetElement<CustomerViewModel>(response);
                        textBoxFIO.Text = customer.CustomerFIO;
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Customer/UpdElement", new CustomerBindingModel
                    {
                        Id = id.Value,
                        CustomerFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Customer/AddElement", new CustomerBindingModel
                    {
                        CustomerFIO = textBoxFIO.Text
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
