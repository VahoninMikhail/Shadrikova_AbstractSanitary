using AbstractSanitaryService.Attributies;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    [CustomInterface("Интерфейс для работы с услугами")]
    public interface IItemService
    {
        [CustomMethod("Метод получения списка услуг")]
        List<ItemViewModel> GetList();

        [CustomMethod("Метод получения услуги по id")]
        ItemViewModel GetElement(int id);

        [CustomMethod("Метод добавления услуги")]
        void AddElement(ItemBindingModel model);

        [CustomMethod("Метод изменения данных по услуге")]
        void UpdElement(ItemBindingModel model);

        [CustomMethod("Метод удаления услуги")]
        void DelElement(int id);
    }
}
