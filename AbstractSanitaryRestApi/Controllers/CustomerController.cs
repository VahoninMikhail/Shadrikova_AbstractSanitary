using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractSanitaryRestApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var element = _service.GetElement(id);
            if (element == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(element);
        }

        [HttpPost]
        public void AddElement(CustomerBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(CustomerBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(CustomerBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
