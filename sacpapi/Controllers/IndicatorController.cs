using sacpapi.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using sacpapi.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class IndicatorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public IndicatorController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        public IActionResult Index()
        {
            var indicators = _context.Indicators.ToList();
            return Json(indicators);
        }

        [HttpGet("tracking")]
        public IActionResult tracking()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM [dbo].[DetailedIndicators]";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<object> result = new List<object>();

                            while (reader.Read())
                            {
                                int id = reader.GetInt32("Id");
                                var indicator = new
                                {
                                    Id = reader["Id"],
                                    Name = reader["Name"],
                                    Type = reader["IndicatorType"],
                                    Definition = reader["Definition"],
                                    DataCollectionFrequency = reader["DataCollectionFrequency"],
                                    DataSource = reader["DataSource"],
                                    Unit = reader["Unit"],
                                    ProgramTargetValue = reader["ProgramTargetValue"],
                                    ChildData = _context.IndicatorTargets.Where(it => it.IndicatorId == id).ToList()
                                };

                                result.Add(indicator);
                            }

                            string jsonResult = JsonConvert.SerializeObject(result);
                            connection.Close();
                            return Json(jsonResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.Indicators.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }
        [HttpGet("detailed")]
        public IActionResult detailed()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM [dbo].[DetailedIndicators]";

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
        public IActionResult CreateIndicator([FromBody] Indicator indicator)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    indicator.CreatedBy = "ph";
                    indicator.UpdatedBy = "ph";
                    _context.Indicators.Add(indicator);
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
        public IActionResult UpdateIndicator(int id, [FromBody] Indicator indicator)
        {
            var existingIndicator = _context.Indicators.Find(id);
            if (existingIndicator == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingIndicator.IndicatorTypeId = indicator.IndicatorTypeId;
                    existingIndicator.IndicatorCategoryId = indicator.IndicatorCategoryId;
                    existingIndicator.ObjectiveId = indicator.ObjectiveId;
                    existingIndicator.OutcomeId = indicator.OutcomeId;
                    existingIndicator.OutputId = indicator.OutputId;
                    existingIndicator.ActivityId = indicator.ActivityId;
                    existingIndicator.Name = indicator.Name;
                    existingIndicator.Definition = indicator.Definition;
                    existingIndicator.Description = indicator.Description;
                    existingIndicator.UnitOfMeasurementId = indicator.UnitOfMeasurementId;
                    existingIndicator.BaselineValue = indicator.BaselineValue;
                    existingIndicator.DataSourceId = indicator.DataSourceId;
                    existingIndicator.DataCollectionMethodId = indicator.DataCollectionMethodId;
                    existingIndicator.ToolId = indicator.ToolId;
                    existingIndicator.DataCollectionFrequencyId = indicator.DataCollectionFrequencyId;
                    existingIndicator.ResponsibleParty = indicator.ResponsibleParty;
                    existingIndicator.ProgramTargetValue = indicator.ProgramTargetValue;
                    existingIndicator.CreatedBy = indicator.CreatedBy;
                    existingIndicator.CreatedDate = indicator.CreatedDate;
                    existingIndicator.UpdatedBy = indicator.UpdatedBy;
                    existingIndicator.UpdatedDate = indicator.UpdatedDate;

                    _context.SaveChanges();

                    return StatusCode(200);
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

        [HttpDelete("{id}")]
        public IActionResult DeleteIndicator(int id)
        {
            var indicator = _context.Indicators.Find(id);
            if (indicator == null)
            {
                return NotFound();
            }
            try
            {
                _context.Indicators.Remove(indicator);
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
