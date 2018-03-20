using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<WarehouseViewModel> result = new List<WarehouseViewModel>();
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<WarehousePartViewModel> WarehouseParts = new List<WarehousePartViewModel>();
                for (int j = 0; j < source.WarehouseParts.Count; ++j)
                {
                    if (source.WarehouseParts[j].WarehouseId == source.Warehouses[i].Id)
                    {
                        string partName = string.Empty;
                        for (int k = 0; k < source.Parts.Count; ++k)
                        {
                            if (source.ItemParts[j].PartId == source.Parts[k].Id)
                            {
                                partName = source.Parts[k].PartName;
                                break;
                            }
                        }
                        WarehouseParts.Add(new WarehousePartViewModel
                        {
                            Id = source.WarehouseParts[j].Id,
                            WarehouseId = source.WarehouseParts[j].WarehouseId,
                            PartId = source.WarehouseParts[j].PartId,
                            PartName = partName,
                            Count = source.WarehouseParts[j].Count
                        });
                    }
                }
                result.Add(new WarehouseViewModel
                {
                    Id = source.Warehouses[i].Id,
                    WarehouseName = source.Warehouses[i].WarehouseName,
                    WarehouseParts = WarehouseParts
                });
            }
            return result;
        }

        public WarehouseViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<WarehousePartViewModel> WarehouseParts = new List<WarehousePartViewModel>();
                for (int j = 0; j < source.WarehouseParts.Count; ++j)
                {
                    if (source.WarehouseParts[j].WarehouseId == source.Warehouses[i].Id)
                    {
                        string partName = string.Empty;
                        for (int k = 0; k < source.Parts.Count; ++k)
                        {
                            if (source.ItemParts[j].PartId == source.Parts[k].Id)
                            {
                                partName = source.Parts[k].PartName;
                                break;
                            }
                        }
                        WarehouseParts.Add(new WarehousePartViewModel
                        {
                            Id = source.WarehouseParts[j].Id,
                            WarehouseId = source.WarehouseParts[j].WarehouseId,
                            PartId = source.WarehouseParts[j].PartId,
                            PartName = partName,
                            Count = source.WarehouseParts[j].Count
                        });
                    }
                }
                if (source.Warehouses[i].Id == id)
                {
                    return new WarehouseViewModel
                    {
                        Id = source.Warehouses[i].Id,
                        WarehouseName = source.Warehouses[i].WarehouseName,
                        WarehouseParts = WarehouseParts
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WarehouseBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id > maxId)
                {
                    maxId = source.Warehouses[i].Id;
                }
                if (source.Warehouses[i].WarehouseName == model.WarehouseName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Warehouses.Add(new Warehouse
            {
                Id = maxId + 1,
                WarehouseName = model.WarehouseName
            });
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Warehouses[i].WarehouseName == model.WarehouseName &&
                    source.Warehouses[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Warehouses[index].WarehouseName = model.WarehouseName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.WarehouseParts.Count; ++i)
            {
                if (source.WarehouseParts[i].WarehouseId == id)
                {
                    source.WarehouseParts.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == id)
                {
                    source.Warehouses.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
