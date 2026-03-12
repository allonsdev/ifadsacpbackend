using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using sacpapi.Data;
using System.Data;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class FarmerDiaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public FarmerDiaryController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet("inputs/{id}")]
        public IActionResult getInputs(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Select * from FarmerInputs where FarmerId =" + id;

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

        [HttpGet("inputs")]
        public IActionResult getAllInputs(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Select * from FarmerInputs";

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


        [HttpGet("sales")]
        public IActionResult getAllSales(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Select * from FarmerSales";

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

        [HttpGet("sales/{id}")]
        public IActionResult getSales(int id)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "Select * from FarmerSales where FarmerId =" + id;

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
    }
}
