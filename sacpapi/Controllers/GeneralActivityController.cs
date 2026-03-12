using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using sacpapi.Data;
using sacpapi.Data.Services;
using sacpapi.Models;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class GeneralActivityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        private readonly IAuditTrail auditTrail;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public GeneralActivityController(ApplicationDbContext context, IConfiguration configuration, IAuditTrail auditTrail)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
            this.auditTrail = auditTrail;
            //_httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var generalActivities = _context.GeneralActivities.ToList();

            //var username = _httpContextAccessor.HttpContext.User.Identity.Name;

            //// Log the action
            //auditTrail.Log("GeneralActivities", 1, "Create", null, "new", username);

            return Json(generalActivities);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.GeneralActivities.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }


        // GET: api/Participants/DownloadParticipants/{gtid}
        [HttpGet("DownloadParticipants/{gtid}")]
        public async Task<IActionResult> DownloadParticipants(int gtid)
        {
            // 1️⃣ Retrieve participants with the given GTID
            var participants = await _context.Participants
                .Where(p => p.Gtid == gtid)
                .OrderBy(p => p.HhNationalId)
                .ToListAsync();

            if (!participants.Any())
                return NotFound($"No participants found for GeneralActivityID {gtid}.");

            ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani");
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Participants");

            // 3️⃣ Add header row
            var headers = new[]
            {
                "HhNationalId",
                "FirstName",
                "Surname",
                "Gender",
                "YearOfBirth",
                "GroupType",
                "CategoryName",
                "ParticipantNationalId",
                "PwdTin",
                "HhGender",
                "ContactNumber",
                "CreatedBy"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
            }

            // 4️⃣ Populate rows
            int row = 2;
            foreach (var p in participants)
            {
                worksheet.Cells[row, 1].Value = p.HhNationalId;
                worksheet.Cells[row, 2].Value = p.FirstName;
                worksheet.Cells[row, 3].Value = p.Surname;
                worksheet.Cells[row, 4].Value = p.Gender;
                worksheet.Cells[row, 5].Value = p.YearOfBirth;
                worksheet.Cells[row, 6].Value = p.GroupType;
                worksheet.Cells[row, 7].Value = p.CategoryName;
                worksheet.Cells[row, 8].Value = p.ParticipantNationalId;
                worksheet.Cells[row, 9].Value = p.PwdTin;
                worksheet.Cells[row, 10].Value = p.HhGender;
                worksheet.Cells[row, 11].Value = p.ContactNumber;
                worksheet.Cells[row, 12].Value = p.Createdby;
                row++;
            }

            // 5️⃣ Auto-fit columns
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // 6️⃣ Convert Excel package to byte array
            var excelData = package.GetAsByteArray();

            // 7️⃣ Return as downloadable file
            var fileName = $"Participants_{gtid}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(excelData,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName);
        }
        [HttpGet("detailed")]
        public IActionResult Detailed()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Select all columns from the table/view
                    string sql = "SELECT * FROM [dbo].[GeneralActivitiesDetailed] ORDER BY StartDate desc";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            var settings = new JsonSerializerSettings
                            {
                                DateFormatString = "yyyy/MM/dd"
                            };

                            // Serialize DataTable to JSON
                            string jsonResult = JsonConvert.SerializeObject(dataTable, settings);

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

        [HttpPost("{id}")]
        public IActionResult GetFacilitators(int id)
        {
            var facilitatorIds = _context.Facilitators
         .Where(f => f.GeneralActivityId == id)
         .Select(f => f.FacilitatorId)
         .ToArray();

            return Json(facilitatorIds);
        }

        [HttpPost]
        public IActionResult CreateGeneralActivity([FromBody] CreateGeneralActivityModel model)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.GeneralActivities.Add(model.GeneralActivity);
                        _context.SaveChanges();

                        var generalActivityId = model.GeneralActivity.Id;

                        if (model.FacilitatorIds.Length > 0)
                        {
                            foreach (var id in model.FacilitatorIds)
                            {
                                var existingFacilitator = _context.Facilitators.FirstOrDefault(f => f.FacilitatorId == id && f.GeneralActivityId == generalActivityId);
                                if (existingFacilitator == null)
                                {
                                    var facilitator = new Facilitator { FacilitatorId = id, GeneralActivityId = generalActivityId };
                                    _context.Facilitators.Add(facilitator);
                                }
                            }
                        }

                        _context.SaveChanges();
                        transaction.Commit();

                        //var username = _httpContextAccessor.HttpContext.User.Identity.Name;

                        //// Log the action
                        //auditTrail.Log("GeneralActivities", generalActivityId, "Create", null, JsonConvert.SerializeObject(model.GeneralActivity), username);

                        return StatusCode(201);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateGeneralActivity(int id, [FromBody] CreateGeneralActivityModel model)
        {
            var existingGeneralActivity = _context.GeneralActivities.Find(id);
            if (existingGeneralActivity == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var generalActivity = model.GeneralActivity;
                using (var transaction = _context.Database.BeginTransaction())
                {

                    try
                    {
                        existingGeneralActivity.ProjectId = generalActivity.ProjectId;
                        existingGeneralActivity.OrganisationId = generalActivity.OrganisationId;
                        existingGeneralActivity.SubComponentId = generalActivity.SubComponentId;
                        existingGeneralActivity.InterventionId = generalActivity.InterventionId;
                        existingGeneralActivity.ActivityTypeId = generalActivity.ActivityTypeId;
                        existingGeneralActivity.ActivityId = generalActivity.ActivityId;
                        existingGeneralActivity.ActiveStatusId = generalActivity.ActiveStatusId;
                        existingGeneralActivity.DistrictId = generalActivity.DistrictId;
                        existingGeneralActivity.Site = generalActivity.Site;
                        existingGeneralActivity.SiteType = generalActivity.SiteType;
                        existingGeneralActivity.IrrigationSchemeId = generalActivity.IrrigationSchemeId;
                        existingGeneralActivity.WaterpointId = generalActivity.WaterpointId;
                        existingGeneralActivity.LeadFacilitatorId = generalActivity.LeadFacilitatorId;
                        existingGeneralActivity.Details = generalActivity.Details;
                        existingGeneralActivity.IssuesComments = generalActivity.IssuesComments;
                        existingGeneralActivity.StartDate = generalActivity.StartDate;
                        existingGeneralActivity.EndDate = generalActivity.EndDate;
                        existingGeneralActivity.CreatedBy = generalActivity.CreatedBy;
                        existingGeneralActivity.CreatedDate = generalActivity.CreatedDate;
                        existingGeneralActivity.UpdatedBy = generalActivity.UpdatedBy;
                        existingGeneralActivity.UpdatedDate = generalActivity.UpdatedDate;

                        var existingFacilitator = _context.Facilitators.Where(f => f.GeneralActivityId == id).ToList();
                        _context.Facilitators.RemoveRange(existingFacilitator);

                        if (model.FacilitatorIds.Length > 0)
                        {
                            foreach (var fid in model.FacilitatorIds)
                            {
                                var facilitator = new Facilitator { FacilitatorId = fid, GeneralActivityId = id };
                                _context.Facilitators.Add(facilitator);
                            }
                        }

                        _context.SaveChanges();
                        transaction.Commit();
                        return StatusCode(200);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return BadRequest(ex.Message);
                    }
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGeneralActivity(int id)
        {
            var generalActivity = _context.GeneralActivities.Find(id);
            if (generalActivity == null)
            {
                return NotFound();
            }

            try
            {
                _context.GeneralActivities.Remove(generalActivity);
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
