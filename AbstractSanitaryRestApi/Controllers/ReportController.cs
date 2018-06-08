using AbstractSanitaryService.BindingModels;
using AbstractSanitaryService.Interfaces;
using System;
using System.Web.Http;

namespace AbstractSanitaryRestApi.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IHttpActionResult GetWarehousesLoad()
        {
            var list = _service.GetWarehousesLoad();
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult GetCustomerOrderings(ReportBindingModel model)
        {
            var list = _service.GetCustomerOrderings(model);
            if (list == null)
            {
                InternalServerError(new Exception("Нет данных"));
            }
            return Ok(list);
        }

        [HttpPost]
        public void SaveItemPrice(ReportBindingModel model)
        {
            _service.SaveItemPrice(model);
        }

        [HttpPost]
        public void SaveWarehousesLoad(ReportBindingModel model)
        {
            _service.SaveWarehousesLoad(model);
        }

        [HttpPost]
        public void SaveCustomerOrderings(ReportBindingModel model)
        {
            _service.SaveCustomerOrderings(model);
        }
    }
}
