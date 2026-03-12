using Microsoft.AspNetCore.Mvc;
using sacpapi.Data.Services;

namespace sacpapi.Controllers
{
    public class EOIBeneficiariesController : Controller
    {
        private readonly IEOIBeneficiaries _service;
        public EOIBeneficiariesController(IEOIBeneficiaries service)
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
