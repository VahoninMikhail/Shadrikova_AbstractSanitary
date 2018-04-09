using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<ItemViewModel> result = new List<ItemViewModel>();
            for (int i = 0; i < source.Items.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<ItemPartViewModel> itemParts = new List<ItemPartViewModel>();
                for (int j = 0; j < source.ItemParts.Count; ++j)
                {
                    if (source.ItemParts[j].ItemId == source.Items[i].Id)
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
                        itemParts.Add(new ItemPartViewModel
                        {
                            Id = source.ItemParts[j].Id,
                            ItemId = source.ItemParts[j].ItemId,
                            PartId = source.ItemParts[j].PartId,
                            PartName = partName,
                            Count = source.ItemParts[j].Count
                        });
                    }
                }
                result.Add(new ItemViewModel
                {
                    Id = source.Items[i].Id,
                    ItemName = source.Items[i].ItemName,
                    Price = source.Items[i].Price,
                    ItemParts = itemParts
                });
            }
            return result;
        }

        public ItemViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Items.Count; ++i)
            {
                // требуется дополнительно получить список компонентов для изделия и их количество
                List<ItemPartViewModel> itemParts = new List<ItemPartViewModel>();
                for (int j = 0; j < source.ItemParts.Count; ++j)
                {
                    if (source.ItemParts[j].ItemId == source.Items[i].Id)
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
                        itemParts.Add(new ItemPartViewModel
                        {
                            Id = source.ItemParts[j].Id,
                            ItemId = source.ItemParts[j].ItemId,
                            PartId = source.ItemParts[j].PartId,
                            PartName = partName,
                            Count = source.ItemParts[j].Count
                        });
                    }
                }
                if (source.Items[i].Id == id)
                {
                    return new ItemViewModel
                    {
                        Id = source.Items[i].Id,
                        ItemName = source.Items[i].ItemName,
                        Price = source.Items[i].Price,
                        ItemParts = itemParts
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(ItemBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Items.Count; ++i)
            {
                if (source.Items[i].Id > maxId)
                {
                    maxId = source.Items[i].Id;
                }
                if (source.Items[i].ItemName == model.ItemName)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            source.Items.Add(new Item
            {
                Id = maxId + 1,
                ItemName = model.ItemName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = 0;
            for (int i = 0; i < source.ItemParts.Count; ++i)
            {
                if (source.ItemParts[i].Id > maxPCId)
                {
                    maxPCId = source.ItemParts[i].Id;
                }
            }
            // убираем дубли по компонентам
            for (int i = 0; i < model.ItemParts.Count; ++i)
            {
                for (int j = 1; j < model.ItemParts.Count; ++j)
                {
                    if (model.ItemParts[i].PartId ==
                        model.ItemParts[j].PartId)
                    {
                        model.ItemParts[i].Count +=
                            model.ItemParts[j].Count;
                        model.ItemParts.RemoveAt(j--);
                    }
                }
            }
            // добавляем компоненты
            for (int i = 0; i < model.ItemParts.Count; ++i)
            {
                source.ItemParts.Add(new ItemPart
                {
                    Id = ++maxPCId,
                    ItemId = maxId + 1,
                    PartId = model.ItemParts[i].PartId,
                    Count = model.ItemParts[i].Count
                });
            }
        }

        public void UpdElement(ItemBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Items.Count; ++i)
            {
                if (source.Items[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Items[i].ItemName == model.ItemName &&
                    source.Items[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Items[index].ItemName = model.ItemName;
            source.Items[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.ItemParts.Count; ++i)
            {
                if (source.ItemParts[i].Id > maxPCId)
                {
                    maxPCId = source.ItemParts[i].Id;
                }
            }
            // обновляем существуюущие компоненты
            for (int i = 0; i < source.ItemParts.Count; ++i)
            {
                if (source.ItemParts[i].ItemId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.ItemParts.Count; ++j)
                    {
                        // если встретили, то изменяем количество
                        if (source.ItemParts[i].Id == model.ItemParts[j].Id)
                        {
                            source.ItemParts[i].Count = model.ItemParts[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    // если не встретили, то удаляем
                    if (flag)
                    {
                        source.ItemParts.RemoveAt(i--);
                    }
                }
            }
            // новые записи
            for (int i = 0; i < model.ItemParts.Count; ++i)
            {
                if (model.ItemParts[i].Id == 0)
                {
                    // ищем дубли
                    for (int j = 0; j < source.ItemParts.Count; ++j)
                    {
                        if (source.ItemParts[j].ItemId == model.Id &&
                            source.ItemParts[j].PartId == model.ItemParts[i].PartId)
                        {
                            source.ItemParts[j].Count += model.ItemParts[i].Count;
                            model.ItemParts[i].Id = source.ItemParts[j].Id;
                            break;
                        }
                    }
                    // если не нашли дубли, то новая запись
                    if (model.ItemParts[i].Id == 0)
                    {
                        source.ItemParts.Add(new ItemPart
                        {
                            Id = ++maxPCId,
                            ItemId = model.Id,
                            PartId = model.ItemParts[i].PartId,
                            Count = model.ItemParts[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            // удаяем записи по компонентам при удалении изделия
            for (int i = 0; i < source.ItemParts.Count; ++i)
            {
                if (source.ItemParts[i].ItemId == id)
                {
                    source.ItemParts.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Items.Count; ++i)
            {
                if (source.Items[i].Id == id)
                {
                    source.Items.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
