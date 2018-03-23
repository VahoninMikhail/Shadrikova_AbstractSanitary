using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsList
{
    public class ItemServiceList : IItemService
    {
        private ListDataSingleton source;

        public ItemServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<ItemViewModel> GetList()
        {
            List<ItemViewModel> result = source.Items
                .Select(rec => new ItemViewModel
                {
                    Id = rec.Id,
                    ItemName = rec.ItemName,
                    Price = rec.Price,
                    ItemParts = source.ItemParts
                            .Where(recIP => recIP.ItemId == rec.Id)
                            .Select(recIP => new ItemPartViewModel
                            {
                                Id = recIP.Id,
                                ItemId = recIP.ItemId,
                                PartId = recIP.PartId,
                                PartName = source.Parts
                                    .FirstOrDefault(recP => recP.Id == recIP.PartId)?.PartName,
                                Count = recIP.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ItemViewModel GetElement(int id)
        {
            Item element = source.Items.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ItemViewModel
                {
                    Id = element.Id,
                    ItemName = element.ItemName,
                    Price = element.Price,
                    ItemParts = source.ItemParts
                            .Where(recIP => recIP.ItemId == element.Id)
                            .Select(recIP => new ItemPartViewModel
                            {
                                Id = recIP.Id,
                                ItemId = recIP.ItemId,
                                PartId = recIP.PartId,
                                PartName = source.Parts
                                        .FirstOrDefault(recP => recP.Id == recIP.PartId)?.PartName,
                                Count = recIP.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ItemBindingModel model)
        {
            Item element = source.Items.FirstOrDefault(rec => rec.ItemName == model.ItemName);
            if (element != null)
            {
                throw new Exception("Уже есть услуга с таким названием");
            }
            int maxId = source.Items.Count > 0 ? source.Items.Max(rec => rec.Id) : 0;
            source.Items.Add(new Item
            {
                Id = maxId + 1,
                ItemName = model.ItemName,
                Price = model.Price
            });
            int maxIPId = source.ItemParts.Count > 0 ?
                                    source.ItemParts.Max(rec => rec.Id) : 0;
            var groupParts = model.ItemParts
                                        .GroupBy(rec => rec.PartId)
                                        .Select(rec => new
                                        {
                                            PartId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupPart in groupParts)
            {
                source.ItemParts.Add(new ItemPart
                {
                    Id = ++maxIPId,
                    ItemId = maxId + 1,
                    PartId = groupPart.PartId,
                    Count = groupPart.Count
                });
            }
        }

        public void UpdElement(ItemBindingModel model)
        {
            Item element = source.Items.FirstOrDefault(rec =>
                                        rec.ItemName == model.ItemName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть услуга с таким названием");
            }
            element = source.Items.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.ItemName = model.ItemName;
            element.Price = model.Price;

            int maxIPId = source.ItemParts.Count > 0 ? source.ItemParts.Max(rec => rec.Id) : 0;
            var compIds = model.ItemParts.Select(rec => rec.PartId).Distinct();
            var updateParts = source.ItemParts
                                            .Where(rec => rec.ItemId == model.Id &&
                                           compIds.Contains(rec.PartId));
            foreach (var updatePart in updateParts)
            {
                updatePart.Count = model.ItemParts
                                                .FirstOrDefault(rec => rec.Id == updatePart.Id).Count;
            }
            source.ItemParts.RemoveAll(rec => rec.ItemId == model.Id &&
                                       !compIds.Contains(rec.PartId));
            var groupParts = model.ItemParts
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.PartId)
                                        .Select(rec => new
                                        {
                                            PartId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupPart in groupParts)
            {
                ItemPart elementPC = source.ItemParts
                                        .FirstOrDefault(rec => rec.ItemId == model.Id &&
                                                        rec.PartId == groupPart.PartId);
                if (elementPC != null)
                {
                    elementPC.Count += groupPart.Count;
                }
                else
                {
                    source.ItemParts.Add(new ItemPart
                    {
                        Id = ++maxIPId,
                        ItemId = model.Id,
                        PartId = groupPart.PartId,
                        Count = groupPart.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Item element = source.Items.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.ItemParts.RemoveAll(rec => rec.ItemId == id);
                source.Items.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
