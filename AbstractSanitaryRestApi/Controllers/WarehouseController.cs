using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractSanitaryRestApi.Controllers
{
    public class WarehouseController : ApiController
    {
        private readonly IWarehouseService _service;

        public WarehouseController(IWarehouseService service)
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
        public void AddElement(WarehouseBindingModel model)
        {
            _service.AddElement(model);
        }

        [HttpPost]
        public void UpdElement(WarehouseBindingModel model)
        {
            _service.UpdElement(model);
        }

        [HttpPost]
        public void DelElement(WarehouseBindingModel model)
        {
            _service.DelElement(model.Id);
        }
    }
}
