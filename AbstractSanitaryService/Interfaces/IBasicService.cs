using AbstractSanitaryService.Attributies;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    [CustomInterface("Интерфейс для работы с заказами")]
    public interface IBasicService
    {
        [CustomMethod("Метод получения списка заказов")]
        List<OrderingViewModel> GetList();

        [CustomMethod("Метод создания заказа")]
        void CreateOrdering(OrderingBindingModel model);

        [CustomMethod("Метод передачи заказа в работу")]
        void TakeOrderingInWork(OrderingBindingModel model);

        [CustomMethod("Метод передачи заказа на оплату")]
        void FinishOrdering(int id);

        [CustomMethod("Метод фиксирования оплаты по заказу")]
        void PayOrdering(int id);

        [CustomMethod("Метод пополнения запчасти на складе")]
        void PutPartOnWarehouse(WarehousePartBindingModel model);
    }
}
