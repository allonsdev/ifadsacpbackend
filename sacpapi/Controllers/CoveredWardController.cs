using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Models;
using System.Data;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CoveredWardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public CoveredWardController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetAllCoveredWardCodes()
        {
            var Ids = await _context.CoveredWards
                                           .Select(cd => cd.WardCode)
                                           .ToArrayAsync();

            return Ids;
        }

        [HttpGet("wards")]
        public IActionResult GetWards()
        {
            try
            {
                // Read the contents of the GeoJSON file
                string geoJsonData = System.IO.File.ReadAllText("wwwroot/wards.geojson");

                return Content(geoJsonData, "application/json");
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or return an appropriate error response
                return StatusCode(500, "An error occurred while reading the GeoJSON file.");
            }
        }

        [HttpGet("list")]
        public IActionResult GetAllCoveredWards()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT dbo.CoveredWards.Id, dbo.CoveredWards.WardCode, dbo.luWards.Name, dbo.luDistricts.Name AS District\r\nFROM     dbo.CoveredWards INNER JOIN\r\n                  dbo.luWards ON dbo.CoveredWards.WardCode = dbo.luWards.WardCode INNER JOIN\r\n                  dbo.luDistricts ON dbo.luWards.DistrictId = dbo.luDistricts.Id";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            if (dataTable.Rows.Count > 0)
                            {
                                var resultList = new List<Dictionary<string, object>>();

                                foreach (DataRow row in dataTable.Rows)
                                {
                                    var dict = new Dictionary<string, object>();
                                    foreach (DataColumn col in dataTable.Columns)
                                    {
                                        dict[col.ColumnName] = row[col];
                                    }
                                    resultList.Add(dict);
                                }

                                var jsonResult = JsonConvert.SerializeObject(resultList);
                                connection.Close();
                                return Content(jsonResult, "application/json");
                            }
                            else
                            {
                                connection.Close();
                                return BadRequest(404);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Coverage: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CoveredWard>> GetCoveredWardById(int id)
        {
            var coveredWard = await _context.CoveredWards.FindAsync(id);

            if (coveredWard == null)
            {
                return NotFound();
            }

            return coveredWard;
        }
        [HttpPost]
        public IActionResult Create([FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {

                    foreach (var id in ids)
                    {
                        var exists = _context.CoveredWards.FirstOrDefault(g => g.WardCode == id);

                        if (exists == null)
                        {
                            CoveredWard coveredWard = new CoveredWard();
                            coveredWard.ProjectId = 1;
                            coveredWard.WardCode = id;

                            _context.CoveredWards.Add(coveredWard);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(201);

                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoveredWard(int id, CoveredWard coveredWard)
        {
            if (id != coveredWard.Id)
            {
                return BadRequest();
            }

            _context.Entry(coveredWard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoveredWardExists(id))
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
        public async Task<IActionResult> DeleteCoveredWard(int id)
        {
            var coveredWard = await _context.CoveredWards.FindAsync(id);
            if (coveredWard == null)
            {
                return NotFound();
            }

            _context.CoveredWards.Remove(coveredWard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoveredWardExists(int id)
        {
            return _context.CoveredWards.Any(e => e.Id == id);
        }
    }
}
