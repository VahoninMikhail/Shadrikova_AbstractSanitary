using AbstractSanitaryModel;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using AbstractSanitaryService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace AbstractSanitaryService.ImplementationsBD
{
    public class BasicServiceBD : IBasicService
    {
        private AbstractDbContext context;

        public BasicServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<OrderingViewModel> GetList()
        {
            List<OrderingViewModel> result = context.Orderings
                .Select(rec => new OrderingViewModel
                {
                    Id = rec.Id,
                    CustomerId = rec.CustomerId,
                    ItemId = rec.ItemId,
                    PlumberId = rec.PlumberId,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                    DateImplement = rec.DateImplement == null ? "" :
                                        SqlFunctions.DateName("dd", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("mm", rec.DateImplement.Value) + " " +
                                        SqlFunctions.DateName("yyyy", rec.DateImplement.Value),
                    Status = rec.Status.ToString(),
                    Count = rec.Count,
                    Sum = rec.Sum,
                    CustomerFIO = rec.Customer.CustomerFIO,
                    ItemName = rec.Item.ItemName,
                    PlumberName = rec.Plumber.PlumberFIO
                })
                .ToList();
            return result;
        }

        public void CreateOrdering(OrderingBindingModel model)
        {
            var ordering = new Ordering
            {
                CustomerId = model.CustomerId,
                ItemId = model.ItemId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = OrderingStatus.Принят
            };
            context.Orderings.Add(ordering);
            context.SaveChanges();

            var customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerId);
            SendEmail(customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} создан успешно", ordering.Id,
                ordering.DateCreate.ToShortDateString()));
        }

        public void TakeOrderingInWork(OrderingBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Ordering element = context.Orderings.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    var itemParts = context.ItemParts
                                                .Include(rec => rec.Part)
                                                .Where(rec => rec.ItemId == element.ItemId);
                    foreach (var itemPart in itemParts)
                    {
                        int countOnWarehouses = itemPart.Count * element.Count;
                        var warehouseParts = context.WarehouseParts
                                                    .Where(rec => rec.PartId == itemPart.PartId);
                        foreach (var warehousePart in warehouseParts)
                        {
                            if (warehousePart.Count >= countOnWarehouses)
                            {
                                warehousePart.Count -= countOnWarehouses;
                                countOnWarehouses = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnWarehouses -= warehousePart.Count;
                                warehousePart.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnWarehouses > 0)
                        {
                            throw new Exception("Не достаточно запчасти " +
                                itemPart.Part.PartName + ", требуется " +
                                itemPart.Count + ", не хватает " + countOnWarehouses);
                        }
                    }
                    element.PlumberId = model.PlumberId;
                    element.DateImplement = DateTime.Now;
                    element.Status = OrderingStatus.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Customer.Mail, "Оповещение по заказам",
                        string.Format("Заказ №{0} от {1} передан в работу", element.Id, element.DateCreate.ToShortDateString()));
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void FinishOrdering(int id)
        {
            Ordering element = context.Orderings.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = OrderingStatus.Готов;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} передан на оплату", element.Id,
                element.DateCreate.ToShortDateString()));
        }

        public void PayOrdering(int id)
        {
            Ordering element = context.Orderings.Include(rec => rec.Customer).FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Status = OrderingStatus.Оплачен;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам",
                string.Format("Заказ №{0} от {1} оплачен успешно", element.Id, element.DateCreate.ToShortDateString()));
        }

        public void PutPartOnWarehouse(WarehousePartBindingModel model)
        {
            WarehousePart element = context.WarehouseParts
                                                .FirstOrDefault(rec => rec.WarehouseId == model.WarehouseId &&
                                                                    rec.PartId == model.PartId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.WarehouseParts.Add(new WarehousePart
                {
                    WarehouseId = model.WarehouseId,
                    PartId = model.PartId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;

            try
            {
                objMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["MailLogin"]);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;

                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
                    ConfigurationManager.AppSettings["MailPassword"]);

                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}
