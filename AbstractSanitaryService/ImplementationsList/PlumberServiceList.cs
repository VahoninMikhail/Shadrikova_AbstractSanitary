using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<PlumberViewModel> result = new List<PlumberViewModel>();
            for (int i = 0; i < source.Plumbers.Count; ++i)
            {
                result.Add(new PlumberViewModel
                {
                    Id = source.Plumbers[i].Id,
                    PlumberFIO = source.Plumbers[i].PlumberFIO
                });
            }
            return result;
        }

        public PlumberViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Plumbers.Count; ++i)
            {
                if (source.Plumbers[i].Id == id)
                {
                    return new PlumberViewModel
                    {
                        Id = source.Plumbers[i].Id,
                        PlumberFIO = source.Plumbers[i].PlumberFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(PlumberBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Plumbers.Count; ++i)
            {
                if (source.Plumbers[i].Id > maxId)
                {
                    maxId = source.Plumbers[i].Id;
                }
                if (source.Plumbers[i].PlumberFIO == model.PlumberFIO)
                {
                    throw new Exception("Уже есть сантехник с таким ФИО");
                }
            }
            source.Plumbers.Add(new Plumber
            {
                Id = maxId + 1,
                PlumberFIO = model.PlumberFIO
            });
        }

        public void UpdElement(PlumberBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Plumbers.Count; ++i)
            {
                if (source.Plumbers[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Plumbers[i].PlumberFIO == model.PlumberFIO &&
                    source.Plumbers[i].Id != model.Id)
                {
                    throw new Exception("Уже есть сантехник с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Plumbers[index].PlumberFIO = model.PlumberFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Plumbers.Count; ++i)
            {
                if (source.Plumbers[i].Id == id)
                {
                    source.Plumbers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
