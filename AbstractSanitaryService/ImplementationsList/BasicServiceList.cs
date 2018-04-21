using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsList
{
    public class BasicServiceList : IBasicService
    {
        private ListDataSingleton source;

        public BasicServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<OrderingViewModel> GetList()
        {
            List<OrderingViewModel> result = source.Orderings
                .Select(rec => new OrderingViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    ItemId = rec.ItemId,
                    PlumberId = rec.PlumberId,
                    Count = rec.Count,
                    Sum = rec.Sum,
                    DateCreate = rec.DateCreate.ToLongDateString(),
                    DateImplement = rec.DateImplement?.ToLongDateString(),
                    Status = rec.Status.ToString(),
                    CustomerFIO = source.Customers
                                    .FirstOrDefault(recC => recC.Id == rec.CustomerId)?.CustomerFIO,
                    ItemName = source.Items
                                    .FirstOrDefault(recI => recI.Id == rec.ItemId)?.ItemName,
                    PlumberName = source.Plumbers
                                    .FirstOrDefault(recPl => recPl.Id == rec.PlumberId)?.PlumberFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrdering(OrderingBindingModel model)
        {
            int maxId = source.Orderings.Count > 0 ? source.Orderings.Max(rec => rec.Id) : 0;
            source.Orderings.Add(new Ordering
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                ItemId = model.ItemId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = OrderingStatus.Принят
            });
        }

        public void TakeOrderingInWork(OrderingBindingModel model)
        {
            Ordering element = source.Orderings.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            var itemParts = source.ItemParts.Where(rec => rec.ItemId == element.ItemId);
            foreach (var itemPart in itemParts)
            {
                int countOnWarehouses = source.WarehouseParts
                                            .Where(rec => rec.PartId == itemPart.PartId)
                                            .Sum(rec => rec.Count);
                if (countOnWarehouses < itemPart.Count * element.Count)
                {
                    var componentName = source.Parts
                                    .FirstOrDefault(rec => rec.Id == itemPart.PartId);
                    throw new Exception("Не достаточно запчасти " + componentName?.PartName +
                        ", требуется " + itemPart.Count + ", в наличии " + countOnWarehouses);
                }
            }
            foreach (var itemPart in itemParts)
            {
                int countOnWarehouses = itemPart.Count * element.Count;
                var warehouseParts = source.WarehouseParts
                                            .Where(rec => rec.PartId == itemPart.PartId);
                foreach (var warehousePart in warehouseParts)
                {
                    if (warehousePart.Count >= countOnWarehouses)
                    {
                        warehousePart.Count -= countOnWarehouses;
                        break;
                    }
                    else
                    {
                        countOnWarehouses -= warehousePart.Count;
                        warehousePart.Count = 0;
                    }
                }
            }
            element.PlumberId = model.PlumberId;
            element.DateImplement = DateTime.Now;
            element.Status = OrderingStatus.Выполняется;
        }

        public void FinishOrdering(int id)
        {
            Ordering element = source.Orderings.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = OrderingStatus.Готов;
        }

        public void PayOrdering(int id)
        {
            Ordering element = source.Orderings.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = OrderingStatus.Оплачен;
        }

        public void PutPartOnWarehouse(WarehousePartBindingModel model)
        {
            WarehousePart element = source.WarehouseParts
                                                .FirstOrDefault(rec => rec.WarehouseId == model.WarehouseId &&
                                                                    rec.PartId == model.PartId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.WarehouseParts.Count > 0 ? source.WarehouseParts.Max(rec => rec.Id) : 0;
                source.WarehouseParts.Add(new WarehousePart
                {
                    Id = ++maxId,
                    WarehouseId = model.WarehouseId,
                    PartId = model.PartId,
                    Count = model.Count
                });
            }
        }
    }
}
