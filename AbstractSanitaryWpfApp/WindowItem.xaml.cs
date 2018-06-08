using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowItem.xaml
    /// </summary>
    public partial class WindowItem : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<ItemPartViewModel> itemParts;

        public WindowItem()
        {
            InitializeComponent();
            Loaded += WindowItem_Load;
        }

        private void WindowItem_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Item/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var item = APIClient.GetElement<ItemViewModel>(response);
                        textBoxName.Text = item.ItemName;
                        textBoxPrice.Text = item.Price.ToString();
                        itemParts = item.ItemParts;
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
            else
                itemParts = new List<ItemPartViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (itemParts != null)
                {
                    dataGridViewItem.ItemsSource = null;
                    dataGridViewItem.ItemsSource = itemParts;
                    dataGridViewItem.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewItem.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewItem.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewItem.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new WindowItemPart();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    form.Model.ItemId = id.Value;
                    itemParts.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewItem.SelectedItem != null)
            {
                var form = new WindowItemPart();
                form.Model = itemParts[dataGridViewItem.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    itemParts[dataGridViewItem.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewItem.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        itemParts.RemoveAt(dataGridViewItem.SelectedIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Введите название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Введите цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (itemParts == null || itemParts.Count == 0)
            {
                MessageBox.Show("Выберите запчасти", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<ItemPartBindingModel> itemPartBM = new List<ItemPartBindingModel>();
                for (int i = 0; i < itemParts.Count; ++i)
                {
                    itemPartBM.Add(new ItemPartBindingModel
                    {
                        Id = itemParts[i].Id,
                        ItemId = itemParts[i].ItemId,
                        PartId = itemParts[i].PartId,
                        Count = itemParts[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Item/UpdElement", new ItemBindingModel
                    {
                        Id = id.Value,
                        ItemName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ItemParts = itemPartBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Item/AddElement", new ItemBindingModel
                    {
                        ItemName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ItemParts = itemPartBM
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
