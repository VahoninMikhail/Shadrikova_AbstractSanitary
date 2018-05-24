using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<PartViewModel> result = source.Parts
                .Select(rec => new PartViewModel
                {
                    Id = rec.Id,
                    PartName = rec.PartName
                })
                .ToList();
            return result;
        }

        public PartViewModel GetElement(int id)
        {
            Part element = source.Parts.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PartViewModel
                {
                    Id = element.Id,
                    PartName = element.PartName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PartBindingModel model)
        {
            Part element = source.Parts.FirstOrDefault(rec => rec.PartName == model.PartName);
            if (element != null)
            {
                throw new Exception("Уже есть запчасть с таким названием");
            }
            int maxId = source.Parts.Count > 0 ? source.Parts.Max(rec => rec.Id) : 0;
            source.Parts.Add(new Part
            {
                Id = maxId + 1,
                PartName = model.PartName
            });
        }

        public void UpdElement(PartBindingModel model)
        {
            Part element = source.Parts.FirstOrDefault(rec =>
                                        rec.PartName == model.PartName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть деталь с таким названием");
            }
            element = source.Parts.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.PartName = model.PartName;
        }

        public void DelElement(int id)
        {
            Part element = source.Parts.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Parts.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
