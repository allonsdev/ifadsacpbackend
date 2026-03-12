using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sacpapi.Data.Services;
using sacpapi.Models;
using System.Data;
using System.Text.Json.Nodes;


namespace sacpapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParameterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public ParameterController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }
    

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAll([FromBody] JObject data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                dynamic jsonData = data;

                string sql = jsonData.SelectQuery;

                using (var command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);

                        if (dataTable.Rows.Count > 0)
                        {
                            string jsonResult = JsonConvert.SerializeObject(dataTable);
                            return Json(jsonResult);
                        }
                        else
                        {
                            var columns = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();
                            return Json(columns);
                        }
                    }
                }
            }
        }


        [Route("{tablename}")]
        [HttpPost]
        public IActionResult AddParameter(string tablename, [FromBody] JObject data)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                dynamic jsonData = data;

                var columns = new List<string>();
                var values = new List<string>();

                foreach (var property in jsonData.Properties())
                {
                    columns.Add(property.Name);

                    if (property.Value.Type == JTokenType.String)
                    {
                        values.Add($"'{property.Value}'");
                    }
                    else if (property.Value.Type == JTokenType.Integer || property.Value.Type == JTokenType.Float)
                    {
                        values.Add(property.Value.ToString());
                    }
                    else if (property.Value.Type == JTokenType.Null)
                    {
                        values.Add("NULL");
                    }
                }

                string columnsStr = string.Join(", ", columns);
                string valuesStr = string.Join(", ", values);

                string sql = $"INSERT INTO {tablename} ({columnsStr}) VALUES ({valuesStr})";

                using (var command = new SqlCommand(sql, connection))
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return StatusCode(200);
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
            }
        }

        [Route("{tablename}/delete")]
        [HttpPost]
        public IActionResult DeleteRecord(string tablename,[FromBody] JObject requestBody)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                if (requestBody == null)
                {
                    return BadRequest("Invalid request body"); // Return a bad request response if the request body is null
                }
                int id = requestBody["id"]?.Value<int>() ?? 0;
                string fieldName = requestBody["primaryKey"].ToString();
                connection.Open();

                string sql = $"DELETE FROM {tablename} WHERE {fieldName} = @Id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return StatusCode(200);
                    }
                    else
                    {
                        return StatusCode(404);
                    }
                }
            }
        }







        [Route("{tablename}")]
        [HttpGet]
        public IActionResult GetValues(string tablename)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "Select * from " + tablename + " Order by Name asc";

                using (var command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            string jsonResult = JsonConvert.SerializeObject(dataTable);
                            return Json(jsonResult);
                        }
                        else
                        {
                            return BadRequest();
                        }
                       
                    }
                }
            }
        }
    }
}
