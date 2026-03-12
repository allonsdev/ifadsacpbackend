using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sacpapi.Data; // Assuming your DbContext is defined in this namespace
using sacpapi.Models; // Assuming your models are in this namespace

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class ObjectiveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ObjectiveController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var objectives = _context.Objectives.ToList();
            return Json(objectives);
        }

        [HttpPost]
        public IActionResult CreateObjective([FromBody] JObject data)
        {
            if (data != null)
            {
                dynamic jsonData = data;
                try
                {
                    Objective objective = new Objective()
                    {
                        Name = jsonData["name"],
                        ProjectId = 1
                    };

                    _context.Objectives.Add(objective);
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
        public IActionResult DeleteObjective(int id)
        {
            try
            {
                var objective = _context.Objectives.Find(id);
                if (objective == null)
                {
                    return NotFound(); // If the objective doesn't exist
                }

                _context.Objectives.Remove(objective);
                _context.SaveChanges();

                return StatusCode(204); // No content, successful deletion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // In case of an exception during deletion
            }
        }

    }
}
