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
    public class CoveredDistrictController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public CoveredDistrictController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet]
        public IActionResult GetAllCoveredDistrictIds()
        {
            var coveredDistricts = _context.CoveredDistricts
                .Join(_context.luDistricts,
                    cd => cd.DistrictId,
                    d => d.Id,
                    (cd, d) => d.Name)
                .ToArray();

            return Ok(coveredDistricts);
        }

        [HttpGet("names")]
        public IActionResult GetAllCoveredDistrictNames()
        {
            var result = from district in _context.luDistricts
                         join coveredDistrict in _context.CoveredDistricts
                         on district.Id equals coveredDistrict.DistrictId
                         join province in _context.luProvinces
                         on district.ProvinceId equals province.Id // Adjust if necessary
                         select new
                         {
                             DistrictName = district.Name,
                             ProvinceName = province.Name
                         };


            return Json(result.ToList());

        }
        [HttpGet("name")]
        public IActionResult GetAllCoveredDistrictNames([FromQuery] string? provinceName)
        {
            var query = from district in _context.luDistricts
                        join coveredDistrict in _context.CoveredDistricts
                        on district.Id equals coveredDistrict.DistrictId
                        join province in _context.luProvinces
                        on district.ProvinceId equals province.Id
                        where string.IsNullOrEmpty(provinceName) || province.Name == provinceName // Filter before selection
                        select new
                        {
                            DistrictName = district.Name
                        };

            return Json(query.ToList());
        }

        [HttpGet("names/byProvince")]
        public IActionResult GetCoveredDistrictNamesByProvince(string provinceName)
        {
            var result = from district in _context.luDistricts
                         join coveredDistrict in _context.CoveredDistricts
                         on district.Id equals coveredDistrict.DistrictId
                         join province in _context.luProvinces
                         on district.ProvinceId equals province.Id
                         where province.Name == provinceName // Apply filter for province name
                         select new
                         {
                             DistrictName = district.Name,
                             ProvinceName = province.Name
                         };

            return Json(result.ToList());
        }

        [HttpGet("list")]
        public IActionResult GetAllCoveredDistricts()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT dbo.CoveredDistricts.Id, dbo.CoveredDistricts.DistrictId, dbo.luDistricts.Name, dbo.luProvinces.Name AS Province " +
                        "FROM     dbo.CoveredDistricts INNER JOIN" +
                        "               dbo.luDistricts ON dbo.CoveredDistricts.DistrictId = dbo.luDistricts.Id INNER JOIN" +
                        "          dbo.luProvinces ON dbo.luDistricts.ProvinceId = dbo.luProvinces.Id";

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
                                return Ok(jsonResult);
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
        public async Task<ActionResult<CoveredDistrict>> GetCoveredDistrictById(int id)
        {
            var coveredDistrict = await _context.CoveredDistricts.FindAsync(id);

            if (coveredDistrict == null)
            {
                return NotFound();
            }

            return coveredDistrict;
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
                        var exists = _context.CoveredDistricts.FirstOrDefault(g => g.DistrictId == id);

                        if (exists == null)
                        {
                            CoveredDistrict coveredDistrict = new CoveredDistrict();
                            coveredDistrict.ProjectId = 1;
                            coveredDistrict.DistrictId = id;

                            _context.CoveredDistricts.Add(coveredDistrict);
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
        public async Task<IActionResult> UpdateCoveredDistrict(int id, CoveredDistrict coveredDistrict)
        {
            if (id != coveredDistrict.Id)
            {
                return BadRequest();
            }

            _context.Entry(coveredDistrict).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoveredDistrictExists(id))
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
        public async Task<IActionResult> DeleteCoveredDistrict(int id)
        {
            var coveredDistrict = await _context.CoveredDistricts.FindAsync(id);
            if (coveredDistrict == null)
            {
                return NotFound();
            }

            _context.CoveredDistricts.Remove(coveredDistrict);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoveredDistrictExists(int id)
        {
            return _context.CoveredDistricts.Any(e => e.Id == id);
        }
    }
}
