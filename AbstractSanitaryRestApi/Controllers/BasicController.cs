using AbstractSanitaryRestApi.Services;
using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractSanitaryRestApi.Controllers
{
    public class BasicController : ApiController
    {
        private readonly IBasicService _service;

        public BasicController(IBasicService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetList()
        {
            var list = _service.GetList();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void CreateOrdering(OrderingBindingModel model)
        {
            _service.CreateOrdering(model);
        }

        [HttpPost]
        public void TakeOrderingInWork(OrderingBindingModel model)
        {
            _service.TakeOrderingInWork(model);
        }

        [HttpPost]
        public void FinishOrdering(OrderingBindingModel model)
        {
            _service.FinishOrdering(model.Id);
        }

        [HttpPost]
        public void PayOrdering(OrderingBindingModel model)
        {
            _service.PayOrdering(model.Id);
        }

        [HttpPost]
        public void PutPartOnWarehouse(WarehousePartBindingModel model)
        {
            _service.PutPartOnWarehouse(model);
        }

        [HttpGet]
        public IHttpActionResult GetInfo()
        {
            ReflectionService service = new ReflectionService();
            var list = service.GetInfoByAssembly();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }
    }
}
