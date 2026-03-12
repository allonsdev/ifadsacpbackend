using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender emailSender;

        public FileController(ApplicationDbContext context, IConfiguration configuration, IEmailSender emailSender)
        {
            _context = context;
            _configuration = configuration;
            this.emailSender = emailSender;
        }

        // GET: /File
        [HttpGet("{staffId}")]
        public IActionResult GetAllFiles(int staffId)
        {
            var files = _context.Files.ToList();
            var allowedFiles = new List<object>();

            // Check file permissions for each file where ApplySecurity is true
            foreach (var file in files)
            {
                if (file.ApplySecurity && IsStaffPermittedToFile(file.Id, staffId) && file.ApprovalStatus == "Approved") // Check permission only if ApplySecurity is true
                {
                    allowedFiles.Add(new
                    {
                        file.Id,
                        file.FileType,
                        file.Author,
                        file.Title,
                        file.Description,
                        file.Date,
                        file.FileExtension,
                        file.AuthorOrganization,
                        file.ApplySecurity,
                        file.ApprovalStatus,
                        file.ContentType,
                        file.CreatedDate
                    });
                }
                else if (!file.ApplySecurity && file.ApprovalStatus == "Approved") // Files with ApplySecurity as false are automatically allowed
                {
                    allowedFiles.Add(new
                    {
                        file.Id,
                        file.FileType,
                        file.Author,
                        file.Title,
                        file.Description,
                        file.Date,
                        file.FileExtension,
                        file.AuthorOrganization,
                        file.ApplySecurity,
                        file.ApprovalStatus,
                        file.ContentType,
                        file.CreatedDate
                    });
                }
            }

            return Ok(allowedFiles);
        }

        // GET: /File/UserCreated/{userId}
        [HttpGet("UserCreated/{userId}")]
        public IActionResult GetFilesCreatedByUser(string userId)
        {
            var userFiles = _context.Files
                                    .Where(f => f.CreatedBy == userId)
                                    .ToList();

            if (userFiles == null || !userFiles.Any())
            {
                return NotFound("No files found created by this user.");
            }

            var result = userFiles.Select(file => new
            {
                file.Id,
                file.FileType,
                file.Author,
                file.Title,
                file.Description,
                file.Date,
                file.FileExtension,
                file.AuthorOrganization,
                file.ApplySecurity,
                file.ApprovalStatus,
                file.ContentType,
                file.CreatedDate
            }).ToList();

            return Ok(result);
        }

        // GET: /File/{id}
        [HttpGet("{id}/{staffId}")]
        public ActionResult<Files> GetFileById(int id, int staffId)
        {
            var file = _context.Files.Find(id);

            if (file == null)
            {
                return NotFound();
            }

            // Check if ApplySecurity is true and staff is permitted to access the file
            if (file.ApplySecurity && !IsStaffPermittedToFile(id, staffId))
            {
                return Forbid(); // Or return NotFound() if you don't want to disclose file existence
            }

            return file;
        }

        private bool IsStaffPermittedToFile(int fileId, int staffId)
        {
            // Check if there is a corresponding entry in the FilePermission table
            var filePermission = _context.FilePermissions.FirstOrDefault(fp => fp.FileId == fileId && fp.StaffId == staffId);

            // Return true if a permission entry exists, otherwise false
            return filePermission != null;
        }


        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadModel model)
        {
            if (model == null || model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid file.");
            }

            // Define the relative path where you want to save the uploaded files
            var relativePath = "UploadedFiles"; // Adjust as needed

            var uploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            if (!Directory.Exists(uploadsDirectory))
                Directory.CreateDirectory(uploadsDirectory);

            // Save the file to the upload directory
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
            var filePath = Path.Combine(uploadsDirectory, uniqueFileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Save file details to database
            var file = new Files
            {
                FileType = model.FileType,
                Author = model.Author,
                Title = model.Title,
                Description = model.Description,
                Date = model.Date,
                FilePath = filePath, // Assigning the full file path including directory
                FileExtension = Path.GetExtension(model.File.FileName),
                AuthorOrganization = model.AuthorOrganization,
                ApplySecurity = model.ApplySecurity,
                ApprovalStatus = "Draft",
                ContentType = model.File.ContentType,
                ObjectId = model.ObjectId,
                ObjectTypeId = model.ObjectTypeId,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            // Save file permissions to database
            if (model.FilePermissions != null && model.FilePermissions.Any())
            {
                foreach (var staffId in model.FilePermissions)
                {
                    var filePermission = new FilePermission
                    {
                        FileId = file.Id,
                        StaffId = staffId
                    };

                    _context.FilePermissions.Add(filePermission);

                    var email = _context.StaffMembers
                    .Where(sm => sm.Id == staffId)
                    .Select(sm => sm.EmailAddress)
                    .FirstOrDefault();

                    var staffIds = int.Parse(file.CreatedBy);

                    var usersender = _context.StaffMembers
                            .Where(sm => sm.Id == staffIds)
                            .Select(sm => sm.FirstName + " " + sm.Surname)
                            .FirstOrDefault();
                  

                    await emailSender.SendEmailAsync(email, "SACP: New File Upload", $@"{usersender} has upload a {file.FileType} titled {file.Title}. <p>Click <a href='https://primasysdb.com/sacp/projectview/approvefiles'>here</a> to login to the system.</p> ");
                }
                await _context.SaveChangesAsync();
            }

            return StatusCode(201);
        }

        // PUT: /File/Edit/{fileId}/{userId}
        [HttpPut("{fileId}/{userId}")]
        public IActionResult EditFileDetails(int fileId, string userId, [FromBody] Files updatedFile)
        {
            try
            {
                var file = _context.Files.FirstOrDefault(f => f.Id == fileId && f.CreatedBy == userId);

                if (file == null)
                {
                    return NotFound("File not found or you do not have permission to edit this file.");
                }

                // Update file details
                file.FileType = updatedFile.FileType;
                file.Author = updatedFile.Author;
                file.Title = updatedFile.Title;
                file.Description = updatedFile.Description;
                file.Date = updatedFile.Date;
                file.AuthorOrganization = updatedFile.AuthorOrganization;
                file.ApplySecurity = updatedFile.ApplySecurity;
                file.ApprovalStatus = updatedFile.ApprovalStatus;
                file.ActionedBy = updatedFile.ActionedBy;
                file.ToBeActionedBy = updatedFile.ToBeActionedBy;
                file.ContentType = updatedFile.ContentType;
                file.ObjectId = updatedFile.ObjectId;
                file.ObjectTypeId = updatedFile.ObjectTypeId;
                file.UpdatedBy = userId;  // Set the user as the one who updated the file
                file.UpdatedDate = DateTime.Now;


                _context.Files.Update(file);
                _context.SaveChanges();

                return Ok("File details updated successfully.");
            }
            catch (DbUpdateException dbEx)
            {
                // Handle database update exceptions
                return StatusCode(500, $"Internal server error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /File/Download/{id}
        [HttpGet("Download/{id}")]
        public IActionResult Download(int id)
        {
            var file = _context.Files.Find(id);

            if (file == null)
            {
                return NotFound();
            }

            var filePath = file.FilePath;

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, file.ContentType, Path.GetFileName(filePath));
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, [FromBody] Files updatedFile)
        {
            var file = _context.Files.Find(id);

            if (file == null)
            {
                return NotFound();
            }

            // Update file details
            file.FileType = updatedFile.FileType;
            file.Author = updatedFile.Author;
            file.Title = updatedFile.Title;
            file.Description = updatedFile.Description;
            file.Date = updatedFile.Date;
            file.AuthorOrganization = updatedFile.AuthorOrganization;
            file.ApplySecurity = updatedFile.ApplySecurity;
            file.ApprovalStatus = updatedFile.ApprovalStatus;
            file.ActionedBy = updatedFile.ActionedBy;
            file.ToBeActionedBy = updatedFile.ToBeActionedBy;
            file.ContentType = updatedFile.ContentType;
            file.ObjectId = updatedFile.ObjectId;
            file.ObjectTypeId = updatedFile.ObjectTypeId;
            file.UpdatedBy = updatedFile.UpdatedBy;
            file.UpdatedDate = DateTime.Now;

            _context.Files.Update(file);
            _context.SaveChanges();

            return StatusCode(200);
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            // Find permissions associated with the file
            var permissions = _context.FilePermissions.Where(fp => fp.FileId == id).ToList();

            // Remove permissions
            _context.FilePermissions.RemoveRange(permissions);

            // Find the file
            var file = _context.Files.Find(id);

            if (file == null)
            {
                return NotFound();
            }

            // Remove the file itself
            _context.Files.Remove(file);
            _context.SaveChanges();

            return StatusCode(200);
        }

        [HttpGet("Approved")]
        public ActionResult<IEnumerable<Files>> GetApprovedFiles()
        {
            var approvedFiles = _context.Files.Where(f => f.ApprovalStatus == "Approved").ToList();
            return Ok(approvedFiles);
        }

        // GET: /File/Disapproved
        [HttpGet("Disapproved")]
        public ActionResult<IEnumerable<Files>> GetDisapprovedFiles()
        {
            var disapprovedFiles = _context.Files.Where(f => f.ApprovalStatus == "Disapproved").ToList();
            return Ok(disapprovedFiles);
        }

        // GET: /File/UnActioned
        [HttpGet("Submitted")]
        public ActionResult<IEnumerable<Files>> GetUnActionedFiles()
        {
            var unActionedFiles = _context.Files.Where(f => f.ApprovalStatus == "Submitted").ToList();
            return Ok(unActionedFiles);
        }

        // PUT: /File/Submit/{fileId}
        [HttpPut("Submit/{fileId}/{staffId}")]
        public IActionResult SubmitFile(int fileId, int staffId)
        {
            var file = _context.Files.Find(fileId);

            if (file == null)
            {
                return NotFound("File not found.");
            }

            // Set the ApprovalStatus to "Approved"
            file.ApprovalStatus = "Submitted";
            file.ActionedBy = staffId;

            _context.SaveChanges();

            return StatusCode(200);
        }

        // PUT: /File/Approve/{fileId}
        [HttpPut("Approve/{fileId}/{staffId}")]
        public IActionResult ApproveFile(int fileId, int staffId)
        {
            var file = _context.Files.Find(fileId);

            if (file == null)
            {
                return NotFound("File not found.");
            }

            // Set the ApprovalStatus to "Approved"
            file.ApprovalStatus = "Approved";
            file.ActionedBy = staffId;

            _context.SaveChanges();

            return StatusCode(200);
        }

        // PUT: /File/Disapprove/{fileId}
        [HttpPut("Disapprove/{fileId}/{staffId}")]
        public IActionResult DisapproveFile(int fileId, int staffId)
        {
            var file = _context.Files.Find(fileId);

            if (file == null)
            {
                return NotFound("File not found.");
            }

            // Set the ApprovalStatus to "Disapproved"
            file.ApprovalStatus = "Disapproved";
            file.ActionedBy = staffId;

            _context.SaveChanges();

            return StatusCode(200);
        }


        // POST: /File/AddPermissions/{fileId}
        [HttpPost("AddPermissions/{fileId}")]
        public IActionResult AddFilePermissions(int fileId, [FromBody] List<int> userIds)
        {
            var file = _context.Files.Find(fileId);

            if (file == null)
            {
                return NotFound("File not found.");
            }

            if (userIds == null || userIds.Count == 0)
            {
                return BadRequest("No user IDs provided.");
            }

            // Add file permissions for each user ID
            foreach (var userId in userIds)
            {
                var filePermission = new FilePermission
                {
                    FileId = fileId,
                    StaffId = userId
                };

                _context.FilePermissions.Add(filePermission);
            }

            _context.SaveChanges();

            return StatusCode(200);
        }

        // POST: /File/RemovePermissions/{fileId}
        [HttpPost("RemovePermissions/{fileId}")]
        public IActionResult RemoveFilePermissions(int fileId, [FromBody] List<int> userIds)
        {
            var file = _context.Files.Find(fileId);

            if (file == null)
            {
                return NotFound("File not found.");
            }

            if (userIds == null || userIds.Count == 0)
            {
                return BadRequest("No user IDs provided.");
            }

            // Remove file permissions for each user ID
            foreach (var userId in userIds)
            {
                var filePermission = _context.FilePermissions.FirstOrDefault(fp => fp.FileId == fileId && fp.StaffId == userId);

                if (filePermission != null)
                {
                    _context.FilePermissions.Remove(filePermission);
                }
            }

            _context.SaveChanges();

            return StatusCode(200);
        }

        [HttpGet("GetPermissions/{fileId}")]
        public IActionResult GetFilePermissions(int fileId)
        {
            var file = _context.Files.Find(fileId);

            if (file == null)
            {
                return NotFound("File not found.");
            }

            var permissions = (from fp in _context.FilePermissions
                               join sm in _context.StaffMembers on fp.StaffId equals sm.Id
                               where fp.FileId == fileId
                               select new
                               {
                                   Id = fp.StaffId,
                                   StaffFullName = sm.StaffFullName
                               }).ToList();

            return Ok(permissions);
        }
    }
}
