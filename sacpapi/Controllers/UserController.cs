using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using sacpapi.Data.Services;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        private readonly IEmailSender emailSender;
        private readonly IAuditTrail auditTrail;
        public UserController(ApplicationDbContext context, IConfiguration configuration, IEmailSender emailSender, IAuditTrail auditTrail)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
            this.emailSender = emailSender;
            this.auditTrail = auditTrail;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var usersWithStaffMembers = _context.Users
                .Join(
                    _context.StaffMembers,
                    user => user.StaffMemberId,
                    staffMember => staffMember.Id,
                    (user, staffMember) => new
                    {
                        UserId = user.Id,
                        Name = staffMember.StaffFullName,
                        Username = user.Username,
                        Email = staffMember.EmailAddress,
                        IsActivated = user.IsActive
                    })
                .ToList();

            return Json(usersWithStaffMembers);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Json(user);
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var staff = _context.StaffMembers.SingleOrDefault(u => u.Id == user.StaffMemberId);
                    if (staff == null)
                    {
                        return NotFound("StaffMember not found");
                    }

                    var exists = _context.Users.SingleOrDefault(u => u.StaffMemberId == user.StaffMemberId);
                    if (exists != null)
                    {
                        return BadRequest("This staffmember already has an account!");
                    }
                    var Username = GenerateUsername(staff.FirstName, staff.Surname, staff.Id);
                    var newPassword = GenerateRandomPassword();
                    string hashedPassword = PasswordHelper.HashPassword(newPassword);
                    user.Username = Username;
                    user.Password = hashedPassword;
                    user.IsActive = true;

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    string systemURL = "https://www.primasysdb.com/sacp";
                    string email = $@"
                    <html>
                    <body>
                        <br>Dear User<br>
                        <p>Your SACP MIS account has been created. Use the following details to login.</p>
                        New Username: <b>{Username}</b><br>
                        New Password: <b>{newPassword}</b><br>
                        <p><b>NB: You can always change this password in your User Profile.<b></p>
                        <p>Click <a href='{systemURL}'>here</a> to login to the system.</p>
                    </body>
                    </html>";

                    await emailSender.SendEmailAsync(staff.EmailAddress, "SACP MIS Account Details", email);

                    auditTrail.Log("Users", user.Id, "Account Creation", "***", "***", user.Username);

                    return StatusCode(201);
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

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            var existingUser = _context.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingUser.StaffMemberId = user.StaffMemberId;
                    existingUser.Username = user.Username;
                    existingUser.Password = user.Password;
                    existingUser.PasswordExpires = user.PasswordExpires;
                    existingUser.PasswordExpirationDays = user.PasswordExpirationDays;
                    existingUser.CreatedBy = user.CreatedBy;
                    existingUser.UpdatedBy = user.UpdatedBy;
                    existingUser.Deleted = user.Deleted;

                    auditTrail.Log("Users", user.Id, "Account Update", "***", "***", user.Username);

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
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                auditTrail.Log("Users", user.Id, "Account Deletion", "***", "***", user.Username);
                _context.Users.Remove(user);
                _context.SaveChanges();

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignIn singIn)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == singIn.Username);

                if (user == null)
                {
                    return Unauthorized("Account Not Found!");
                }

                if (user.IsActive != null)
                {
                    if (!(bool)user.IsActive)
                    {
                        return Unauthorized("This Account is Deactivated. Contact Admin for more information.");
                    }
                }

                if (user.IsLockedOut != null)
                {
                    if ((bool)user.IsLockedOut)
                    {
                        return Unauthorized("This Account is Locked! Contact Admin for more information.");
                    }
                }

                if (!PasswordHelper.VerifyPassword(singIn.Password, user.Password))
                {
                    user.FailedPasswordAttemptCount += 1;
                    int remaining = 0;
                    remaining = 6 - (int)user.FailedPasswordAttemptCount;
                    if (user.FailedPasswordAttemptCount >= 6)
                    {
                        user.IsLockedOut = true;
                    }
                    _context.SaveChanges();

                    if (remaining == 0)
                    {
                        return Unauthorized("This Account Is Locked Out! Please contact the Admin for a password reset!");
                    }
                    else
                    {
                        return Unauthorized("Incorrect Password! You have " + remaining + " attempts remaining");
                    }
                }

                user.FailedPasswordAttemptCount = 0;
                _context.SaveChanges();

                var staff = _context.StaffMembers.SingleOrDefault(u => u.Id == user.StaffMemberId);
                var staffPosition = "";
                using (var connection = new SqlConnection(connectionString))
                {

                    string sql = $"SELECT Name FROM luStaffPositions WHERE Id = {staff.PositionId}";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        connection.Open(); // Open the connection before executing the command

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Check if there is a row returned
                            {
                                staffPosition = reader["Name"].ToString();
                                connection.Close();

                            }
                        }
                    }
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                string? s = _configuration["JwtSecretKey"];
                var key = Encoding.UTF8.GetBytes(s);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Name, user.Username.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, staff.EmailAddress.ToString()),
                        new Claim(ClaimTypes.GivenName, staff.StaffFullName.ToString()),
                        new Claim(ClaimTypes.Role, staffPosition),
                        new Claim("staffId", user.StaffMemberId.ToString()),

                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                auditTrail.Log("Users", user.Id, "User Login Success", "Logged Out", "Logged In", user.Username);

                return Ok(new { Token = tokenString });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("changepassword/{userId}")]
        public IActionResult ChangePassword(int userId, [FromBody] ChangePasswordModel changePasswordModel)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (!PasswordHelper.VerifyPassword(changePasswordModel.OldPassword, user.Password))
                {
                    return BadRequest("Incorrect old password");
                }

                // Hash the new password
                string hashedPassword = PasswordHelper.HashPassword(changePasswordModel.NewPassword);
                user.Password = hashedPassword;

                _context.SaveChanges();

                auditTrail.Log("Users", user.Id, "Password Change Success", changePasswordModel.OldPassword, changePasswordModel.NewPassword, user.Username);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("resetpassword/{userId}")]
        public async Task<IActionResult> ResetPassword(int userId)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                var newPassword = GenerateRandomPassword();

                string hashedPassword = PasswordHelper.HashPassword(newPassword);
                user.IsLockedOut = false;
                user.FailedPasswordAttemptCount = 0;
                user.Password = hashedPassword;
                _context.SaveChanges();

                var staff = _context.StaffMembers.SingleOrDefault(u => u.Id == user.StaffMemberId);

                string systemURL = "https://www.primasysdb.com/sacp";

                string email = $@"
                <html>
                <body>
                    Dear User<br>
                    <p>Your account has been successfully reset. Use your new password below for your next login.</p>
                    New Password: <b>{newPassword}</b><br>
                    <p><b>NB: You can always change this password in your User Profile.<b></p>
                    <p>Click <a href='{systemURL}'>here</a> to login to the system.</p>
                </body>
                </html>";

                await emailSender.SendEmailAsync(staff.EmailAddress, "SACP MIS Password Reset", email);

                auditTrail.Log("Users", user.Id, "Password Reset Success", "***", hashedPassword, user.Username);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("activate/{userId}")]
        public IActionResult Activate(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            if (user.IsActive != null)
            {
                if ((bool)user.IsActive)
                {
                    return BadRequest("Account is already activated");
                }
            }
            user.IsActive = true;
            _context.SaveChanges();

            auditTrail.Log("Users", user.Id, "Account Activation", "***", "***", user.Username);
            return Ok();
        }

        [HttpGet("deactivate/{userId}")]
        public IActionResult Deactivate(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }

            if (user.IsActive != null && !(bool)user.IsActive)
            {
                return BadRequest("Account is already deactivated");
            }

            user.IsActive = false;
            _context.SaveChanges();

            auditTrail.Log("Users", user.Id, "Account Deactivation", "***", "***", user.Username);

            return Ok();
        }

        private string GenerateUsername(string firstname, string lastname, int id)
        {
            // Remove spaces and take the first letter of the first name
            string sanitizedFirstname = firstname.Replace(" ", "").Substring(0, 1).ToLower();

            // Remove spaces from the last name
            string sanitizedLastname = lastname.Replace(" ", "");

            // Combine the components to create the username
            string username = sanitizedFirstname + sanitizedLastname.ToLower() + id.ToString();

            return username;
        }


        private string GenerateRandomPassword(int length = 12)
        {
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string digitChars = "0123456789";
            const string specialChars = "/?"; // Add more special characters as needed

            var random = new Random();

            // Ensure at least one character from each category
            var passwordChars = new[]
            {
                uppercaseChars[random.Next(uppercaseChars.Length)],
                lowercaseChars[random.Next(lowercaseChars.Length)],
                digitChars[random.Next(digitChars.Length)],
                specialChars[random.Next(specialChars.Length)]
            };

            // Fill the rest of the password with random characters
            passwordChars = passwordChars.Concat(
                Enumerable.Repeat(
                    uppercaseChars + lowercaseChars + digitChars + specialChars,
                    length - passwordChars.Length
                )
                .Select(s => s[random.Next(s.Length)])
            ).ToArray();

            // Shuffle the characters to make the password more random
            passwordChars = passwordChars.OrderBy(_ => random.Next()).ToArray();

            return new string(passwordChars);
        }

        public static class PasswordHelper
        {
            public static string HashPassword(string password)
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }

            public static bool VerifyPassword(string enteredPassword, string hashedPassword)
            {
                return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
            }
        }

    }
}
