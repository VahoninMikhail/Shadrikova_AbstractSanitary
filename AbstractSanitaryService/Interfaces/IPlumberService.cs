using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    public interface IPlumberService
    {
        List<PlumberViewModel> GetList();

        PlumberViewModel GetElement(int id);

        void AddElement(PlumberBindingModel model);

        void UpdElement(PlumberBindingModel model);

        void DelElement(int id);
    }
}
