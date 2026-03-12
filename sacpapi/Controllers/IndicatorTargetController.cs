using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Models;
using System.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class IndicatorTargetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public IndicatorTargetController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        public IActionResult Index()
        {
            var indicatorTargets = _context.IndicatorTargets.ToList();
            return Json(indicatorTargets);
        }
        [HttpGet("byid/{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.IndicatorTargets.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }
        [HttpGet("{indicatorId}")]
        public IActionResult GetByIndicatorId(int indicatorId)
        {
            var indicatorTargets = _context.IndicatorTargets
                                        .Where(it => it.IndicatorId == indicatorId)
                                        .ToList();

            return Json(indicatorTargets);
        }
        [HttpGet("detailed/{indicatorId}")]
        public IActionResult detailed(int indicatorId)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM [dbo].[IndicatorTragetsDetailed] where IndicatorId =" + indicatorId;

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            string jsonResult = JsonConvert.SerializeObject(dataTable);
                            connection.Close();
                            return Json(jsonResult);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving files: {ex.Message}");
            }
        }
        [HttpPost]
        public IActionResult CreateIndicatorTarget([FromBody] IndicatorTarget indicatorTarget)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.IndicatorTargets.Add(indicatorTarget);
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
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateIndicatorTarget(int id, [FromBody] IndicatorTarget indicatorTarget)
        {
            if (id != indicatorTarget.Id)
            {
                return BadRequest();
            }

            _context.Entry(indicatorTarget).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.IndicatorTargets.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteIndicatorTarget(int id)
        {
            var indicatorTarget = _context.IndicatorTargets.Find(id);
            if (indicatorTarget == null)
            {
                return NotFound();
            }
            try
            {
                _context.IndicatorTargets.Remove(indicatorTarget);
                _context.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
