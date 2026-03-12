using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FocusGroupDiscussionController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FocusGroupDiscussionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.FocusGroupDiscussions.ToList();
            return Json(data);
        }
    }
}
