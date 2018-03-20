using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    public interface IBasicService
    {
        List<OrderingViewModel> GetList();

        void CreateOrdering(OrderingBindingModel model);

        void TakeOrderingInWork(OrderingBindingModel model);

        void FinishOrdering(int id);

        void PayOrdering(int id);

        void PutPartOnWarehouse(WarehousePartBindingModel model);
    }
}
