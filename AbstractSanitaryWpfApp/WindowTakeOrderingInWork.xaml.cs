using AbstractSanitaryService.BindingModels;
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
    /// Логика взаимодействия для WindowTakeOrderingInWork.xaml
    /// </summary>
    public partial class WindowTakeOrderingInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly IPlumberService servicePlumber;

        private readonly IBasicService serviceBasic;

        private int? id;

        public WindowTakeOrderingInWork(IPlumberService servicePl, IBasicService serviceB)
        {
            InitializeComponent();
            Loaded += WindowTakeOrderingInWork_Load;
            this.servicePlumber = servicePl;
            this.serviceBasic = serviceB;
        }

        private void WindowTakeOrderingInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<PlumberViewModel> listPlumber = servicePlumber.GetList();
                if (listPlumber != null)
                {
                    comboBoxPlumber.DisplayMemberPath = "PlumberFIO";
                    comboBoxPlumber.SelectedValuePath = "Id";
                    comboBoxPlumber.ItemsSource = listPlumber;
                    comboBoxPlumber.SelectedItem = null;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxPlumber.SelectedItem == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceBasic.TakeOrderingInWork(new OrderingBindingModel
                {
                    Id = id.Value,
                    PlumberId = ((PlumberViewModel)comboBoxPlumber.SelectedItem).Id,
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
