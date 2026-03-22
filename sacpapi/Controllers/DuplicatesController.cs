using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DuplicatesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public DuplicatesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        private async Task<IActionResult> QueryView(string viewName, string orderBy = null)
        {
            var results = new List<Dictionary<string, object>>();
            var sql = $"SELECT * FROM {viewName}" + (orderBy != null ? $" ORDER BY {orderBy}" : "");
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                results.Add(row);
            }
            return Ok(results);
        }

        private async Task<IActionResult> DeleteSingle(string fullTableName, string pkColumn, object id)
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand($"DELETE FROM {fullTableName} WHERE {pkColumn} = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            var rows = await cmd.ExecuteNonQueryAsync();
            return rows > 0
                ? Ok(new { success = true, deleted = id, message = $"Record {id} deleted successfully." })
                : NotFound(new { success = false, message = $"Record {id} not found." });
        }

        private async Task<IActionResult> BulkDelete(string fullTableName, string pkColumn, object[] ids)
        {
            if (ids == null || ids.Length == 0)
                return BadRequest(new { success = false, message = "No IDs provided." });

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            var paramNames = ids.Select((_, i) => $"@p{i}").ToArray();
            var sql = $"DELETE FROM {fullTableName} WHERE {pkColumn} IN ({string.Join(",", paramNames)})";
            await using var cmd = new SqlCommand(sql, conn);
            for (int i = 0; i < ids.Length; i++)
                cmd.Parameters.AddWithValue($"@p{i}", ids[i]);
            var rows = await cmd.ExecuteNonQueryAsync();
            return Ok(new { success = true, deleted = rows, message = $"{rows} record(s) deleted successfully." });
        }

        // ─── 1. MSEInfos ────────────────────────────────────────────────────────

        [HttpGet("mse-infos")]
        public Task<IActionResult> GetMSEInfoDuplicates()
            => QueryView("vw_DuplicateMSEInfos");

        [HttpDelete("mse-infos/{id:int}")]
        public Task<IActionResult> DeleteMSEInfo(int id)
            => DeleteSingle("[PrimasysSACP3].[dbo].[MSEInfos]", "Id", id);

        [HttpDelete("mse-infos/bulk")]
        public Task<IActionResult> BulkDeleteMSEInfos([FromBody] int[] ids)
            => BulkDelete("[PrimasysSACP3].[dbo].[MSEInfos]", "Id", ids.Cast<object>().ToArray());

        // ─── 2. EmploymentRecords ───────────────────────────────────────────────

        [HttpGet("employment-records")]
        public Task<IActionResult> GetEmploymentRecordDuplicates()
            => QueryView("vw_DuplicateEmploymentRecords");

        [HttpDelete("employment-records/{id:int}")]
        public Task<IActionResult> DeleteEmploymentRecord(int id)
            => DeleteSingle("EmploymentRecords", "Id", id);

        [HttpDelete("employment-records/bulk")]
        public Task<IActionResult> BulkDeleteEmploymentRecords([FromBody] int[] ids)
            => BulkDelete("EmploymentRecords", "Id", ids.Cast<object>().ToArray());

        // ─── 3. WaterUsers ──────────────────────────────────────────────────────

        [HttpGet("water-users")]
        public Task<IActionResult> GetWaterUserDuplicates()
            => QueryView("vw_DuplicateWaterUsers");

        [HttpDelete("water-users/{id:int}")]
        public Task<IActionResult> DeleteWaterUser(int id)
            => DeleteSingle("WaterUsers", "Id", id);

        [HttpDelete("water-users/bulk")]
        public Task<IActionResult> BulkDeleteWaterUsers([FromBody] int[] ids)
            => BulkDelete("WaterUsers", "Id", ids.Cast<object>().ToArray());

        // ─── 4. SchoolBusinessUnits ─────────────────────────────────────────────

        [HttpGet("school-business-units")]
        public Task<IActionResult> GetSchoolBusinessUnitDuplicates()
            => QueryView("vw_DuplicateSchoolBusinessUnits");

        [HttpDelete("school-business-units/{id:int}")]
        public Task<IActionResult> DeleteSchoolBusinessUnit(int id)
            => DeleteSingle("SchoolBusinessUnits", "Id", id);

        [HttpDelete("school-business-units/bulk")]
        public Task<IActionResult> BulkDeleteSchoolBusinessUnits([FromBody] int[] ids)
            => BulkDelete("SchoolBusinessUnits", "Id", ids.Cast<object>().ToArray());

        // ─── 5. RoadUsers ───────────────────────────────────────────────────────

        [HttpGet("road-users")]
        public Task<IActionResult> GetRoadUserDuplicates()
            => QueryView("vw_DuplicateRoadUsers");

        [HttpDelete("road-users/{id:int}")]
        public Task<IActionResult> DeleteRoadUser(int id)
            => DeleteSingle("RoadUsers", "Id", id);

        [HttpDelete("road-users/bulk")]
        public Task<IActionResult> BulkDeleteRoadUsers([FromBody] int[] ids)
            => BulkDelete("RoadUsers", "Id", ids.Cast<object>().ToArray());

        // ─── 6. IrrigationSchemes ───────────────────────────────────────────────

        [HttpGet("irrigation-schemes")]
        public Task<IActionResult> GetIrrigationSchemeDuplicates()
            => QueryView("vw_DuplicateIrrigationSchemes");

        [HttpDelete("irrigation-schemes/{householdIdentifierNumber}")]
        public Task<IActionResult> DeleteIrrigationScheme(string householdIdentifierNumber)
            => DeleteSingle("[PrimasysSACP3].[dbo].[tblrrigationSchemesDatabase]", "HouseholdIdentifierNumber", householdIdentifierNumber);

        [HttpDelete("irrigation-schemes/bulk")]
        public Task<IActionResult> BulkDeleteIrrigationSchemes([FromBody] string[] ids)
            => BulkDelete("[PrimasysSACP3].[dbo].[tblrrigationSchemesDatabase]", "HouseholdIdentifierNumber", ids.Cast<object>().ToArray());

        // ─── 7. VBUS ────────────────────────────────────────────────────────────

        [HttpGet("vbus")]
        public Task<IActionResult> GetVBUSDuplicates()
            => QueryView("vw_DuplicateVBUS");

        [HttpDelete("vbus/{id}")]
        public Task<IActionResult> DeleteVBUS(string id)
            => DeleteSingle("[PrimasysSACP3].[dbo].[VBUS]", "ID", id);

        [HttpDelete("vbus/bulk")]
        public Task<IActionResult> BulkDeleteVBUS([FromBody] string[] ids)
            => BulkDelete("[PrimasysSACP3].[dbo].[VBUS]", "ID", ids.Cast<object>().ToArray());

        // ─── 8. BeneficiaryV3 ───────────────────────────────────────────────────

        [HttpGet("beneficiary-v3")]
        public Task<IActionResult> GetBeneficiaryV3Duplicates()
            => QueryView("vw_BeneficiaryDuplicates", "HouseholdIdentifierNumber");

        [HttpDelete("beneficiary-v3/{id:int}")]
        public Task<IActionResult> DeleteBeneficiaryV3(int id)
            => DeleteSingle("[PrimasysSACP3].[dbo].[BeneficiaryV3]", "ID", id);

        [HttpDelete("beneficiary-v3/bulk")]
        public Task<IActionResult> BulkDeleteBeneficiaryV3([FromBody] int[] ids)
            => BulkDelete("[PrimasysSACP3].[dbo].[BeneficiaryV3]", "ID", ids.Cast<object>().ToArray());
    }
}