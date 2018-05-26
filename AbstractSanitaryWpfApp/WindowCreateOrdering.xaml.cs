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
    /// Логика взаимодействия для WindowCreateOrdering.xaml
    /// </summary>
    public partial class WindowCreateOrdering : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ICustomerService serviceCustomer;

        private readonly IItemService serviceItem;

        private readonly IBasicService serviceBasic;


        public WindowCreateOrdering(ICustomerService serviceC, IItemService serviceI, IBasicService serviceB)
        {
            InitializeComponent();
            Loaded += WindowCreateOrdering_Load;
            comboBoxItem.SelectionChanged += comboBoxItem_SelectedIndexChanged;

            comboBoxItem.SelectionChanged += new SelectionChangedEventHandler(comboBoxItem_SelectedIndexChanged);
            this.serviceCustomer = serviceC;
            this.serviceItem = serviceI;
            this.serviceBasic = serviceB;
        }

        private void WindowCreateOrdering_Load(object sender, EventArgs e)
        {
            try
            {
                List<CustomerViewModel> listCustomer = serviceCustomer.GetList();
                if (listCustomer != null)
                {
                    comboBoxCustomer.DisplayMemberPath = "CustomerFIO";
                    comboBoxCustomer.SelectedValuePath = "Id";
                    comboBoxCustomer.ItemsSource = listCustomer;
                    comboBoxItem.SelectedItem = null;
                }
                List<ItemViewModel> listItem = serviceItem.GetList();
                if (listItem != null)
                {
                    comboBoxItem.DisplayMemberPath = "ItemName";
                    comboBoxItem.SelectedValuePath = "Id";
                    comboBoxItem.ItemsSource = listItem;
                    comboBoxItem.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxItem.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((ItemViewModel)comboBoxItem.SelectedItem).Id;
                    ItemViewModel product = serviceItem.GetElement(id);
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product.Price).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Введите количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxCustomer.SelectedItem == null)
            {
                MessageBox.Show("Выберите заказчика", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxItem.SelectedItem == null)
            {
                MessageBox.Show("Выберите услугу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceBasic.CreateOrdering(new OrderingBindingModel
                {
                    CustomerId = ((CustomerViewModel)comboBoxCustomer.SelectedItem).Id,
                    ItemId = ((ItemViewModel)comboBoxItem.SelectedItem).Id,
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
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
