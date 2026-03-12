using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sacpapi.Data;
using sacpapi.Models; // Import your ApplicationDbContext namespace

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class OutputController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OutputController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var ouputs = _context.Outputs.ToList();
            return Json(ouputs);
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            var ouputs = _context.Outputs.Where(o => o.OutcomeId == id).ToList();
            return Json(ouputs);
        }
        [HttpPost]
        public IActionResult CreateOutput([FromBody] JObject data)
        {
            if (data != null)
            {
                dynamic jsonData = data;
                try
                {
                    Output output = new Output()
                    {
                        Name = jsonData["name"],
                        OutcomeId = jsonData["outcomeId"]
                    };

                    _context.Outputs.Add(output);
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
        public IActionResult DeleteOutput(int id)
        {
            try
            {
                var output = _context.Outputs.FirstOrDefault(o => o.Id == id);

                if (output == null)
                {
                    return NotFound(); // If output with given id doesn't exist
                }

                _context.Outputs.Remove(output);
                _context.SaveChanges();

                return NoContent(); // Indicates successful deletion
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
