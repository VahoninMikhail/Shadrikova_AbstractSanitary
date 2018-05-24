using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsBD
{
    public class ItemServiceBD : IItemService
    {
        private AbstractDbContext context;

        public ItemServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<ItemViewModel> GetList()
        {
            List<ItemViewModel> result = context.Items
                .Select(rec => new ItemViewModel
                {
                    Id = rec.Id,
                    ItemName = rec.ItemName,
                    Price = rec.Price,
                    ItemParts = context.ItemParts
                            .Where(recIP => recIP.ItemId == rec.Id)
                            .Select(recIP => new ItemPartViewModel
                            {
                                Id = recIP.Id,
                                ItemId = recIP.ItemId,
                                PartId = recIP.PartId,
                                PartName = recIP.Part.PartName,
                                Count = recIP.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public ItemViewModel GetElement(int id)
        {
            Item element = context.Items.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new ItemViewModel
                {
                    Id = element.Id,
                    ItemName = element.ItemName,
                    Price = element.Price,
                    ItemParts = context.ItemParts
                            .Where(recIP => recIP.ItemId == element.Id)
                            .Select(recIP => new ItemPartViewModel
                            {
                                Id = recIP.Id,
                                ItemId = recIP.ItemId,
                                PartId = recIP.PartId,
                                PartName = recIP.Part.PartName,
                                Count = recIP.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(ItemBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Item element = context.Items.FirstOrDefault(rec => rec.ItemName == model.ItemName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть услуга с таким названием");
                    }
                    element = new Item
                    {
                        ItemName = model.ItemName,
                        Price = model.Price
                    };
                    context.Items.Add(element);
                    context.SaveChanges();
                    var groupParts = model.ItemParts
                                                .GroupBy(rec => rec.PartId)
                                                .Select(rec => new
                                                {
                                                    PartId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    foreach (var groupPart in groupParts)
                    {
                        context.ItemParts.Add(new ItemPart
                        {
                            ItemId = element.Id,
                            PartId = groupPart.PartId,
                            Count = groupPart.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(ItemBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Item element = context.Items.FirstOrDefault(rec =>
                                        rec.ItemName == model.ItemName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть услуга с таким названием");
                    }
                    element = context.Items.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.ItemName = model.ItemName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    var parIds = model.ItemParts.Select(rec => rec.PartId).Distinct();
                    var updateParts = context.ItemParts
                                                    .Where(rec => rec.ItemId == model.Id &&
                                                        parIds.Contains(rec.PartId));
                    foreach (var updatePart in updateParts)
                    {
                        updatePart.Count = model.ItemParts
                                                        .FirstOrDefault(rec => rec.Id == updatePart.Id).Count;
                    }
                    context.SaveChanges();
                    context.ItemParts.RemoveRange(
                                        context.ItemParts.Where(rec => rec.ItemId == model.Id &&
                                                                            !parIds.Contains(rec.PartId)));
                    context.SaveChanges();
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
                        ItemPart elementIP = context.ItemParts
                                                .FirstOrDefault(rec => rec.ItemId == model.Id &&
                                                                rec.PartId == groupPart.PartId);
                        if (elementIP != null)
                        {
                            elementIP.Count += groupPart.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.ItemParts.Add(new ItemPart
                            {
                                ItemId = model.Id,
                                PartId = groupPart.PartId,
                                Count = groupPart.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Item element = context.Items.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.ItemParts.RemoveRange(
                                            context.ItemParts.Where(rec => rec.ItemId == id));
                        context.Items.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
