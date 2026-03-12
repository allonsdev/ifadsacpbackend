using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class APGroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        public APGroupController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.APGroups.ToList();
            return Json(data);
        }
    }
}
