using AbstractSanitaryService.Attributies;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    [CustomInterface("Интерфейс для работы с отчетами")]
    public interface IReportService
    {
        [CustomMethod("Метод сохранения списка услуг в doc-файл")]
        void SaveItemPrice(ReportBindingModel model);

        [CustomMethod("Метод получения списка складов с количеством запчастей на них")]
        List<WarehousesLoadViewModel> GetWarehousesLoad();

        [CustomMethod("Метод сохранения списка списка складов с количеством запчастей на них в xls-файл")]
        void SaveWarehousesLoad(ReportBindingModel model);

        [CustomMethod("Метод получения списка заказов клиентов")]
        List<CustomerOrderingsModel> GetCustomerOrderings(ReportBindingModel model);

        [CustomMethod("Метод сохранения списка заказов клиентов в pdf-файл")]
        void SaveCustomerOrderings(ReportBindingModel model);
    }
}
