using AbstractSanitaryService.Attributies;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    [CustomInterface("Интерфейс для работы с сантехниками")]
    public interface IPlumberService
    {
        [CustomMethod("Метод получения списка сантехников")]
        List<PlumberViewModel> GetList();

        [CustomMethod("Метод получения сантехника по id")]
        PlumberViewModel GetElement(int id);

        [CustomMethod("Метод добавления сантехника")]
        void AddElement(PlumberBindingModel model);

        [CustomMethod("Метод изменения данных по сантехнику")]
        void UpdElement(PlumberBindingModel model);

        [CustomMethod("Метод удаления сантехника")]
        void DelElement(int id);
    }
}
