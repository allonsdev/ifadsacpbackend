using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class StaffMemberController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public StaffMemberController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }
        [HttpGet]
        public IActionResult GetAllStaffMembers()
        {
            var staffMembers = _context.StaffMembers.ToList();
            return Json(staffMembers);
        }

        [HttpGet("detailed")]
        public IActionResult detailed()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM [dbo].[StaffMembersDetailed]";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            string jsonResult = JsonConvert.SerializeObject(dataTable);
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
        [HttpGet("notusers")]
        public List<object> GetStaffMembersNotUsers()
        {
            var staffMembersNotUsers = _context.StaffMembers
                .Where(sm => !_context.Users.Any(u => u.StaffMemberId == sm.Id))
                .Select(sm => new
                {
                    Id = sm.Id,
                    Name = sm.StaffFullName
                })
                .ToList<object>();

            return staffMembersNotUsers;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var item = _context.StaffMembers.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return Json(item);
        }

        [HttpPost]
        public IActionResult CreateStaffMember([FromBody] StaffMember staffMember)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string cleanedNationalIdNumber = RemoveSpecialCharacters(staffMember.NationalIdNumber);

                    if (_context.StaffMembers.Any(sm => sm.NationalIdNumber == cleanedNationalIdNumber))
                    {
                        return Conflict("Staff member with the same NationalIdNumber already exists.");
                    }

                    staffMember.NationalIdNumber = cleanedNationalIdNumber;

                    _context.StaffMembers.Add(staffMember);
                    _context.SaveChanges();

                    return StatusCode(201);
                }
                catch (Exception ex)
                {
                    return BadRequest("Error while saving record!");
                }
            }
            else
            {
                return BadRequest("Error while saving record!");
            }
        }

        [HttpPost("batch")]
        public IActionResult SaveStaffMembers(IEnumerable<StaffMember> staffMembers)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    try
                    {
                        foreach (var staffMember in staffMembers)
                        {
                            string cleanedNationalIdNumber = RemoveSpecialCharacters(staffMember.NationalIdNumber);

                            if (_context.StaffMembers.Any(sm => sm.NationalIdNumber == cleanedNationalIdNumber))
                            {
                                return Conflict("Staff member with the same NationalIdNumber already exists.");
                            }

                            staffMember.NationalIdNumber = cleanedNationalIdNumber;

                            _context.StaffMembers.Add(staffMember);
                        }

                        _context.SaveChanges();

                        return StatusCode(201);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error while saving records!");
                    }
                }
                else
                {
                    return BadRequest("Error while saving records!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        private string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, "[^a-zA-Z0-9]", "");
        }


        [HttpPut("{id}")]
        public IActionResult UpdateStaffMember(int id, [FromBody] StaffMember staffMember)
        {
            var existingStaffMember = _context.StaffMembers.Find(id);
            if (existingStaffMember == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingStaffMember.FirstName = staffMember.FirstName;
                    existingStaffMember.Surname = staffMember.Surname;
                    existingStaffMember.NationalIdNumber = staffMember.NationalIdNumber;
                    existingStaffMember.Sex = staffMember.Sex;
                    existingStaffMember.Address = staffMember.Address;
                    existingStaffMember.DistrictId = staffMember.DistrictId;
                    existingStaffMember.ContactNo = staffMember.ContactNo;
                    existingStaffMember.EmailAddress = staffMember.EmailAddress;
                    existingStaffMember.OrganisationId = staffMember.OrganisationId;
                    existingStaffMember.DepartmentId = staffMember.DepartmentId;
                    existingStaffMember.PositionId = staffMember.PositionId;
                    existingStaffMember.CreatedBy = staffMember.CreatedBy;
                    existingStaffMember.CreatedDate = staffMember.CreatedDate;
                    existingStaffMember.UpdatedBy = staffMember.UpdatedBy;
                    existingStaffMember.UpdatedDate = staffMember.UpdatedDate;

                    _context.SaveChanges();

                    return StatusCode(200);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStaffMember(int id)
        {
            var staffMember = _context.StaffMembers.Find(id);
            if (staffMember == null)
            {
                return NotFound();
            }

            try
            {
                _context.StaffMembers.Remove(staffMember);
                _context.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("users")]
        public IActionResult GetStaffMembersWithUsers()
        {
            try
            {
                var staffMembersWithUsers = _context.StaffMembers
                    .Where(sm => _context.Users.Any(u => u.StaffMemberId == sm.Id))
                    .ToList();

                return Json(staffMembersWithUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving staff members with users: {ex.Message}");
            }
        }

    }
}
