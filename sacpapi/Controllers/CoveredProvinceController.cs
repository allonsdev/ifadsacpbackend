using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Models;
using System.Data;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CoveredProvinceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public CoveredProvinceController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetAllCoveredProvinceIds()
        {
            var provinceIds = await _context.CoveredProvinces
                                            .Select(cp => cp.ProvinceId)
                                            .ToArrayAsync();

            return provinceIds;
        }

        [HttpGet("names")]
        public IActionResult GetAllCoveredDistrictNames()
        {
            var result = from province in _context.luProvinces
                         join coveredProvince in _context.CoveredProvinces
                         on province.Id equals coveredProvince.ProvinceId
                         select new
                         {
                             ProvinceName = province.Name
                         };

            return Json(result.ToList());

        }

        [HttpGet("list")]
        public IActionResult GetAllCoveredProvinces()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT dbo.CoveredProvinces.Id, dbo.CoveredProvinces.ProvinceId, dbo.luProvinces.Name FROM dbo.CoveredProvinces INNER JOIN dbo.luProvinces ON dbo.CoveredProvinces.ProvinceId = dbo.luProvinces.Id";

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
                return StatusCode(500, $"Error retrieving files: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CoveredProvince>> GetCoveredProvinceById(int id)
        {
            var coveredProvince = await _context.CoveredProvinces.FindAsync(id);

            if (coveredProvince == null)
            {
                return NotFound();
            }

            return coveredProvince;
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
                        var exists = _context.CoveredProvinces.FirstOrDefault(g => g.ProvinceId == id);

                        if (exists == null)
                        {
                            CoveredProvince coveredProvince = new CoveredProvince();
                            coveredProvince.ProjectId = 1;
                            coveredProvince.ProvinceId = id;

                            _context.CoveredProvinces.Add(coveredProvince);
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
        public async Task<IActionResult> UpdateCoveredProvince(int id, CoveredProvince coveredProvince)
        {
            if (id != coveredProvince.Id)
            {
                return BadRequest();
            }

            _context.Entry(coveredProvince).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoveredProvinceExists(id))
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
        public async Task<IActionResult> DeleteCoveredProvince(int id)
        {
            var coveredProvince = await _context.CoveredProvinces.FindAsync(id);
            if (coveredProvince == null)
            {
                return NotFound();
            }

            _context.CoveredProvinces.Remove(coveredProvince);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoveredProvinceExists(int id)
        {
            return _context.CoveredProvinces.Any(e => e.Id == id);
        }
    }
}
