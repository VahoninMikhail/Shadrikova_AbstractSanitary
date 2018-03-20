using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<OrderingViewModel> result = new List<OrderingViewModel>();
            for (int i = 0; i < source.Orderings.Count; ++i)
            {
                string customerFIO = string.Empty;
                for (int j = 0; j < source.Customers.Count; ++j)
                {
                    if (source.Customers[j].Id == source.Orderings[i].CustomerId)
                    {
                        customerFIO = source.Customers[j].CustomerFIO;
                        break;
                    }
                }
                string itemName = string.Empty;
                for (int j = 0; j < source.Items.Count; ++j)
                {
                    if (source.Items[j].Id == source.Orderings[i].ItemId)
                    {
                        itemName = source.Items[j].ItemName;
                        break;
                    }
                }
                string plumberFIO = string.Empty;
                if (source.Orderings[i].PlumberId.HasValue)
                {
                    for (int j = 0; j < source.Plumbers.Count; ++j)
                    {
                        if (source.Plumbers[j].Id == source.Orderings[i].PlumberId.Value)
                        {
                            plumberFIO = source.Plumbers[j].PlumberFIO;
                            break;
                        }
                    }
                }
                result.Add(new OrderingViewModel
                {
                    Id = source.Orderings[i].Id,
                    CustomerId = source.Orderings[i].CustomerId,
                    CustomerFIO = customerFIO,
                    ItemId = source.Orderings[i].ItemId,
                    ItemName = itemName,
                    PlumberId = source.Orderings[i].PlumberId,
                    PlumberName = plumberFIO,
                    Count = source.Orderings[i].Count,
                    Sum = source.Orderings[i].Sum,
                    DateCreate = source.Orderings[i].DateCreate.ToLongDateString(),
                    DateImplement = source.Orderings[i].DateImplement?.ToLongDateString(),
                    Status = source.Orderings[i].Status.ToString()
                });
            }
            return result;
        }

        public void CreateOrdering(OrderingBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Orderings.Count; ++i)
            {
                if (source.Orderings[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
            }
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
            int index = -1;
            for (int i = 0; i < source.Orderings.Count; ++i)
            {
                if (source.Orderings[i].Id == model.Id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            for (int i = 0; i < source.ItemParts.Count; ++i)
            {
                if (source.ItemParts[i].ItemId == source.Orderings[index].ItemId)
                {
                    int countOnWarehouses = 0;
                    for (int j = 0; j < source.WarehouseParts.Count; ++j)
                    {
                        if (source.WarehouseParts[j].PartId == source.ItemParts[i].PartId)
                        {
                            countOnWarehouses += source.WarehouseParts[j].Count;
                        }
                    }
                    if (countOnWarehouses < source.ItemParts[i].Count * source.Orderings[index].Count)
                    {
                        for (int j = 0; j < source.Parts.Count; ++j)
                        {
                            if (source.Parts[j].Id == source.ItemParts[i].PartId)
                            {
                                throw new Exception("Не достаточно запчастей " + source.Parts[j].PartName +
                                    " требуется " + source.ItemParts[i].Count + ", в наличии " + countOnWarehouses);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < source.ItemParts.Count; ++i)
            {
                if (source.ItemParts[i].ItemId == source.Orderings[index].ItemId)
                {
                    int countOnWarehouses = source.ItemParts[i].Count * source.Orderings[index].Count;
                    for (int j = 0; j < source.WarehouseParts.Count; ++j)
                    {
                        if (source.WarehouseParts[j].PartId == source.ItemParts[i].PartId)
                        {
                            if (source.WarehouseParts[j].Count >= countOnWarehouses)
                            {
                                source.WarehouseParts[j].Count -= countOnWarehouses;
                                break;
                            }
                            else
                            {
                                countOnWarehouses -= source.WarehouseParts[j].Count;
                                source.WarehouseParts[j].Count = 0;
                            }
                        }
                    }
                }
            }
            source.Orderings[index].PlumberId = model.PlumberId;
            source.Orderings[index].DateImplement = DateTime.Now;
            source.Orderings[index].Status = OrderingStatus.Выполняется;
        }

        public void FinishOrdering(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Orderings.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Orderings[index].Status = OrderingStatus.Готов;
        }

        public void PayOrdering(int id)
        {
            int index = -1;
            for (int i = 0; i < source.Orderings.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    index = i;
                    break;
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Orderings[index].Status = OrderingStatus.Оплачен;
        }

        public void PutPartOnWarehouse(WarehousePartBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.WarehouseParts.Count; ++i)
            {
                if (source.WarehouseParts[i].WarehouseId == model.WarehouseId &&
                    source.WarehouseParts[i].PartId == model.PartId)
                {
                    source.WarehouseParts[i].Count += model.Count;
                    return;
                }
                if (source.WarehouseParts[i].Id > maxId)
                {
                    maxId = source.WarehouseParts[i].Id;
                }
            }
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
