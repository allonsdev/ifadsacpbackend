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
    [Route("[controller]")]
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public GroupController(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Groups.ToList();
            return Json(data);
        }

        [HttpGet("array")]
        public IActionResult IdsArray()
        {
            var data = _context.Groups.Select(b => b._id).ToArray();
            return Json(data);
        }

        [HttpGet("valuechains")]
        public IActionResult GetValueChain()
        {
            var distinctValueChains = _context.Groups
                .GroupBy(e => e.valuechain)
                .Select(group => group.Key)
                .ToArray();
            return Json(distinctValueChains);
        }

        [HttpGet("{value}")]
        public IActionResult GetbyValueChain(string value)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * from Groups where valuechain ='" + value +"'";

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
                                return Content(jsonResult);
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
                return StatusCode(500, $"Error retrieving groups: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult Create([FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    var successful = 0;
                    var unsuccessful = 0;

                    foreach (var id in ids)
                    {
                        try
                        {
                            var existingEntity = _context.APGroups.FirstOrDefault(e => e._id == id);

                            if (existingEntity != null)
                            {
                                var existing2Entity = _context.Groups.FirstOrDefault(e => e._id == id);

                                if (existing2Entity == null)
                                {

                                    Group newEntity = new Group();

                                    newEntity.province = existingEntity.province;
                                    newEntity.district = existingEntity.district;
                                    newEntity.ward = existingEntity.ward;
                                    newEntity.nameapg = existingEntity.nameapg;
                                    newEntity.point = existingEntity.point;
                                    newEntity.namevil = existingEntity.namevil;
                                    newEntity.yearest = existingEntity.yearest;
                                    newEntity.nointeger = existingEntity.nointeger;
                                    newEntity.females = existingEntity.females;
                                    newEntity.males = existingEntity.males;
                                    newEntity.youth = existingEntity.youth;
                                    newEntity.youth_001 = existingEntity.youth_001;
                                    newEntity.subsector = existingEntity.subsector;
                                    newEntity.valuechain = existingEntity.valuechain;
                                    newEntity.enumerator = existingEntity.enumerator;
                                    newEntity.contact = existingEntity.contact;
                                    newEntity.chair = existingEntity.chair;
                                    newEntity.sex = existingEntity.sex;
                                    newEntity.age = existingEntity.age;
                                    newEntity.category = existingEntity.category;
                                    newEntity.vice = existingEntity.vice;
                                    newEntity.sex_001 = existingEntity.sex_001;
                                    newEntity.age_001 = existingEntity.age_001;
                                    newEntity.category_001 = existingEntity.category_001;
                                    newEntity.sec = existingEntity.sec;
                                    newEntity.sex_002 = existingEntity.sex_002;
                                    newEntity.age_002 = existingEntity.age_002;
                                    newEntity.category_002 = existingEntity.category_002;
                                    newEntity.ward_001 = existingEntity.ward_001;
                                    newEntity.sex_003 = existingEntity.sex_003;
                                    newEntity.age_003 = existingEntity.age_003;
                                    newEntity.category_003 = existingEntity.category_003;
                                    newEntity.treas = existingEntity.treas;
                                    newEntity.sex_004 = existingEntity.sex_004;
                                    newEntity.age_004 = existingEntity.age_004;
                                    newEntity.category_004 = existingEntity.category_004;
                                    newEntity.group = existingEntity.group;
                                    newEntity.sex_005 = existingEntity.sex_005;
                                    newEntity.age_005 = existingEntity.age_005;
                                    newEntity.category_005 = existingEntity.category_005;
                                    newEntity.group_001 = existingEntity.group_001;
                                    newEntity.sex_006 = existingEntity.sex_006;
                                    newEntity.age_006 = existingEntity.age_006;
                                    newEntity.category_006 = existingEntity.category_006;
                                    newEntity.group_002 = existingEntity.group_002;
                                    newEntity.sex_007 = existingEntity.sex_007;
                                    newEntity.age_007 = existingEntity.age_007;
                                    newEntity.category_007 = existingEntity.category_007;
                                    newEntity.group_003 = existingEntity.group_003;
                                    newEntity.sex_008 = existingEntity.sex_008;
                                    newEntity.age_008 = existingEntity.age_008;
                                    newEntity.category_008 = existingEntity.category_008;
                                    newEntity.group_004 = existingEntity.group_004;
                                    newEntity.sex_009 = existingEntity.sex_009;
                                    newEntity.age_009 = existingEntity.age_009;
                                    newEntity.category_009 = existingEntity.category_009;
                                    newEntity.group_005 = existingEntity.group_005;
                                    newEntity.sex_010 = existingEntity.sex_010;
                                    newEntity.age_010 = existingEntity.age_010;
                                    newEntity.category_010 = existingEntity.category_010;
                                    newEntity.group_006 = existingEntity.group_006;
                                    newEntity.sex_011 = existingEntity.sex_011;
                                    newEntity.age_011 = existingEntity.age_011;
                                    newEntity.category_011 = existingEntity.category_011;
                                    newEntity.group_007 = existingEntity.group_007;
                                    newEntity.sex_012 = existingEntity.sex_012;
                                    newEntity.age_012 = existingEntity.age_012;
                                    newEntity.category_012 = existingEntity.category_012;
                                    newEntity.group_008 = existingEntity.group_008;
                                    newEntity.sex_013 = existingEntity.sex_013;
                                    newEntity.age_013 = existingEntity.age_013;
                                    newEntity.category_013 = existingEntity.category_013;
                                    newEntity.group_009 = existingEntity.group_009;
                                    newEntity.sex_014 = existingEntity.sex_014;
                                    newEntity.age_014 = existingEntity.age_014;
                                    newEntity.category_014 = existingEntity.category_014;
                                    newEntity.group_010 = existingEntity.group_010;
                                    newEntity.sex_015 = existingEntity.sex_015;
                                    newEntity.age_015 = existingEntity.age_015;
                                    newEntity.category_015 = existingEntity.category_015;
                                    newEntity.group_011 = existingEntity.group_011;
                                    newEntity.sex_016 = existingEntity.sex_016;
                                    newEntity.age_016 = existingEntity.age_016;
                                    newEntity.category_016 = existingEntity.category_016;
                                    newEntity.group_012 = existingEntity.group_012;
                                    newEntity.sex_017 = existingEntity.sex_017;
                                    newEntity.age_017 = existingEntity.age_017;
                                    newEntity.category_017 = existingEntity.category_017;
                                    newEntity.group_013 = existingEntity.group_013;
                                    newEntity.sex_018 = existingEntity.sex_018;
                                    newEntity.age_018 = existingEntity.age_018;
                                    newEntity.category_018 = existingEntity.category_018;
                                    newEntity.group_014 = existingEntity.group_014;
                                    newEntity.sex_019 = existingEntity.sex_019;
                                    newEntity.age_019 = existingEntity.age_019;
                                    newEntity.category_019 = existingEntity.category_019;
                                    newEntity.group_015 = existingEntity.group_015;
                                    newEntity.sex_020 = existingEntity.sex_020;
                                    newEntity.age_020 = existingEntity.age_020;
                                    newEntity.category_020 = existingEntity.category_020;
                                    newEntity.group_016 = existingEntity.group_016;
                                    newEntity.sex_021 = existingEntity.sex_021;
                                    newEntity.age_021 = existingEntity.age_021;
                                    newEntity.category_021 = existingEntity.category_021;
                                    newEntity.group_017 = existingEntity.group_017;
                                    newEntity.sex_022 = existingEntity.sex_022;
                                    newEntity.age_022 = existingEntity.age_022;
                                    newEntity.category_022 = existingEntity.category_022;
                                    newEntity.group_018 = existingEntity.group_018;
                                    newEntity.sex_023 = existingEntity.sex_023;
                                    newEntity.age_023 = existingEntity.age_023;
                                    newEntity.category_023 = existingEntity.category_023;
                                    newEntity.group_019 = existingEntity.group_019;
                                    newEntity.sex_024 = existingEntity.sex_024;
                                    newEntity.age_024 = existingEntity.age_024;
                                    newEntity.category_024 = existingEntity.category_024;
                                    newEntity.group_020 = existingEntity.group_020;
                                    newEntity.sex_025 = existingEntity.sex_025;
                                    newEntity.age_025 = existingEntity.age_025;
                                    newEntity.category_025 = existingEntity.category_025;
                                    newEntity.group_021 = existingEntity.group_021;
                                    newEntity.sex_026 = existingEntity.sex_026;
                                    newEntity.age_026 = existingEntity.age_026;
                                    newEntity.category_026 = existingEntity.category_026;
                                    newEntity.group_022 = existingEntity.group_022;
                                    newEntity.sex_027 = existingEntity.sex_027;
                                    newEntity.age_027 = existingEntity.age_027;
                                    newEntity.category_027 = existingEntity.category_027;
                                    newEntity.group_023 = existingEntity.group_023;
                                    newEntity.sex_028 = existingEntity.sex_028;
                                    newEntity.age_028 = existingEntity.age_028;
                                    newEntity.category_028 = existingEntity.category_028;
                                    newEntity.group_024 = existingEntity.group_024;
                                    newEntity.sex_029 = existingEntity.sex_029;
                                    newEntity.age_029 = existingEntity.age_029;
                                    newEntity.category_029 = existingEntity.category_029;
                                    newEntity.group_025 = existingEntity.group_025;
                                    newEntity.sex_030 = existingEntity.sex_030;
                                    newEntity.age_030 = existingEntity.age_030;
                                    newEntity.category_030 = existingEntity.category_030;
                                    newEntity.group_026 = existingEntity.group_026;
                                    newEntity.sex_031 = existingEntity.sex_031;
                                    newEntity.age_031 = existingEntity.age_031;
                                    newEntity.category_031 = existingEntity.category_031;
                                    newEntity.group_027 = existingEntity.group_027;
                                    newEntity.sex_032 = existingEntity.sex_032;
                                    newEntity.age_032 = existingEntity.age_032;
                                    newEntity.category_032 = existingEntity.category_032;
                                    newEntity.group_028 = existingEntity.group_028;
                                    newEntity.sex_033 = existingEntity.sex_033;
                                    newEntity.age_033 = existingEntity.age_033;
                                    newEntity.category_033 = existingEntity.category_033;
                                    newEntity.group_029 = existingEntity.group_029;
                                    newEntity.sex_034 = existingEntity.sex_034;
                                    newEntity.age_034 = existingEntity.age_034;
                                    newEntity.category_034 = existingEntity.category_034;
                                    newEntity._id = existingEntity._id;
                                    newEntity._uuid = existingEntity._uuid;
                                    newEntity._submission_time = existingEntity._submission_time;
                                    newEntity._status = existingEntity._status;
                                    newEntity._submitted_by = existingEntity._submitted_by;
                                    newEntity.__version__ = existingEntity.__version__;

                                    _context.Groups.Add(newEntity);
                                    successful += 1;
                                }
                                else
                                {
                                    unsuccessful += 1;
                                }
                            }
                            else
                            {
                                unsuccessful += 1;
                            }

                        }
                        catch (Exception)
                        {
                            unsuccessful += 1;
                            continue;
                        }
                    }
                    _context.SaveChanges();

                    var result = new
                    {
                        SuccessfulRecords = successful,
                        UnsuccessfulRecords = unsuccessful,
                        TotalRecords = successful + unsuccessful
                    };

                    return Ok(result);
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var groups = _context.Groups.Find(id);
            if (groups == null)
            {
                return NotFound();
            }
            try
            {
                _context.Groups.Remove(groups);
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

