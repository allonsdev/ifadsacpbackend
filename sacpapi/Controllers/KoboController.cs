using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sacpapi.Data;
using System;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;
namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class KoboController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string KoboApiBaseUrl = "https://kc.kobotoolbox.org/";
        private readonly string KoboApiEndpoint = "api/v1/data/2085032";
        private readonly string AccessToken = "b087792e3787c84913d1e62f7795bb5f133e5e59";
        private readonly string? connectionString;

        public KoboController(IHttpClientFactory httpClientFactory, ApplicationDbContext context, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet("GetKobo")]
        public async Task<IActionResult> FetchDataFromKobo()
        {
            try
            {
                using (HttpClient client = _httpClientFactory.CreateClient())
                {
                    client.BaseAddress = new Uri(KoboApiBaseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", AccessToken);
                    client.Timeout = TimeSpan.FromSeconds(20000);

                    var requestUrl = $"{client.BaseAddress}{KoboApiEndpoint}";
                    Console.WriteLine($"Requesting URL: {requestUrl}");

                    HttpResponseMessage response = await client.GetAsync(KoboApiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        // Deserialize the JSON array
                        var jsonArray = JsonConvert.DeserializeObject<JArray>(jsonString);
                        // HashSet to store distinct keys
                        HashSet<string> distinctKeys = new HashSet<string>();

                        foreach (var item in jsonArray)
                        {
                            if (item is JObject obj)
                            {
                                foreach (var property in obj.Properties())
                                {
                                    distinctKeys.Add(property.Name);
                                }
                            }
                        }

                        // Print all distinct keys
                        Console.WriteLine("Distinct Keys:");
                        foreach (var key in distinctKeys)
                        {
                            Console.WriteLine(key);
                        }



                        if (jsonArray != null && jsonArray.Count > 0)
                        {
                            var firstItem = jsonArray[0] as JObject;
                            string tableName = "KoboData";

                            // Create table if not exists
                            string createTableQuery = GenerateCreateTableQuery(firstItem, tableName);
                            ExecuteSqlQuery(createTableQuery);

                            // Insert data into table
                            foreach (var item in jsonArray.OfType<JObject>())
                            {
                                string insertQuery = GenerateInsertQuery(item, tableName);
                                ExecuteSqlQuery(insertQuery);
                            }

                            return Ok("Data fetched and inserted successfully.");
                        }
                        else
                        {
                            return Ok("No data found.");
                        }
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, $"Failed to fetch data from Kobo API. Status Code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateCreateTableQuery(JObject firstItem, string tableName)
        {
            var columns = string.Join(", ", firstItem.Properties().Select(p => $"[{p.Name}] NVARCHAR(MAX)"));
            return $"IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='{tableName}' AND xtype='U') " +
                   $"CREATE TABLE [{tableName}] ({columns})";
        }

        private string GenerateInsertQuery(JObject item, string tableName)
        {
            if (item == null || string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentException("Item and tableName cannot be null or empty.");
            }

            var columns = string.Join(", ", item.Properties().Select(p => $"[{p.Name}]"));
            var values = string.Join(", ", item.Properties().Select(p =>
            {
                var value = p.Value;
                if (value.Type == JTokenType.Null)
                    return "NULL";

                // Handle special characters in strings
                var valueStr = value.ToString().Replace("'", "''");
                return $"'{valueStr}'";
            }));

            var escapedTableName = tableName.Replace("'", "''"); // Escape single quotes

            return $"INSERT INTO [{escapedTableName}] ({columns}) VALUES ({values})";
        }


        private void ExecuteSqlQuery(string query)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
