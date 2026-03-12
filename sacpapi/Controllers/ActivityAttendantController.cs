using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sacpapi.Data.Services;
using sacpapi.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class ActivityAttendantController : Controller
    {
        private IActivityAttendants _service;

        public ActivityAttendantController(IActivityAttendants service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromBody] dynamic data)
        {
            try
            {
                if (data is not null && data.ValueKind == JsonValueKind.Array)

                {

                    var jsonArray = JsonConvert.DeserializeObject<dynamic[]>(data.ToString());
                    var successful = 0;
                    var unsuccessful = 0;

                    foreach (var obj in jsonArray)
                    {

                        try
                        {
                            ActivityAttendant attendant = new ActivityAttendant();
                            attendant.KoboBeneficiaryId = obj["Kobo Beneficiary ID"];
                            attendant.FormhubUuid = obj["Formhub Uuid"];
                            attendant.ActivityName = obj["Activity Name"];
                            attendant.Province = obj["Province"];
                            attendant.District = obj["District"];
                            attendant.Ward = obj["Ward"];
                            if (obj["Date"] != null && DateTime.TryParse(obj["Date"].ToString(), out DateTime dateValue))
                            {
                                attendant.Date = DateTime.MinValue;
                            }
                            if (obj["Date of Birth"] != null && DateTime.TryParse(obj["Date of Birth"].ToString(), out DateTime dateOfBirthValue))
                            {
                                attendant.DateOfBirth = DateTime.MinValue;
                            }
                            if (obj["Submission Time"] != null && DateTime.TryParse(obj["Submission Time"].ToString(), out DateTime submissionTimeValue))
                            {
                                attendant.SubmissionTime = DateTime.MinValue;
                            }
                            attendant.Enumerator = obj["Enumerator"];
                            attendant.HouseholdHead = obj["Household Head"];
                            attendant.HouseholdHeadSex = obj["Household Head Sex"];
                            attendant.OrganizationName = obj["Organization Name"];
                            attendant.IdNumber = obj["ID Number"];
                            attendant.PhoneNumber = obj["Phone Number"];
                            attendant.MetaInstanceID = obj["Meta Instance ID"];
                            attendant.XFormIdString = obj["X Form ID String"];
                            attendant.Uuid = obj["UUID"];
                            attendant.SubmissionStatus = obj["Submission Status"];
                            attendant.SubmittedBy = obj["Submitted By"];

                            _service.Add(attendant);
                            successful += 1;
                        }
                        catch (Exception)
                        {
                            unsuccessful += 1;
                            continue;
                        }
                    }
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
                    return StatusCode(500, $"Cannot Save Empty Data");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
    
}
