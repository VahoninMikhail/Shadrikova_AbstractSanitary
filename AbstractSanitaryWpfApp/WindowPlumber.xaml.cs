﻿using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowPlumber.xaml
    /// </summary>
    public partial class WindowPlumber : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        public WindowPlumber()
        {
            InitializeComponent();
            Loaded += WindowPlumber_Load;
        }

        private void WindowPlumber_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var plumber = Task.Run(() => APIClient.GetRequestData<PlumberViewModel>("api/Plumber/Get/" + id.Value)).Result;
                    textBoxFullName.Text = plumber.PlumberFIO;
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
            if (string.IsNullOrEmpty(textBoxFullName.Text))
            {
                MessageBox.Show("Введите ФИО", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string fio = textBoxFullName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Plumber/UpdElement", new PlumberBindingModel
                {
                    Id = id.Value,
                    PlumberFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Plumber/AddElement", new PlumberBindingModel
                {
                    PlumberFIO = fio
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
