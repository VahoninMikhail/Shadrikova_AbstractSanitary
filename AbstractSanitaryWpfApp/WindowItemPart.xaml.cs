using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowItemPart.xaml
    /// </summary>
    public partial class WindowItemPart : Window
    {
        public ItemPartViewModel Model { set { model = value; } get { return model; } }

        private ItemPartViewModel model;

        public WindowItemPart()
        {
            InitializeComponent();
            Loaded += WindowItemPart_Load;
        }

        private void WindowItemPart_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Part/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxPart.DisplayMemberPath = "PartName";
                    comboBoxPart.SelectedValuePath = "Id";
                    comboBoxPart.ItemsSource = APIClient.GetElement<List<PartViewModel>>(response);
                    comboBoxPart.SelectedItem = null;
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

            if (model != null)
            {
                comboBoxPart.IsEnabled = false;
                comboBoxPart.SelectedValue = model.PartId;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxPart.SelectedItem == null)
            {
                MessageBox.Show("Выберите запчасть", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new ItemPartViewModel
                    {
                        PartId = Convert.ToInt32(comboBoxPart.SelectedValue),
                        PartName = comboBoxPart.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
