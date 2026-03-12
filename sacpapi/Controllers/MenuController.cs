using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var menu = _context.Menu
                .Where(item => item.ShowMenu == true)
                .OrderBy(item => item.Index)
                .ToList();
            return Json(menu);
        }
    }
}
