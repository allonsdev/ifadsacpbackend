using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sacpapi.Data;
using sacpapi.Models; // Ensure to import the appropriate namespace for your ApplicationDbContext

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class OutcomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OutcomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var outcomes = _context.Outcomes.ToList();
            return Json(outcomes);
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            var outcomes = _context.Outcomes.Where(o=> o.ObjectiveId == id ).ToList();
            return Json(outcomes);
        }

        [HttpPost]
        public IActionResult CreateOutcome([FromBody] JObject data)
        {
            if (data != null)
            {
                dynamic jsonData = data;
                try
                {
                    Outcome outcome = new Outcome()
                    {
                        Name = jsonData["name"],
                        ObjectiveId = jsonData["objectiveId"]
                    };

                    _context.Outcomes.Add(outcome);
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
        public IActionResult DeleteOutcome(int id)
        {
            try
            {
                var outcomeToDelete = _context.Outcomes.Find(id);

                if (outcomeToDelete == null)
                {
                    return NotFound();
                }

                _context.Outcomes.Remove(outcomeToDelete);
                _context.SaveChanges();

                return NoContent(); // 204 No Content status code upon successful deletion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

