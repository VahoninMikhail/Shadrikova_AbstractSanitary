using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.ViewModels;
using System.Collections.Generic;

namespace AbstractSanitaryService.Interfaces
{
    public interface IWarehouseService
    {
        List<WarehouseViewModel> GetList();

        WarehouseViewModel GetElement(int id);

        void AddElement(WarehouseBindingModel model);

        void UpdElement(WarehouseBindingModel model);

        void DelElement(int id);
    }
}
