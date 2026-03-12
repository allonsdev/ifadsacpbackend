using sacpapi.Data; // Include the namespace where ApplicationDbContext is defined
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sacpapi.Models;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ActivityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ActivityController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var activities = _context.Activities.ToList();
            return Json(activities);
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            var activities = _context.Activities.Where(o => o.OutputId == id).ToList();
            return Json(activities);
        }
        [HttpPost]
        public IActionResult Createactivity([FromBody] JObject data)
        {
            if (data != null)
            {
                dynamic jsonData = data;
                try
                {
                    Activity activity = new Activity()
                    {
                        Name = jsonData["name"],
                        OutputId = jsonData["outputId"],
                        NameInShort = jsonData["nameInShort"]
                    };

                    _context.Activities.Add(activity);
                    _context.SaveChanges();

                    return StatusCode(201);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteActivity(int id)
        {
            try
            {
                var activityToDelete = _context.Activities.FirstOrDefault(a => a.Id == id);

                if (activityToDelete == null)
                {
                    return NotFound(); // Return 404 if activity with the provided ID is not found
                }

                _context.Activities.Remove(activityToDelete);
                _context.SaveChanges();

                return NoContent(); // Return 204 No Content on successful deletion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // Return bad request if an exception occurs
            }
        }

    }
}
