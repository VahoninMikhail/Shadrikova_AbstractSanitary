using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractSanitaryService.ImplementationsList
{
    public class PartServiceList : IPartService
    {
        private ListDataSingleton source;

        public PartServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<PartViewModel> GetList()
        {
            List<PartViewModel> result = new List<PartViewModel>();
            for (int i = 0; i < source.Parts.Count; ++i)
            {
                result.Add(new PartViewModel
                {
                    Id = source.Parts[i].Id,
                    PartName = source.Parts[i].PartName
                });
            }
            return result;
        }

        public PartViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Parts.Count; ++i)
            {
                if (source.Parts[i].Id == id)
                {
                    return new PartViewModel
                    {
                        Id = source.Parts[i].Id,
                        PartName = source.Parts[i].PartName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PartBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Parts.Count; ++i)
            {
                if (source.Parts[i].Id > maxId)
                {
                    maxId = source.Parts[i].Id;
                }
                if (source.Parts[i].PartName == model.PartName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Parts.Add(new Part
            {
                Id = maxId + 1,
                PartName = model.PartName
            });
        }

        public void UpdElement(PartBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Parts.Count; ++i)
            {
                if (source.Parts[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Parts[i].PartName == model.PartName &&
                    source.Parts[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Parts[index].PartName = model.PartName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Parts.Count; ++i)
            {
                if (source.Parts[i].Id == id)
                {
                    source.Parts.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
