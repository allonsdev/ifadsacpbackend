using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using sacpapi.Data;
using System.Data;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ParticipationMatrixController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public ParticipationMatrixController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }
        [HttpGet("beneficiaries/{countmethod}")]
        public IActionResult GetBeneficiaryActivities(int countmethod)
        {
            // Fetch all required data at once to reduce multiple database calls
            var beneficiaries = _context.Beneficiaryv3.ToList();
            var activities = _context.Activities.ToList();
            var generalActivities = _context.GeneralActivities.ToList();
            var participants = _context.GeneralActivityParticipants.ToList();

            // Create a lookup dictionary for GeneralActivities
            var generalActivitiesByActivityId = generalActivities
                .GroupBy(a => a.ActivityId)
                .ToDictionary(g => g.Key, g => g.Select(a => a.Id).ToHashSet());

            // Create a lookup dictionary for Participants with ParticipantTypeId = 1
            var participantLookup = participants
                .Where(p => p.ParticipantTypeId == 1)
                .GroupBy(p => p.ParticipantId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<Dictionary<string, object>>();

            foreach (var beneficiary in beneficiaries)
            {
                var beneficiaryData = new Dictionary<string, object>
        {
            { "Beneficiary", $"{beneficiary.Name} {beneficiary.Surname}" },
            { "District", beneficiary.District },
            { "Ward", beneficiary.Ward },
            { "Contact", beneficiary.ContactNumber },
            { "HouseHoldIdentifier", beneficiary.HouseholdIdentifierNumber }
        };

                int participantcount = 0;

                foreach (var activity in activities)
                {
                    int participated = 0;

                    // Check if the activity exists in the lookup
                    if (generalActivitiesByActivityId.TryGetValue(activity.Id, out var generalActivityIds))
                    {
                        // Check if the beneficiary participated in any of the activities
                        if (participantLookup.TryGetValue(beneficiary.ID, out var participantList))
                        {
                            participated = participantList.Count(p => generalActivityIds.Contains(p.GeneralActivityId));
                        }
                    }

                    // Apply counting method
                    if (countmethod == 1)
                    {
                        beneficiaryData[activity.NameInShort] = participated;
                        participantcount += participated;
                    }
                    else
                    {
                        beneficiaryData[activity.NameInShort] = participated > 0 ? 1 : 0;
                        if (participated > 0) participantcount++;
                    }
                }

                beneficiaryData["ParticipationCount"] = participantcount;
                result.Add(beneficiaryData);
            }

            return Json(result);
        }


        [HttpGet("vbus/{countmethod}")]
        public IActionResult GetVBUSActivities(int countmethod)
        {
            // Fetch all required data in a single query each
            var beneficiaries = _context.VBU.ToList();
            var activities = _context.Activities.ToList();
            var generalActivities = _context.GeneralActivities.ToList();
            var participants = _context.GeneralActivityParticipants.ToList();

            // Create a lookup dictionary for GeneralActivities
            var generalActivitiesByActivityId = generalActivities
                .GroupBy(a => a.ActivityId)
                .ToDictionary(g => g.Key, g => g.Select(a => a.Id).ToHashSet());

            // Create a lookup dictionary for Participants
            var participantLookup = participants
                .Where(p => p.ParticipantTypeId == 6)
                .GroupBy(p => p.ParticipantId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<Dictionary<string, object>>();

            foreach (var beneficiary in beneficiaries)
            {
                var beneficiaryData = new Dictionary<string, object>
        {
            { "Beneficiary", $"{beneficiary.Name} {beneficiary.Surname}" },
            { "District", beneficiary.District },
            { "Ward", beneficiary.Ward },
            { "Contact", beneficiary.ContactNumber },
            { "HouseHoldIdentifier", beneficiary.HhdIdentifierNo }
        };

                int participantcount = 0;

                foreach (var activity in activities)
                {
                    int participated = 0;

                    // Check if activity exists in the lookup
                    if (generalActivitiesByActivityId.TryGetValue(activity.Id, out var generalActivityIds))
                    {
                        // Check if the beneficiary participated in any of the activities
                        if (participantLookup.TryGetValue(beneficiary.Id, out var participantList))
                        {
                            participated = participantList.Count(p => generalActivityIds.Contains(p.GeneralActivityId));
                        }
                    }

                    // Apply counting method
                    if (countmethod == 1)
                    {
                        beneficiaryData[activity.NameInShort] = participated;
                        participantcount += participated;
                    }
                    else
                    {
                        beneficiaryData[activity.NameInShort] = participated > 0 ? 1 : 0;
                        if (participated > 0) participantcount++;
                    }
                }

                beneficiaryData["ParticipationCount"] = participantcount;
                result.Add(beneficiaryData);
            }

            return Json(result);
        }



        [HttpGet("groups/{countmethod}")]
        public IActionResult GetGroupActivities(int countmethod)
        {
            var groups = _context.Beneficiaryv3.ToList();
            var activities = _context.Activities.ToList();
            var participants = _context.GeneralActivityParticipants.ToList();

            var result = new List<Dictionary<string, object>>();

            foreach (var group in groups)
            {
                var groupData = new Dictionary<string, object>
        {
            { "Group", group.APGGroup },
            { "Value Chain", group.ValueChain },
            { "District", group.District }
          
        };
                var participantcount = 0;
                foreach (var activity in activities)
                {
                    var ga = _context.GeneralActivities
                        .Where(a => a.ActivityId == activity.Id)
                        .Select(a => a.Id)
                        .ToArray();

                    var participated = participants.Count(p =>
                        ga.Contains(p.GeneralActivityId) && p.ParticipantId == group.ID && p.ParticipantTypeId == 1);

                    if (countmethod == 1)
                    {
                        groupData[activity.NameInShort] = participated;
                        participantcount += participated;
                    }
                    else
                    {
                        groupData[activity.NameInShort] = participated > 0 ? 1 : 0;
                        if (participated > 0)
                        {

                            participantcount++;
                        }
                    }
                }

                groupData["ParticipationCount"] = participantcount;
                result.Add(groupData);
            }

            return Json(result);
        }

        [HttpGet("msmes/{countmethod}")]
        public IActionResult GetMSMEActivities(int countmethod)
        {
            var msmes = _context.MSMEs.ToList();
            var activities = _context.Activities.ToList();
            var participants = _context.GeneralActivityParticipants.ToList();

            var result = new List<Dictionary<string, object>>();

            foreach (var msme in msmes)
            {
                var msmeData = new Dictionary<string, object>
        {
            { "MSME", msme.tradingname },
            { "District", msme.district },
            { "Ward", msme.ward },
            { "Main Value Chain", msme.valuechain1},
        };
                var participantcount = 0;
                foreach (var activity in activities)
                {
                    var ma = _context.GeneralActivities
                        .Where(a => a.ActivityId == activity.Id)
                        .Select(a => a.Id)
                        .ToArray();

                    var participated = participants.Count(p =>
                        ma.Contains(p.GeneralActivityId) && p.ParticipantId == msme.Id && p.ParticipantTypeId == 1);

                    if (countmethod == 1)
                    {
                        msmeData[activity.NameInShort] = participated;
                        participantcount += participated;
                    }
                    else
                    {
                        msmeData[activity.NameInShort] = participated > 0 ? 1 : 0;
                        if (participated > 0)
                        {

                            participantcount++;
                        }
                    }
                }

                msmeData["ParticipationCount"] = participantcount;
                result.Add(msmeData);
            }

            return Json(result);
        }

        [HttpGet("staffmembers/{countmethod}")]
        public IActionResult GetStaffMemberActivities(int countmethod)
        {
            var staffMembers = _context.StaffMembers.ToList();
            var activities = _context.Activities.ToList();
            var participants = _context.GeneralActivityParticipants.ToList();

            var result = new List<Dictionary<string, object>>();

            foreach (var staffMember in staffMembers)
            {
                var staffMemberData = new Dictionary<string, object>
        {
            { "Staff Member", staffMember.StaffFullName }
        };
                var participantcount = 0;
                foreach (var activity in activities)
                {
                    var sa = _context.GeneralActivities
                        .Where(a => a.ActivityId == activity.Id)
                        .Select(a => a.Id)
                        .ToArray();

                    var participated = participants.Count(p =>
                        sa.Contains(p.GeneralActivityId) && p.ParticipantId == staffMember.Id && p.ParticipantTypeId == 1);

                    if (countmethod == 1)
                    {
                        staffMemberData[activity.NameInShort] = participated;
                        participantcount += participated;
                    }
                    else
                    {
                        staffMemberData[activity.NameInShort] = participated > 0 ? 1 : 0;
                        if (participated > 0)
                        {

                            participantcount++;
                        }
                    }
                }

                staffMemberData["ParticipationCount"] = participantcount;
                result.Add(staffMemberData);
            }

            return Json(result);
        }


        [HttpGet("organisations/{countmethod}")]
        public IActionResult GetOrganisationActivities(int countmethod)
        {
            var organisations = _context.Organisations.ToList();
            var activities = _context.Activities.ToList();
            var participants = _context.GeneralActivityParticipants.ToList();

            var result = new List<Dictionary<string, object>>();

            foreach (var organisation in organisations)
            {
                var organisationData = new Dictionary<string, object>
        {
            { "Organisation", organisation.Name }
        };
                var participantcount = 0;
                foreach (var activity in activities)
                {
                    var oa = _context.GeneralActivities
                        .Where(a => a.ActivityId == activity.Id)
                        .Select(a => a.Id)
                        .ToArray();

                    var participated = participants.Count(p =>
                        oa.Contains(p.GeneralActivityId) && p.ParticipantId == organisation.Id && p.ParticipantTypeId == 1);

                    if (countmethod == 1)
                    {
                        organisationData[activity.NameInShort] = participated;
                        participantcount += participated;
                    }
                    else
                    {
                        organisationData[activity.NameInShort] = participated > 0 ? 1 : 0;
                        if (participated > 0)
                        {

                        participantcount++;
                        }
                    }
                }

                organisationData["ParticipationCount"] = participantcount;
                result.Add(organisationData);
            }

            return Json(result);
        }

    }
}

