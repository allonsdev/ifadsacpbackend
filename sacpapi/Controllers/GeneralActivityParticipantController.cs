using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using sacpapi.Data;
using sacpapi.Data.Services;
using sacpapi.Models;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class GeneralActivityParticipantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GeneralActivityParticipantController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var generalActivityParticipants = _context.GeneralActivityParticipants.ToList();
            return Json(generalActivityParticipants);
        }

        [HttpGet("{generalActivityId}/{participantTypeId}")]
        public IActionResult GetById(int generalActivityId, int participantTypeId)
        {
            var generalActivityParticipants = _context.GeneralActivityParticipants
                .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                .ToList();
            return Json(generalActivityParticipants);
        }

        [HttpGet("ids/{generalActivityId}/{participantTypeId}")]
        public IActionResult GetParticipantIds(int generalActivityId, int participantTypeId)
        {
            var generalActivityParticipants = _context.GeneralActivityParticipants
                .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                .Select(gap => gap.ParticipantId)
                .ToArray();
            return Json(generalActivityParticipants);
        }

        [HttpGet("detailed/{generalActivityId}/{participantTypeId}")]
        public IActionResult GetParticipants(int generalActivityId, int participantTypeId)
        {
            try
            {
                IQueryable<object> generalActivityParticipants;

                switch (participantTypeId)
                {
                    case 1:
                        generalActivityParticipants = _context.GeneralActivityParticipants
                            .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                            .Join(_context.Beneficiaries,
                                gap => gap.ParticipantId,
                                indBen => indBen.Id,
                                (gap, indBen) => new { indBen });
                        break;
                    case 2:
                        generalActivityParticipants = _context.GeneralActivityParticipants
                            .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                            .Join(_context.Groups,
                                gap => gap.ParticipantId,
                                group => group.Id,
                                (gap, group) => new { group });
                        break;
                    case 3:
                        generalActivityParticipants = _context.GeneralActivityParticipants
                            .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                            .Join(_context.MSMEs,
                                gap => gap.ParticipantId,
                                msme => msme.Id,
                                (gap, msme) => new { msme });
                        break;
                    case 4:
                        generalActivityParticipants = _context.GeneralActivityParticipants
                            .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                            .Join(_context.StaffMembers,
                                gap => gap.ParticipantId,
                                indStake => indStake.Id,
                                (gap, indStake) => new { indStake });
                        break;
                    case 5:
                        generalActivityParticipants = _context.GeneralActivityParticipants
                            .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId)
                            .Join(_context.Organisations,
                                gap => gap.ParticipantId,
                                orgStake => orgStake.Id,
                                (gap, orgStake) => new { orgStake });
                        break;
                    default:
                        return BadRequest("Invalid participant type.");
                }

                var result = generalActivityParticipants.ToList();
                return Json(result);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Participation/{participantId}/{participantTypeId}")]
        public IActionResult GetParticipation(int participantId, int participantTypeId)
        {
            var result = _context.GeneralActivityParticipants
                .Where(gap => gap.ParticipantId == participantId && gap.ParticipantTypeId == participantTypeId)
                .Join(
                    _context.GeneralActivities,
                    gap => gap.GeneralActivityId,
                    ga => ga.Id,
                    (gap, ga) => new { GeneralActivityParticipant = gap, GeneralActivity = ga }
                )
                .Join(
                    _context.Activities,
                    joined => joined.GeneralActivity.ActivityId,
                    activity => activity.Id,
                    (joined, activity) => new { joined.GeneralActivityParticipant, joined.GeneralActivity, Activity = activity }
                )
                .Select(joined => new { ActivityName = joined.Activity.Name })
                .ToList();

            return Json(result);
        }

        [HttpPost("{generalActivityId}/{participantTypeId}")]
        public IActionResult Create(int generalActivityId, int participantTypeId, [FromBody] List<int> ids)
        {
            try
            {
                var ParticipantsToRemove = _context.GeneralActivityParticipants
                .Where(gap => gap.GeneralActivityId == generalActivityId && gap.ParticipantTypeId == participantTypeId);

                _context.GeneralActivityParticipants.RemoveRange(ParticipantsToRemove);
                _context.SaveChanges();

                if (ids != null && ids.Count > 0)
                {

                    foreach (var id in ids)
                    {
                        var exists = _context.GeneralActivityParticipants.FirstOrDefault(g => g.GeneralActivityId == id && g.ParticipantTypeId == participantTypeId);

                        if (exists == null)
                        {
                            GeneralActivityParticipant generalActivityParticipant = new GeneralActivityParticipant();
                            generalActivityParticipant.GeneralActivityId = generalActivityId;
                            generalActivityParticipant.ParticipantTypeId = participantTypeId;
                            generalActivityParticipant.ParticipantId = id;

                            _context.GeneralActivityParticipants.Add(generalActivityParticipant);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(201);

                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost("exists")]
        public IActionResult CheckExistingNationalIDs([FromBody] List<string> nationalIDs)
        {
            var id = nationalIDs;
            // Filter existing national IDs
            var existingNationalIDs = _context.Beneficiaries
                .Where(b => nationalIDs.Contains(b.IDNumber))
                .Select(b => b.IDNumber)
                .ToArray();

            return Json(existingNationalIDs);
        }

        [HttpPost("stakeholdertemplate/save-data/{generalActivityId}")]
        public async Task<IActionResult> SaveStakeholderData(
    [FromBody] List<StakeholderParticipants> participants,
    [FromRoute] string generalActivityId)
        {
            if (participants == null || participants.Count == 0)
                return BadRequest("No participant data provided");

            var createdList = new List<object>();
            var updatedList = new List<object>();
            var failedList = new List<object>();

            foreach (var (p, index) in participants.Select((p, i) => (p, i)))
            {
                try
                {
                    // Force GTID + uploader from route
                    p.Gtid = generalActivityId;
                    
                    p.UploadedDate = DateTime.Now;

                    if (string.IsNullOrWhiteSpace(p.NameOfParticipant) ||
                        string.IsNullOrWhiteSpace(p.Organisation) ||
                        string.IsNullOrWhiteSpace(p.ContactNumber) ||
                        string.IsNullOrWhiteSpace(p.EmailAddress))
                    {
                        failedList.Add(new { Row = index + 2, Reason = "Missing required fields" });
                        continue;
                    }

                    var existing = await _context.StakeholderParticipants.FirstOrDefaultAsync(x =>
                        x.Gtid == p.Gtid &&
                        x.Organisation == p.Organisation &&
                        x.NameOfParticipant == p.NameOfParticipant &&
                        x.EmailAddress == p.EmailAddress &&
                        x.ContactNumber == p.ContactNumber
                    );

                    if (existing != null)
                    {
                        existing.Sex = p.Sex;
                        existing.Position = p.Position;
                       
                        existing.UploadedDate = DateTime.Now;

                        updatedList.Add(new
                        {
                            Action = "Updated Duplicate",
                            ParticipantId = existing.Id,
                            Name = existing.NameOfParticipant
                        });

                        continue;
                    }

                    _context.StakeholderParticipants.Add(p);
                    await _context.SaveChangesAsync();

                    createdList.Add(new
                    {
                        Action = "Created",
                        ParticipantId = p.Id,
                        Name = p.NameOfParticipant
                    });
                }
                catch (Exception ex)
                {
                    failedList.Add(new { Row = index + 2, Reason = ex.Message });
                }
            }

            return Ok(new
            {
                Created = createdList,
                UpdatedDuplicates = updatedList,
                Failed = failedList
            });
        }


        [HttpPost("stakeholdertemplate/upload-file/{generalActivityId}")]
        public async Task<IActionResult> UploadStakeholderExcel(
            [FromForm] IFormFile file,
            [FromRoute] string generalActivityId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid Excel file");

            var uploadsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads",
                "StakeholderExcel",
                generalActivityId);

            Directory.CreateDirectory(uploadsPath);

            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{file.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                Message = "File uploaded successfully",
                FileName = fileName
            });
        }




        [HttpPost("savetemplatedata/{generalActivityId}")]
        public IActionResult SaveParticipation(int generalActivityId, [FromBody] List<ParticipantRequest> participants)
        {
            try
            {
                List<string> failedBeneficiaryIds = new List<string>();
                int successfulCount = 0;
                int failedCount = 0;
                var part = participants;

                foreach (var participant in participants)
                {
                    // Check if beneficiary already exists
                    var existingBeneficiary = _context.Beneficiaries.FirstOrDefault(b => b.IDNumber == participant.IDNumber);
                    var beneficiaryId = 0;

                    if (existingBeneficiary == null)
                    {
                        // If beneficiary doesn't exist, create a new record
                        var newBeneficiary = new Beneficiary
                        {
                            FarmerName = participant.FarmerName,
                            Gender = participant.Gender,
                            IDNumber = participant.IDNumber,
                            DateOfBirth = participant.DateOfBirth,
                            Disabled = participant.Disabled,
                            ContactNumber = participant.ContactNumber,
                            Disability = participant.Disability,
                            Province = participant.Province,
                            District = participant.District,
                            Ward = participant.Ward,
                            Village = participant.Village,
                            HouseholdHeadSex = participant.HouseholdHeadSex,
                            MaritalStatus = participant.MaritalStatus,
                            RelationshipToHead = participant.RelationshipToHead,
                            ExtensionOfficerName = participant.ExtensionOfficerName,
                            SubmissionTime = DateTime.Now,
                            Status = "submitted_via_web_template"
                        };

                        // Save new beneficiary
                        _context.Beneficiaries.Add(newBeneficiary);
                        _context.SaveChanges();

                        // Now, use the ID of the newly created beneficiary
                        beneficiaryId = newBeneficiary.Id;

                    }
                    else
                    {
                        // If beneficiary already exists, use existing ID
                        beneficiaryId = existingBeneficiary.Id;
                    }

                    // Save participation using BeneficiaryId


                    try
                    {
                        var existingParticipation = _context.GeneralActivityParticipants
                            .FirstOrDefault(p => p.GeneralActivityId == generalActivityId &&
                                                 p.ParticipantTypeId == 1 &&
                                                 p.ParticipantId == beneficiaryId);

                        if (existingParticipation == null)
                        {
                            // Create and add new participation record
                            var participation = new GeneralActivityParticipant
                            {
                                GeneralActivityId = generalActivityId,
                                ParticipantTypeId = 1,
                                ParticipantId = beneficiaryId
                            };

                            _context.GeneralActivityParticipants.Add(participation);
                            _context.SaveChanges();
                        }

                        successfulCount++;
                    }
                    catch (Exception ex)
                    {
                        failedBeneficiaryIds.Add(participant.IDNumber);
                        failedCount++;
                    }
                }

                var result = new
                {
                    FailedBeneficiaryIds = failedBeneficiaryIds,
                    SuccessfulCount = successfulCount,
                    FailedCount = failedCount
                };

                return Json(result); // Return the result object as JSON
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


      
        // POST: api/participants/create
        [HttpPost("CreateParticipants")]
        public async Task<IActionResult> UpsertParticipant([FromBody] Participants participant)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(participant.HhNationalId) ||
                string.IsNullOrWhiteSpace(participant.ParticipantNationalId) ||
                participant.Gtid == null)
            {
                return BadRequest("HhNationalId, Gtid, and ParticipantNationalId are required.");
            }

            try
            {
                // Check if participant exists
                var existingParticipant = await _context.Participants.FirstOrDefaultAsync(p =>
                    p.HhNationalId == participant.HhNationalId &&
                    p.Gtid == participant.Gtid &&
                    p.ParticipantNationalId == participant.ParticipantNationalId);

                if (existingParticipant != null)
                {
                    // ✅ Update the existing participant's details
                    existingParticipant.FirstName = participant.FirstName;
                    existingParticipant.Surname = participant.Surname;
                    existingParticipant.Gender = participant.Gender;
                    existingParticipant.YearOfBirth = participant.YearOfBirth;
                    existingParticipant.GroupType = participant.GroupType;
                    existingParticipant.CategoryName = participant.CategoryName;
                    existingParticipant.PwdTin = participant.PwdTin;
                    existingParticipant.HhGender = participant.HhGender;
                    existingParticipant.ContactNumber = participant.ContactNumber;

                    _context.Participants.Update(existingParticipant);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Participant updated successfully", participant = existingParticipant });
                }

                // ✅ Create new participant
                _context.Participants.Add(participant);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Participant created successfully", participant });
            }
            catch (DbUpdateException dbEx)
            {
                // Log specific DB update error
                // _logger.LogError(dbEx, "Database update error while upserting participant.");
                return StatusCode(500, "A database error occurred while processing your request.");
            }
            catch (Exception ex)
            {
                // Log general error
                // _logger.LogError(ex, "An unexpected error occurred while upserting participant.");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

        [HttpPost("UploadParticipantsTemplate/{id}")]
        public async Task<IActionResult> UploadParticipantsTemplate(IFormFile file, int id)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedTemplates");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            var results = new List<object>();
            int successCount = 0;
            int failedCount = 0;

            try
            {
                // Save the uploaded file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var participants = new List<Participants>();

                ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name
                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("Excel file does not contain any worksheet.");

                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            string hhNationalId = worksheet.Cells[row, 1].Text?.Trim();
                            string firstName = worksheet.Cells[row, 2].Text?.Trim();
                            string surname = worksheet.Cells[row, 3].Text?.Trim();
                            string gender = worksheet.Cells[row, 4].Text?.Trim();
                            string yearOfBirthText = worksheet.Cells[row, 5].Text?.Trim();
                            string groupType = worksheet.Cells[row, 6].Text?.Trim();
                            string categoryName = worksheet.Cells[row, 7].Text?.Trim();
                            string participantNationalId = worksheet.Cells[row, 8].Text?.Trim();
                            string pwdTin = worksheet.Cells[row, 9].Text?.Trim();
                            string hhGender = worksheet.Cells[row, 10].Text?.Trim();
                            string contactNumber = worksheet.Cells[row, 11].Text?.Trim();

                            // ----------------------------
                            // REQUIRED FIELDS CHECK
                            // ----------------------------
                            if (string.IsNullOrWhiteSpace(hhNationalId) ||
                                string.IsNullOrWhiteSpace(participantNationalId) ||
                                string.IsNullOrWhiteSpace(firstName) ||
                                string.IsNullOrWhiteSpace(surname))
                            {
                                failedCount++;
                                results.Add(new { Row = row, Status = "Failed", Reason = "Missing required fields" });
                                continue;
                            }

                            // ----------------------------
                            // GENDER VALIDATION
                            // ----------------------------
                            if (!string.IsNullOrWhiteSpace(gender))
                            {
                                gender = gender.ToUpper();
                                if (gender == "M") gender = "Male";
                                if (gender == "F") gender = "Female";
                                if (gender == "MALEALE") gender = "Male";
                                if (gender == "FEMALES") gender = "Female";

                                if (gender != "Male" && gender != "Female")
                                {
                                    failedCount++;
                                    results.Add(new { Row = row, Status = "Failed", Reason = "Invalid Gender" });
                                    continue;
                                }
                            }

                            // ----------------------------
                            // HOUSEHOLD GENDER VALIDATION
                            // ----------------------------
                            if (!string.IsNullOrWhiteSpace(hhGender))
                            {
                                hhGender = hhGender.ToUpper();
                                if (hhGender == "WHH") hhGender = "WHH";
                                if (hhGender == "MHH") hhGender = "MHH";

                                if (hhGender != "WHH" && hhGender != "MHH")
                                {
                                    failedCount++;
                                    results.Add(new { Row = row, Status = "Failed", Reason = "Invalid Household Gender" });
                                    continue;
                                }
                            }

                            // ----------------------------
                            // CONTACT NUMBER VALIDATION
                            // ----------------------------
                            if (!string.IsNullOrWhiteSpace(contactNumber) &&
                                !contactNumber.All(char.IsDigit))
                            {
                                failedCount++;
                                results.Add(new { Row = row, Status = "Failed", Reason = "Invalid Contact Number" });
                                continue;
                            }

                            // ----------------------------
                            // YEAR OF BIRTH VALIDATION
                            // ----------------------------
                            int yearOfBirth = 0;
                            if (!string.IsNullOrWhiteSpace(yearOfBirthText))
                            {
                                if (!int.TryParse(yearOfBirthText, out yearOfBirth) ||
                                    yearOfBirth < 1900 || yearOfBirth > DateTime.Now.Year)
                                {
                                    failedCount++;
                                    results.Add(new { Row = row, Status = "Failed", Reason = "Invalid YearOfBirth" });
                                    continue;
                                }
                            }

                            // ----------------------------
                            // DUPLICATE CHECK
                            // ----------------------------
                            var existing = await _context.Participants.FirstOrDefaultAsync(p =>
                                p.Gtid == id &&
                                p.HhNationalId == hhNationalId &&
                                p.ParticipantNationalId == participantNationalId);

                            if (existing != null)
                            {
                                // Update existing
                                existing.FirstName = firstName;
                                existing.Surname = surname;
                                existing.Gender = gender;
                                existing.YearOfBirth = yearOfBirth;
                                existing.GroupType = groupType;
                                existing.CategoryName = categoryName;
                                existing.PwdTin = pwdTin;
                                existing.HhGender = hhGender;
                                existing.ContactNumber = contactNumber;

                                _context.Participants.Update(existing);
                            }
                            else
                            {
                                // New participant
                                participants.Add(new Participants
                                {
                                    Gtid = id,
                                    HhNationalId = hhNationalId,
                                    FirstName = firstName,
                                    Surname = surname,
                                    Gender = gender,
                                    YearOfBirth = yearOfBirth,
                                    GroupType = groupType,
                                    CategoryName = categoryName,
                                    ParticipantNationalId = participantNationalId,
                                    PwdTin = pwdTin,
                                    HhGender = hhGender,
                                    ContactNumber = contactNumber
                                });
                            }

                            successCount++;
                            results.Add(new { Row = row, Status = "Success" });
                        }
                        catch (Exception rowEx)
                        {
                            failedCount++;
                            results.Add(new { Row = row, Status = "Failed", Reason = rowEx.Message });
                        }
                    }
                }

                // ----------------------------
                // ADD NEW PARTICIPANTS
                // ----------------------------
                if (participants.Any())
                    _context.Participants.AddRange(participants);

                // ----------------------------
                // SAVE TEMPLATE UPLOAD RECORD
                // ----------------------------
                _context.ParticipantTemplateUploads.Add(new ParticipantTemplateUploads
                {
                    FileName = fileName,
                    FilePath = filePath,
                    UploadedBy = User.Identity?.Name ?? "Anonymous",
                    RecordCount = participants.Count
                });

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Import completed.",
                    totalSuccess = successCount,
                    totalFailed = failedCount,
                    results
                });
            }
            catch (DbUpdateException dbEx)
            {
                var message = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500, $"Database validation failed: {message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error occurred: {ex.Message}");
            }
        }
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile([FromForm] Template model)
        {
            if (model == null || model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            // Define the relative path where you want to save the uploaded files
            var relativePath = "UploadedTemplates"; // Adjust as needed

            var absolutePath = AppDomain.CurrentDomain.BaseDirectory;
            var parentDirectory = Directory.GetParent(absolutePath)?.FullName;

            if (string.IsNullOrEmpty(parentDirectory))
                return BadRequest();

            var uploadsDirectory = Path.Combine(parentDirectory, relativePath);

            if (!Directory.Exists(uploadsDirectory))
                Directory.CreateDirectory(uploadsDirectory);
            // Save the file to the upload directory
            var uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + model.File.FileName;
            var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(stream);
                }

                return StatusCode(200);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
