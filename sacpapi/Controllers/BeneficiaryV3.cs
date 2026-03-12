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

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
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

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
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
