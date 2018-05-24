using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsList
{
    public class PlumberServiceList : IPlumberService
    {
        private ListDataSingleton source;

        public PlumberServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<PlumberViewModel> GetList()
        {
            List<PlumberViewModel> result = source.Plumbers
                .Select(rec => new PlumberViewModel
                {
                    Id = rec.Id,
                    PlumberFIO = rec.PlumberFIO
                })
                .ToList();
            return result;
        }

        public PlumberViewModel GetElement(int id)
        {
            Plumber element = source.Plumbers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PlumberViewModel
                {
                    Id = element.Id,
                    PlumberFIO = element.PlumberFIO
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PlumberBindingModel model)
        {
            Plumber element = source.Plumbers.FirstOrDefault(rec => rec.PlumberFIO == model.PlumberFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сантехник с таким ФИО");
            }
            int maxId = source.Plumbers.Count > 0 ? source.Plumbers.Max(rec => rec.Id) : 0;
            source.Plumbers.Add(new Plumber
            {
                Id = maxId + 1,
                PlumberFIO = model.PlumberFIO
            });
        }

        public void UpdElement(PlumberBindingModel model)
        {
            Plumber element = source.Plumbers.FirstOrDefault(rec =>
                                        rec.PlumberFIO == model.PlumberFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сантехник с таким ФИО");
            }
            element = source.Plumbers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.PlumberFIO = model.PlumberFIO;
        }

        public void DelElement(int id)
        {
            Plumber element = source.Plumbers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Plumbers.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
