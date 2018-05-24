using AbstractSanitaryService.Attributies;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    [CustomInterface("Интерфейс для работы с запчастями")]
    public interface IPartService
    {
        [CustomMethod("Метод получения списка запчастей")]
        List<PartViewModel> GetList();

        [CustomMethod("Метод получения запчасти по id")]
        PartViewModel GetElement(int id);

        [CustomMethod("Метод добавления запчасти")]
        void AddElement(PartBindingModel model);

        [CustomMethod("Метод изменения данных по запчасти")]
        void UpdElement(PartBindingModel model);

        [CustomMethod("Метод удаления запчасти")]
        void DelElement(int id);
    }
}
