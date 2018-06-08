using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    public interface IReportService
    {
        void SaveItemPrice(ReportBindingModel model);

        List<WarehousesLoadViewModel> GetWarehousesLoad();

        void SaveWarehousesLoad(ReportBindingModel model);

        List<CustomerOrderingsViewModel> GetCustomerOrderings(ReportBindingModel model);

        void SaveCustomerOrderings(ReportBindingModel model);
    }
}