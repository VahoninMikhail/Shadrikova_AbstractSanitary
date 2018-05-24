using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsBD
{
    public class WarehouseServiceBD : IWarehouseService
    {
        private AbstractDbContext context;

        public WarehouseServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<WarehouseViewModel> GetList()
        {
            List<WarehouseViewModel> result = context.Warehouses
                .Select(rec => new WarehouseViewModel
                {
                    Id = rec.Id,
                    WarehouseName = rec.WarehouseName,
                    WarehouseParts = context.WarehouseParts
                            .Where(recWP => recWP.WarehouseId == rec.Id)
                            .Select(recWP => new WarehousePartViewModel
                            {
                                Id = recWP.Id,
                                WarehouseId = recWP.WarehouseId,
                                PartId = recWP.PartId,
                                PartName = recWP.Part.PartName,
                                Count = recWP.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public WarehouseViewModel GetElement(int id)
        {
            Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new WarehouseViewModel
                {
                    Id = element.Id,
                    WarehouseName = element.WarehouseName,
                    WarehouseParts = context.WarehouseParts
                            .Where(recWP => recWP.WarehouseId == element.Id)
                            .Select(recWP => new WarehousePartViewModel
                            {
                                Id = recWP.Id,
                                WarehouseId = recWP.WarehouseId,
                                PartId = recWP.PartId,
                                PartName = recWP.Part.PartName,
                                Count = recWP.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(WarehouseBindingModel model)
        {
            Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            context.Warehouses.Add(new Warehouse
            {
                WarehouseName = model.WarehouseName
            });
            context.SaveChanges();
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            Warehouse element = context.Warehouses.FirstOrDefault(rec =>
                                        rec.WarehouseName == model.WarehouseName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.WarehouseName = model.WarehouseName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        context.WarehouseParts.RemoveRange(
                                            context.WarehouseParts.Where(rec => rec.WarehouseId == id));
                        context.Warehouses.Remove(element);
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
