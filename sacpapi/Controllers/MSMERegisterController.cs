using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sacpapi.Data.Services;
using sacpapi.Models;
using System.Net.Http.Headers;

namespace sacpapi.Controllers
{
    public class MSMERegisterController : Controller
    {
        private readonly IMSMERegisterEnterprises _service;
        private readonly IHttpClientFactory _httpClientFactory;

        public MSMERegisterController(IMSMERegisterEnterprises service, IHttpClientFactory httpClientFactory)
        {
            _service = service;
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            var data = _service.GetAll();
            if (!(data.Any())) return Json(data);
            return Json(data);
        }

    }
}


