using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsList
{
    public class WarehouseServiceList : IWarehouseService
    {
        private ListDataSingleton source;

        public WarehouseServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<WarehouseViewModel> GetList()
        {
            List<WarehouseViewModel> result = source.Warehouses
                .Select(rec => new WarehouseViewModel
                {
                    Id = rec.Id,
                    WarehouseName = rec.WarehouseName,
                    WarehouseParts = source.WarehouseParts
                            .Where(recWP => recWP.WarehouseId == rec.Id)
                            .Select(recWP => new WarehousePartViewModel
                            {
                                Id = recWP.Id,
                                WarehouseId = recWP.WarehouseId,
                                PartId = recWP.PartId,
                                PartName = source.Parts
                                    .FirstOrDefault(recP => recP.Id == recWP.PartId)?.PartName,
                                Count = recWP.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public WarehouseViewModel GetElement(int id)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WarehouseViewModel
                {
                    Id = element.Id,
                    WarehouseName = element.WarehouseName,
                    WarehouseParts = source.WarehouseParts
                            .Where(recWP => recWP.WarehouseId == element.Id)
                            .Select(recWP => new WarehousePartViewModel
                            {
                                Id = recWP.Id,
                                WarehouseId = recWP.WarehouseId,
                                PartId = recWP.PartId,
                                PartName = source.Parts
                                    .FirstOrDefault(recP => recP.Id == recWP.PartId)?.PartName,
                                Count = recWP.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WarehouseBindingModel model)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Warehouses.Count > 0 ? source.Warehouses.Max(rec => rec.Id) : 0;
            source.Warehouses.Add(new Warehouse
            {
                Id = maxId + 1,
                WarehouseName = model.WarehouseName
            });
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec =>
                                        rec.WarehouseName == model.WarehouseName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WarehouseName = model.WarehouseName;
        }

        public void DelElement(int id)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.WarehouseParts.RemoveAll(rec => rec.WarehouseId == id);
                source.Warehouses.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
