using AbstractSanitaryService.Attributies;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    [CustomInterface("Интерфейс для работы со складами")]
    public interface IWarehouseService
    {
        [CustomMethod("Метод получения списка складов")]
        List<WarehouseViewModel> GetList();

        [CustomMethod("Метод получения склада по id")]
        WarehouseViewModel GetElement(int id);

        [CustomMethod("Метод добавления склада")]
        void AddElement(WarehouseBindingModel model);

        [CustomMethod("Метод изменения данных по складу")]
        void UpdElement(WarehouseBindingModel model);

        [CustomMethod("Метод удаления склада")]
        void DelElement(int id);
    }
}
