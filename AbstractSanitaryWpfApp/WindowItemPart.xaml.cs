using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using Unity;
using Unity.Attributes;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowItemPart.xaml
    /// </summary>
    public partial class WindowItemPart : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public ItemPartViewModel Model { set { model = value; } get { return model; } }

        private readonly IPartService service;

        private ItemPartViewModel model;

        public WindowItemPart(IPartService service)
        {
            InitializeComponent();
            Loaded += WindowItemPart_Load;
            this.service = service;
        }

        private void WindowItemPart_Load(object sender, EventArgs e)
        {
            List<PartViewModel> list = service.GetList();
            try
            {
                if (list != null)
                {
                    comboBoxPart.DisplayMemberPath = "PartName";
                    comboBoxPart.SelectedValuePath = "Id";
                    comboBoxPart.ItemsSource = list;
                    comboBoxPart.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxPart.IsEnabled = false;
                foreach (PartViewModel item in list)
                {
                    if (item.PartName == model.PartName)
                    {
                        comboBoxPart.SelectedItem = item;
                    }
                }
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
