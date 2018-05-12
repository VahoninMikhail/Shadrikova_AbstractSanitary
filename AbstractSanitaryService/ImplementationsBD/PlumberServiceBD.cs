using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractSanitaryService.ImplementationsBD
{
    public class PlumberServiceBD : IPlumberService
    {
        private AbstractDbContext context;

        public PlumberServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<PlumberViewModel> GetList()
        {
            List<PlumberViewModel> result = context.Plumbers
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
            Plumber element = context.Plumbers.FirstOrDefault(rec => rec.Id == id);
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
            Plumber element = context.Plumbers.FirstOrDefault(rec => rec.PlumberFIO == model.PlumberFIO);
            if (element != null)
            {
                throw new Exception("Уже есть сантехник с таким ФИО");
            }
            context.Plumbers.Add(new Plumber
            {
                PlumberFIO = model.PlumberFIO
            });
            context.SaveChanges();
        }

        public void UpdElement(PlumberBindingModel model)
        {
            Plumber element = context.Plumbers.FirstOrDefault(rec =>
                                        rec.PlumberFIO == model.PlumberFIO && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть сантехник с таким ФИО");
            }
            element = context.Plumbers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.PlumberFIO = model.PlumberFIO;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Plumber element = context.Plumbers.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Plumbers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
