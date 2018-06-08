using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AbstractSanitaryWpfApp
{
    /// <summary>
    /// Логика взаимодействия для WindowCustomerOrderings.xaml
    /// </summary>
    public partial class WindowCustomerOrderings : Window
    {
        public WindowCustomerOrderings()
        {
            InitializeComponent();
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                reportViewer.LocalReport.ReportEmbeddedResource = "MebelFactoryViewWpf.ReportCustomerOrderings.rdlc";
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",
                                            "c " + Convert.ToDateTime(dateTimePickerFrom.SelectedDate).ToString("dd-MM") +
                                            " по " + Convert.ToDateTime(dateTimePickerTo.SelectedDate).ToString("dd-MM"));
                reportViewer.LocalReport.SetParameters(parameter);


                var response = APIClient.PostRequest("api/Report/GetCustomerOrderings", new ReportBindingModel
                {
                    DateFrom = dateTimePickerFrom.SelectedDate,
                    DateTo = dateTimePickerTo.SelectedDate
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    var dataSource = APIClient.GetElement<List<CustomerOrderingsViewModel>>(response);
                    ReportDataSource source = new ReportDataSource("DataSetOrderings", dataSource);
                    reportViewer.LocalReport.DataSources.Add(source);
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate >= dateTimePickerTo.SelectedDate)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    var response = APIClient.PostRequest("api/Report/SaveCustomerOrderings", new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate,
                        DateTo = dateTimePickerTo.SelectedDate
                    });
                    if (response.Result.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
