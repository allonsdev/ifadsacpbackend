namespace sacpapi.Data.Services
{
    using Microsoft.Data.SqlClient;
    using System.Data;
    using System.Text.Json;
    using OfficeOpenXml;

    public class ParticipantActivityService
    {
        private readonly string _connectionString;

        public ParticipantActivityService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnectionString");
        }

        // 1️⃣ Return DataTable
        public async Task<DataTable> GetParticipantActivityAsync()
        {
            var table = new DataTable();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("dbo.GetParticipantActivityPivot", conn)
            {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            table.Load(reader);

            return table;
        }

        // 2️⃣ Convert DataTable to JSON
        public string DataTableToJson(DataTable table)
        {
            var rows = new List<Dictionary<string, object>>();

            foreach (DataRow dr in table.Rows)
            {
                var row = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                    row[col.ColumnName] = dr[col];
                rows.Add(row);
            }

            return JsonSerializer.Serialize(rows);
        }

        // 3️⃣ Generate Excel File
        public byte[] GenerateExcel(DataTable table)
        {
            // EPPlus 8+ license - must be called before any ExcelPackage creation
            ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // set your name here

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Participant Activity");

            // Headers
            for (int col = 0; col < table.Columns.Count; col++)
            {
                ws.Cells[1, col + 1].Value = table.Columns[col].ColumnName;
                ws.Cells[1, col + 1].Style.Font.Bold = true;
            }

            // Data
            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    // null-safe assignment
                    ws.Cells[row + 2, col + 1].Value = table.Rows[row][col]?.ToString() ?? "";
                }
            }

            ws.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }

}
