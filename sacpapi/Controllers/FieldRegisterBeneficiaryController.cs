using Microsoft.AspNetCore.Mvc;
using sacpapi.Data.Services;
using sacpapi.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace sacpapi.Controllers
{
    public class FieldRegisterBeneficiaryController : Controller
    {
        private readonly IFieldRegisterBeneficiaries _service;
        public FieldRegisterBeneficiaryController(IFieldRegisterBeneficiaries service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            var data = _service.GetAll();
            if (!(data.Any())) return Json(data);
            return Json(data);
        }
    }
}
