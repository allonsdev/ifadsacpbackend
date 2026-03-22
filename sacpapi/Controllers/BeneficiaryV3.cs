using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace sacpapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiaryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BeneficiaryController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string? Clean(string v) => string.IsNullOrWhiteSpace(v) ? null : v;

        private int? TryParseInt(string v) => int.TryParse(v, out var i) ? i : null;

        private decimal? TryParseDecimal(string v) => decimal.TryParse(v, out var d) ? d : null;

        #region GET Endpoints


        [HttpGet("GetBeneficiaries")]
        public async Task<ActionResult<IEnumerable<BeneficiaryV3>>> GetvwBeneficiaries()
        {
            var beneficiaries = await _context.Beneficiaryv3
                .Select(b => new BeneficiaryV3
                {
                    ID = b.ID,
                    HouseholdIdentifierNumber = b.HouseholdIdentifierNumber,
                    Province = b.Province,
                    District = b.District,
                    Ward = b.Ward,
                    Village = b.Village,
                    Name = b.Name,
                    Surname = b.Surname,
                    Sex = b.Sex,
                    Age = b.Age,
                    DateOfBirth = b.DateOfBirth,
                    SexOfHousehold = b.SexOfHousehold,
                    ContactNumber = b.ContactNumber,
                    DisabilityStatus = b.DisabilityStatus,
                    YouthStatus = b.YouthStatus,
                    LandSize = b.LandSize,
                    ValueChain = b.ValueChain,
                    APGGroup = b.APGGroup,
                    Chairperson = b.Chairperson,
                    Status = b.Status
                })
                .ToListAsync();

            return beneficiaries;
        }

        [HttpGet("GetDistinctAPGGroups")]
        public async Task<ActionResult<IEnumerable<object>>> GetDistinctAPGGroups()
        {
            var distinctAPGGroups = await _context.Beneficiaryv3
                .GroupBy(b => new { b.ID, b.APGGroup })
                .Select(group => new
                {
                    ID = group.Key.ID,
                    APGGroup = group.Key.APGGroup
                })
                .Distinct()
                .ToListAsync();

            return distinctAPGGroups;
        }
        #endregion

        [HttpPost("VBUExcelUpload")]
        public async Task<IActionResult> UploadExcelVBU(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var vbuList = new List<VBUModel>();
            var failedList = new List<object>();
            int insertedCount = 0;
            int updatedCount = 0;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "WaterUsers");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name to avoid overwrite
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("No worksheet found.");

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Map headers
                    var headers = new Dictionary<int, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(headerValue))
                            headers[col] = headerValue;
                    }

                    // Process rows
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var vbu = new VBUModel();

                        foreach (var kvp in headers)
                        {
                            int col = kvp.Key;
                            string header = kvp.Value;
                            string cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();

                            try
                            {
                                switch (header)
                                {
                                    case "HHD Identifier No": vbu.HhdIdentifierNo = cellValue; break;
                                    case "province": vbu.Province = cellValue; break;
                                    case "district": vbu.District = cellValue; break;
                                    case "Ward": vbu.Ward = double.TryParse(cellValue, out var ward) ? ward : null; break;
                                    case "Village": vbu.Village = cellValue; break;
                                    case "Name": vbu.Name = cellValue; break;
                                    case "Surname": vbu.Surname = cellValue; break;
                                    case "Sex": vbu.Sex = cellValue; break;
                                    case "Age": vbu.Age = double.TryParse(cellValue, out var age) ? age : null; break;
                                    case "Date of Birth": vbu.DateOfBirth = DateTime.TryParse(cellValue, out var dob) ? dob : null; break;
                                    case "ID number": vbu.IdNumber = cellValue; break;
                                    case "Contact number": vbu.ContactNumber = double.TryParse(cellValue, out var contact) ? contact : null; break;
                                    case "Marital status": vbu.MaritalStatus = cellValue; break;
                                    case "Do you have any form of disability?": vbu.Disability = cellValue; break;
                                    case "Sex of household head": vbu.HouseholdHeadSex = cellValue; break;
                                    case "VBU Land Size ": vbu.VbuLandSize = cellValue; break;
                                    case "Area under production": vbu.AreaUnderProduction = cellValue; break;
                                    case "Are you a member of any existing VBU": vbu.IsVbuMember = cellValue; break;
                                    case "what is the name of the VBU?": vbu.VbuName = cellValue; break;
                                    case "Do you have any leadership position in the group? ": vbu.LeadershipPosition = cellValue; break;
                                    case "Specify position": vbu.SpecifyPosition = cellValue; break;
                                }
                            }
                            catch
                            {
                                // Skip parsing error, log later
                            }
                        }

                        if (string.IsNullOrEmpty(vbu.Name) && string.IsNullOrEmpty(vbu.Surname))
                        {
                            failedList.Add(new { Row = row, vbu.Name, vbu.Surname });
                            continue;
                        }

                        try
                        {
                            var existing = _context.VBU.FirstOrDefault(x =>
                                x.HhdIdentifierNo == vbu.HhdIdentifierNo &&
                                x.Province == vbu.Province &&
                                x.District == vbu.District &&
                                x.Ward == vbu.Ward);

                            if (existing != null)
                            {
                                // Update existing record
                                existing.Village = vbu.Village;
                                existing.Name = vbu.Name;
                                existing.Surname = vbu.Surname;
                                existing.Sex = vbu.Sex;
                                existing.Age = vbu.Age;
                                existing.DateOfBirth = vbu.DateOfBirth;
                                existing.IdNumber = vbu.IdNumber;
                                existing.ContactNumber = vbu.ContactNumber;
                                existing.MaritalStatus = vbu.MaritalStatus;
                                existing.Disability = vbu.Disability;
                                existing.HouseholdHeadSex = vbu.HouseholdHeadSex;
                                existing.VbuLandSize = vbu.VbuLandSize;
                                existing.AreaUnderProduction = vbu.AreaUnderProduction;
                                existing.IsVbuMember = vbu.IsVbuMember;
                                existing.VbuName = vbu.VbuName;
                                existing.LeadershipPosition = vbu.LeadershipPosition;
                                existing.SpecifyPosition = vbu.SpecifyPosition;

                                updatedCount++;
                            }
                            else
                            {
                                _context.VBU.Add(vbu);
                                insertedCount++;
                            }

                            vbuList.Add(vbu);
                        }
                        catch
                        {
                            failedList.Add(new
                            {
                                Row = row,
                                vbu.Name,
                                vbu.Surname,
                                vbu.Province,
                                vbu.District,
                                vbu.Ward
                            });
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                foreach (var vbu in vbuList)
                {
                    failedList.Add(new
                    {
                        vbu.Name,
                        vbu.Surname,
                        vbu.Province,
                        vbu.District,
                        vbu.Ward
                    });
                }
            }

            return Ok(new
            {
                message = "Excel processed",
                totalRecords = vbuList.Count + failedList.Count,
                inserted = insertedCount,
                updated = updatedCount,
                failed = failedList.Count,
                failedRecords = failedList
            });
        }

        #region MSE Upload

        [HttpPost("MseExcelUpload")]
        public async Task<IActionResult> UploadExcelMSE(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var mseList = new List<MSEInfo>();
            var failedList = new List<object>();
            int insertedCount = 0;
            int updatedCount = 0;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "WaterUsers");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name to avoid overwrite
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("No worksheet found.");

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // Map headers
                    var headers = new Dictionary<int, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(headerValue))
                            headers[col] = headerValue;
                    }

                    // Process rows
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var mse = new MSEInfo();

                        foreach (var kvp in headers)
                        {
                            int col = kvp.Key;
                            string header = kvp.Value;
                            string cellValue = worksheet.Cells[row, col].Value?.ToString()?.Trim();

                            try
                            {
                                switch (header)
                                {
                                    case "NameOfBusiness": mse.NameOfBusiness = cellValue; break;
                                    case "TradingName": mse.TradingName = cellValue; break;
                                    case "RegistrationStatus": mse.RegistrationStatus = cellValue; break;
                                    case "ContactPerson": mse.ContactPerson = cellValue; break;
                                    case "PhysicalAddress": mse.PhysicalAddress = cellValue; break;
                                    case "YearsOfOperation": mse.YearsOfOperation = cellValue; break;
                                    case "OwnerSex": mse.OwnerSex = cellValue; break;
                                    case "OwnerAge": mse.OwnerAge = cellValue; break;
                                    case "OwnerDOB": mse.OwnerDOB = cellValue; break;
                                    case "ContactNo": mse.ContactNo = cellValue; break;
                                    case "Province": mse.Province = cellValue; break;
                                    case "District": mse.District = cellValue; break;
                                    case "Ward": mse.Ward = cellValue; break;
                                    case "GPS": mse.GPS = cellValue; break;
                                    case "GPSLatitude": mse.GPSLatitude = cellValue; break;
                                    case "GPSLongitude": mse.GPSLongitude = cellValue; break;
                                    case "GPSAltitude": mse.GPSAltitude = cellValue; break;
                                    case "GPSPrecision": mse.GPSPrecision = cellValue; break;
                                    case "NumberOfMales": int.TryParse(cellValue, out var males); mse.NumberOfMales = males; break;
                                    case "NumberOfFemales": int.TryParse(cellValue, out var females); mse.NumberOfFemales = females; break;
                                    case "Total": int.TryParse(cellValue, out var total); mse.Total = total; break;
                                    case "MaleBeneficiaries": int.TryParse(cellValue, out var maleB); mse.MaleBeneficiaries = maleB; break;
                                    case "FemaleBeneficiaries": int.TryParse(cellValue, out var femaleB); mse.FemaleBeneficiaries = femaleB; break;
                                }
                            }
                            catch
                            {
                                // Skip parsing error, log later
                            }
                        }

                        if (string.IsNullOrEmpty(mse.NameOfBusiness) && string.IsNullOrEmpty(mse.TradingName))
                        {
                            failedList.Add(new { Row = row, mse.NameOfBusiness, mse.TradingName });
                            continue;
                        }

                        try
                        {
                            var existing = _context.MSEInfos.FirstOrDefault(x =>
                                x.NameOfBusiness == mse.NameOfBusiness &&
                                x.TradingName == mse.TradingName &&
                                x.Province == mse.Province &&
                                x.Ward == mse.Ward &&
                                x.ContactPerson == mse.ContactPerson);

                            if (existing != null)
                            {
                                existing.RegistrationStatus = mse.RegistrationStatus;
                                existing.PhysicalAddress = mse.PhysicalAddress;
                                existing.YearsOfOperation = mse.YearsOfOperation;
                                existing.OwnerSex = mse.OwnerSex;
                                existing.OwnerAge = mse.OwnerAge;
                                existing.OwnerDOB = mse.OwnerDOB;
                                existing.ContactNo = mse.ContactNo;
                                existing.District = mse.District;
                                existing.GPS = mse.GPS;
                                existing.GPSLatitude = mse.GPSLatitude;
                                existing.GPSLongitude = mse.GPSLongitude;
                                existing.GPSAltitude = mse.GPSAltitude;
                                existing.GPSPrecision = mse.GPSPrecision;
                                existing.NumberOfMales = mse.NumberOfMales;
                                existing.NumberOfFemales = mse.NumberOfFemales;
                                existing.Total = mse.Total;
                                existing.MaleBeneficiaries = mse.MaleBeneficiaries;
                                existing.FemaleBeneficiaries = mse.FemaleBeneficiaries;

                                updatedCount++;
                            }
                            else
                            {
                                _context.MSEInfos.Add(mse);
                                insertedCount++;
                            }

                            mseList.Add(mse);
                        }
                        catch
                        {
                            failedList.Add(new
                            {
                                Row = row,
                                mse.NameOfBusiness,
                                mse.TradingName,
                                mse.Province,
                                mse.Ward,
                                mse.ContactPerson
                            });
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                foreach (var mse in mseList)
                {
                    failedList.Add(new
                    {
                        mse.NameOfBusiness,
                        mse.TradingName,
                        mse.Province,
                        mse.Ward,
                        mse.ContactPerson
                    });
                }
            }

            return Ok(new
            {
                message = "Excel processed",
                totalRecords = mseList.Count + failedList.Count,
                inserted = insertedCount,
                updated = updatedCount,
                failed = failedList.Count,
                failedRecords = failedList
            });
        }

        #endregion


        [HttpPost("WaterUsersExcelUpload")]
        public async Task<IActionResult> UploadExcelWaterUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var list = new List<WaterUser>();
            var failedList = new List<object>();
            int insertedCount = 0;
            int updatedCount = 0;


            // 1️⃣ Save a copy
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "WaterUsers");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name to avoid overwrite
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);


                ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("No worksheet found.");

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    // ✅ Header Mapping
                    var headers = new Dictionary<int, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var header = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(header))
                            headers[col] = header;
                    }

                    // ✅ Process Rows
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var item = new WaterUser();

                        foreach (var kvp in headers)
                        {
                            int col = kvp.Key;
                            string header = kvp.Value;
                            string value = worksheet.Cells[row, col].Value?.ToString()?.Trim();

                            try
                            {
                                switch (header)
                                {
                                    case "Province": item.Province = Clean(value); break;
                                    case "District": item.District = Clean(value); break;
                                    case "Ward": item.Ward = Clean(value); break;

                                    case "NameOfTheWaterPoint": item.NameOfTheWaterPoint = Clean(value); break;

                                    case "FirstNameOfHhh": item.FirstNameOfHhh = Clean(value); break;
                                    case "SurnameOfHhh": item.SurnameOfHhh = Clean(value); break;

                                    case "HouseholdIdentifier": item.HouseholdIdentifier = Clean(value); break;

                                    case "Gender": item.Gender = Clean(value); break;
                                    case "Yob": item.Yob = TryParseInt(value); break;

                                    case "AgeStatus": item.AgeStatus = Clean(value); break;
                                    case "ContactNumber": item.ContactNumber = Clean(value); break;

                                    case "GenderOfHhh": item.GenderOfHhh = Clean(value); break;
                                    case "PwdStatus": item.PwdStatus = Clean(value); break;

                                    case "FamilySize": item.FamilySize = TryParseInt(value); break;
                                    case "Male": item.Male = TryParseInt(value); break;
                                    case "Female": item.Female = TryParseInt(value); break;

                                    case "NumberOfLivestockOwnedAccessingTheWaterPoint":
                                        item.NumberOfLivestockOwnedAccessingTheWaterPoint = TryParseInt(value);
                                        break;
                                }
                            }
                            catch
                            {
                                // ignore parse errors, log later
                            }
                        }

                        // ❌ Skip bad rows
                        if (string.IsNullOrEmpty(item.HouseholdIdentifier))
                        {
                            failedList.Add(new { Row = row, Reason = "Missing HouseholdIdentifier" });
                            continue;
                        }

                        try
                        {
                            // ✅ UPSERT LOGIC (based on your constraint)
                            var existing = _context.WaterUsers.FirstOrDefault(x =>
                                x.HouseholdIdentifier == item.HouseholdIdentifier &&
                                x.NameOfTheWaterPoint == item.NameOfTheWaterPoint
                            );

                            if (existing != null)
                            {
                                // UPDATE
                                existing.Province = item.Province;
                                existing.District = item.District;
                                existing.Ward = item.Ward;
                                existing.FirstNameOfHhh = item.FirstNameOfHhh;
                                existing.SurnameOfHhh = item.SurnameOfHhh;
                                existing.Gender = item.Gender;
                                existing.Yob = item.Yob;
                                existing.AgeStatus = item.AgeStatus;
                                existing.ContactNumber = item.ContactNumber;
                                existing.GenderOfHhh = item.GenderOfHhh;
                                existing.PwdStatus = item.PwdStatus;
                                existing.FamilySize = item.FamilySize;
                                existing.Male = item.Male;
                                existing.Female = item.Female;
                                existing.NumberOfLivestockOwnedAccessingTheWaterPoint =
                                    item.NumberOfLivestockOwnedAccessingTheWaterPoint;

                                updatedCount++;
                            }
                            else
                            {
                                item.CreatedAt = DateTime.Now;
                                _context.WaterUsers.Add(item);
                                insertedCount++;
                            }

                            list.Add(item);
                        }
                        catch
                        {
                            failedList.Add(new
                            {
                                Row = row,
                                item.HouseholdIdentifier,
                                item.NameOfTheWaterPoint
                            });
                        }
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                foreach (var item in list)
                {
                    failedList.Add(new
                    {
                        item.HouseholdIdentifier,
                        item.NameOfTheWaterPoint
                    });
                }
            }

            return Ok(new
            {
                message = "Excel processed",
                totalRecords = list.Count + failedList.Count,
                inserted = insertedCount,
                updated = updatedCount,
                failed = failedList.Count,
                failedRecords = failedList
            });
        }





        [HttpGet("wateruser")]
        public async Task<IActionResult> GetAllWaterUsers()
        {
            var waterUsers = await _context.WaterUsers.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return Ok(waterUsers);
        }

        [HttpGet("sbu")]
        public async Task<IActionResult> GetAllSBU()
        {
            var sbus = await _context.SchoolBusinessUnits.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return Ok(sbus);
        }


        [HttpGet("roaduser")]
        public async Task<IActionResult> GetAllRoadUsers()
        {
            var roadUsers = await _context.RoadUsers.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return Ok(roadUsers);
        }

        [HttpGet("employee")]
        public async Task<IActionResult> GetAllEmploymentRecords()
        {
            var records = await _context.EmploymentRecords.OrderByDescending(x => x.CreatedAt).ToListAsync();
            return Ok(records);
        }

        [HttpPost("RoadUsersExcelUpload")]
        public async Task<IActionResult> UploadExcelRoadUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var list = new List<RoadUser>();
            var failedList = new List<object>();
            int inserted = 0, updated = 0;


            // 1️⃣ Save a copy
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "RoadUser");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name to avoid overwrite
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name

                using (var package = new ExcelPackage(stream))
                {
                    var ws = package.Workbook.Worksheets.FirstOrDefault();
                    if (ws == null) return BadRequest("No worksheet.");

                    int rows = ws.Dimension.Rows;
                    int cols = ws.Dimension.Columns;

                    var headers = new Dictionary<int, string>();
                    for (int c = 1; c <= cols; c++)
                    {
                        var h = ws.Cells[1, c].Text.Trim();
                        if (!string.IsNullOrEmpty(h)) headers[c] = h;
                    }

                    for (int r = 2; r <= rows; r++)
                    {
                        var item = new RoadUser();

                        foreach (var h in headers)
                        {
                            var val = ws.Cells[r, h.Key].Text.Trim();

                            switch (h.Value)
                            {
                                case "Province": item.Province = Clean(val); break;
                                case "District": item.District = Clean(val); break;
                                case "Ward": item.Ward = Clean(val); break;
                                case "FirstName": item.FirstName = Clean(val); break;
                                case "Surname": item.Surname = Clean(val); break;
                                case "NationalIdNumber": item.NationalIdNumber = Clean(val); break;
                                case "GenderOfHhh": item.GenderOfHhh = Clean(val); break;
                                case "Yob": item.Yob = TryParseInt(val); break;
                                case "YouthStatus": item.YouthStatus = Clean(val); break;
                                case "MaritalStatus": item.MaritalStatus = Clean(val); break;
                                case "PwdStatus": item.PwdStatus = Clean(val); break;
                                case "NumberOfHouseholdMembers": item.NumberOfHouseholdMembers = TryParseInt(val); break;
                                case "Male": item.Male = TryParseInt(val); break;
                                case "Female": item.Female = TryParseInt(val); break;
                                case "NameOfPlaceWhereTheRoadWasRehabilitated": item.NameOfPlaceWhereTheRoadWasRehabilitated = Clean(val); break;
                                case "ContactNumber": item.ContactNumber = Clean(val); break;
                            }
                        }

                        if (string.IsNullOrEmpty(item.NationalIdNumber))
                        {
                            failedList.Add(new { Row = r, Reason = "Missing NationalIdNumber" });
                            continue;
                        }

                        var existing = _context.RoadUsers
                            .FirstOrDefault(x => x.NationalIdNumber == item.NationalIdNumber);

                        if (existing != null)
                        {
                            // UPDATE
                            existing.Province = item.Province;
                            existing.District = item.District;
                            existing.Ward = item.Ward;
                            existing.FirstName = item.FirstName;
                            existing.Surname = item.Surname;
                            existing.GenderOfHhh = item.GenderOfHhh;
                            existing.Yob = item.Yob;
                            existing.YouthStatus = item.YouthStatus;
                            existing.MaritalStatus = item.MaritalStatus;
                            existing.PwdStatus = item.PwdStatus;
                            existing.NumberOfHouseholdMembers = item.NumberOfHouseholdMembers;
                            existing.Male = item.Male;
                            existing.Female = item.Female;
                            existing.NameOfPlaceWhereTheRoadWasRehabilitated = item.NameOfPlaceWhereTheRoadWasRehabilitated;
                            existing.ContactNumber = item.ContactNumber;

                            updated++;
                        }
                        else
                        {
                            item.CreatedAt = DateTime.Now;
                            _context.RoadUsers.Add(item);
                            inserted++;
                        }

                        list.Add(item);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { inserted, updated, failed = failedList.Count, failedList });
        }

        [HttpPost("SBUExcelUpload")]
        public async Task<IActionResult> UploadExcelSBU(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var list = new List<SchoolBusinessUnit>();
            var failed = new List<object>();
            int inserted = 0, updated = 0;

            // 1️⃣ Save a copy
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "SBU");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name to avoid overwrite
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var streams = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(streams);
            }
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name
            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.FirstOrDefault();

            int rows = ws.Dimension.Rows;
            int cols = ws.Dimension.Columns;

            var headers = new Dictionary<int, string>();
            for (int c = 1; c <= cols; c++)
                headers[c] = ws.Cells[1, c].Text.Trim();

            for (int r = 2; r <= rows; r++)
            {
                var item = new SchoolBusinessUnit();

                foreach (var h in headers)
                {
                    var val = ws.Cells[r, h.Key].Text.Trim();

                    switch (h.Value)
                    {
                        case "Province": item.Province = Clean(val); break;
                        case "District": item.District = Clean(val); break;
                        case "Ward": item.Ward = Clean(val); break;
                        case "NameOfSchoolBusinessUnit": item.NameOfSchoolBusinessUnit = Clean(val); break;
                        case "NumberOfFemaleStudents": item.NumberOfFemaleStudents = TryParseInt(val); break;
                        case "NumberOfMaleStudents": item.NumberOfMaleStudents = TryParseInt(val); break;
                        case "NameOfSchoolHead": item.NameOfSchoolHead = Clean(val); break;
                        case "ContactNumberOfTheSchoolHead": item.ContactNumberOfTheSchoolHead = Clean(val); break;
                        case "Latitude": item.Latitude = TryParseDecimal(val); break;
                        case "Longitude": item.Longitude = TryParseDecimal(val); break;
                    }
                }

                if (string.IsNullOrEmpty(item.NameOfSchoolBusinessUnit) || string.IsNullOrEmpty(item.District))
                {
                    failed.Add(new { Row = r });
                    continue;
                }

                var existing = _context.SchoolBusinessUnits.FirstOrDefault(x =>
                    x.NameOfSchoolBusinessUnit == item.NameOfSchoolBusinessUnit &&
                    x.District == item.District);

                if (existing != null)
                {
                    existing.Province = item.Province;
                    existing.Ward = item.Ward;
                    existing.NumberOfFemaleStudents = item.NumberOfFemaleStudents;
                    existing.NumberOfMaleStudents = item.NumberOfMaleStudents;
                    existing.NameOfSchoolHead = item.NameOfSchoolHead;
                    existing.ContactNumberOfTheSchoolHead = item.ContactNumberOfTheSchoolHead;
                    existing.Latitude = item.Latitude;
                    existing.Longitude = item.Longitude;

                    updated++;
                }
                else
                {
                    item.CreatedAt = DateTime.Now;
                    _context.SchoolBusinessUnits.Add(item);
                    inserted++;
                }

                list.Add(item);
            }

            await _context.SaveChangesAsync();

            return Ok(new { inserted, updated, failed = failed.Count });
        }

        [HttpPost("EmploymentExcelUpload")]
        public async Task<IActionResult> UploadExcelEmployment(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var list = new List<EmploymentRecord>();
            var failed = new List<object>();
            int inserted = 0, updated = 0;


            // 1️⃣ Save a copy
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Employee");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            // Generate unique file name to avoid overwrite
            var fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var streams = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(streams);
            }
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name

            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.FirstOrDefault();

            int rows = ws.Dimension.Rows;
            int cols = ws.Dimension.Columns;

            var headers = new Dictionary<int, string>();
            for (int c = 1; c <= cols; c++)
                headers[c] = ws.Cells[1, c].Text.Trim();

            for (int r = 2; r <= rows; r++)
            {
                var item = new EmploymentRecord();

                foreach (var h in headers)
                {
                    var val = ws.Cells[r, h.Key].Text.Trim();

                    switch (h.Value)
                    {
                        case "District": item.District = Clean(val); break;
                        case "Gender": item.Gender = Clean(val); break;
                        case "AgeGroup": item.AgeGroup = Clean(val); break;
                        case "DisabilityStatus": item.DisabilityStatus = Clean(val); break;
                        case "PwdMale": item.PwdMale = TryParseInt(val); break;
                        case "PwdFemale": item.PwdFemale = TryParseInt(val); break;
                    }
                }

                if (string.IsNullOrEmpty(item.District) || string.IsNullOrEmpty(item.Gender))
                {
                    failed.Add(new { Row = r });
                    continue;
                }

                var existing = _context.EmploymentRecords.FirstOrDefault(x =>
                    x.District == item.District &&
                    x.Gender == item.Gender &&
                    x.AgeGroup == item.AgeGroup &&
                    x.DisabilityStatus == item.DisabilityStatus);

                if (existing != null)
                {
                    existing.PwdMale = item.PwdMale;
                    existing.PwdFemale = item.PwdFemale;
                    updated++;
                }
                else
                {
                    item.CreatedAt = DateTime.Now;
                    _context.EmploymentRecords.Add(item);
                    inserted++;
                }

                list.Add(item);
            }

            await _context.SaveChangesAsync();

            return Ok(new { inserted, updated, failed = failed.Count });
        }




        #region Beneficiary Upload

        [HttpPost("ExcelUpload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            byte[] fileBytes;

            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var filePath = Path.Combine(uploadFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var fileName = file.FileName;
            var fileType = file.ContentType;
            var fileSize = file.Length;
            var beneficiaries = new List<BeneficiaryV3>();

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelPackage.License.SetNonCommercialPersonal("Tafadzwa Mazani"); // your license name

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        return BadRequest("No worksheet found in the uploaded file.");

                    int rowCount = worksheet.Dimension.Rows;
                    int colCount = worksheet.Dimension.Columns;

                    var headers = new Dictionary<int, string>();
                    for (int col = 1; col <= colCount; col++)
                    {
                        var headerValue = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(headerValue))
                            headers[col] = headerValue;
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var beneficiary = new BeneficiaryV3();

                        for (int col = 1; col <= colCount; col++)
                        {
                            if (!headers.ContainsKey(col)) continue;
                            string header = headers[col];

                            try
                            {
                                switch (header)
                                {
                                    case "HouseholdIdentifierNumber":
                                        beneficiary.HouseholdIdentifierNumber = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Province":
                                        beneficiary.Province = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "District":
                                        beneficiary.District = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Ward":
                                        beneficiary.Ward = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Village":
                                        beneficiary.Village = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Name":
                                        beneficiary.Name = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Surname":
                                        beneficiary.Surname = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Sex":
                                        beneficiary.Sex = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Age":
                                        if (int.TryParse(worksheet.Cells[row, col].Value?.ToString(), out var age))
                                            beneficiary.Age = age;
                                        break;
                                    case "DateOfBirth":
                                        if (DateTime.TryParse(worksheet.Cells[row, col].Value?.ToString(), out var dob))
                                            beneficiary.DateOfBirth = dob;
                                        break;
                                    case "SexOfHousehold":
                                        beneficiary.SexOfHousehold = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "ContactNumber":
                                        beneficiary.ContactNumber = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "DisabilityStatus":
                                        beneficiary.DisabilityStatus = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "YouthStatus":
                                        beneficiary.YouthStatus = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "LandSize":
                                        beneficiary.LandSize = worksheet.Cells[row, col].Value?.ToString();
                                        break;
                                    case "ValueChain":
                                        beneficiary.ValueChain = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "APGGroup":
                                        beneficiary.APGGroup = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Chairperson":
                                        beneficiary.Chairperson = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                    case "Status":
                                        beneficiary.Status = worksheet.Cells[row, col].Value?.ToString()?.Trim();
                                        break;
                                }
                            }
                            catch
                            {
                                // Ignore parsing errors for now
                            }
                        }

                        beneficiaries.Add(beneficiary);
                    }
                }
            }

            _context.Beneficiaryv3.AddRange(beneficiaries);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "File uploaded successfully",
                fileName,
                fileType,
                fileSize,
                recordCount = beneficiaries.Count,
                beneficiaries
            });
        }

        #endregion
    }
}
