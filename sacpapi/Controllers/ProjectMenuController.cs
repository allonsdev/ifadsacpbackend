using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ProjectMenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectMenuController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var menu = _context.ProjectMenu
                .Where(item => item.ShowMenu == true)
                .OrderBy(item => item.Index)
                .ToList();
            return Json(menu);
        }
    }
}
