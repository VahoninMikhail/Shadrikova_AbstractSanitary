using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Unity;
using Unity.Attributes;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowItem.xaml
    /// </summary>
    public partial class WindowItem : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IItemService service;

        private int? id;

        private List<ItemPartViewModel> itemParts;

        public WindowItem(IItemService service)
        {
            InitializeComponent();
            Loaded += WindowItem_Load;
            this.service = service;
        }

        private void WindowItem_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ItemViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.ItemName;
                        textBoxPrice.Text = view.Price.ToString();
                        itemParts = view.ItemParts;
                        LoadData();
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
            var form = Container.Resolve<WindowItemPart>();
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
                var form = Container.Resolve<WindowItemPart>();
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
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
                if (id.HasValue)
                {
                    service.UpdElement(new ItemBindingModel
                    {
                        Id = id.Value,
                        ItemName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ItemParts = itemPartBM
                    });
                }
                else
                {
                    service.AddElement(new ItemBindingModel
                    {
                        ItemName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        ItemParts = itemPartBM
                    });
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
