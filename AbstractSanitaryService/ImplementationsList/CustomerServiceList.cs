using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractSanitaryService.ImplementationsList
{
    public class CustomerServiceList : ICustomerService
    {
        private ListDataSingleton source;

        public CustomerServiceList()
        {
            source = ListDataSingleton.GetInstance();
        }

        public List<CustomerViewModel> GetList()
        {
            List<CustomerViewModel> result = new List<CustomerViewModel>();
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                result.Add(new CustomerViewModel
                {
                    Id = source.Customers[i].Id,
                    CustomerFIO = source.Customers[i].CustomerFIO
                });
            }
            return result;
        }

        public CustomerViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    return new CustomerViewModel
                    {
                        Id = source.Customers[i].Id,
                        CustomerFIO = source.Customers[i].CustomerFIO
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(CustomerBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].Id > maxId)
                {
                    maxId = source.Customers[i].Id;
                }
                if (source.Customers[i].CustomerFIO == model.CustomerFIO)
                {
                    throw new Exception("Уже есть покупатель с таким ФИО");
                }
            }
            source.Customers.Add(new Customer
            {
                Id = maxId + 1,
                CustomerFIO = model.CustomerFIO
            });
        }

        public void UpdElement(CustomerBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Customers[i].CustomerFIO == model.CustomerFIO &&
                    source.Customers[i].Id != model.Id)
                {
                    throw new Exception("Уже есть покупатель с таким ФИО");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Customers[index].CustomerFIO = model.CustomerFIO;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Customers.Count; ++i)
            {
                if (source.Customers[i].Id == id)
                {
                    source.Customers.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
