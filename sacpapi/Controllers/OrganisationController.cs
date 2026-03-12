using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Data;
using System.Linq;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class OrganisationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public OrganisationController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet]
        public IActionResult GetAllOrganisations()
        {
            var organisations = _context.Organisations.ToList();
            return Json(organisations);
        }
        [HttpGet("detailed")]
        public IActionResult detailed()
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "SELECT * FROM [dbo].[OrganisationDetailed]";

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
        [HttpGet("{id}")]
        public IActionResult GetOrganisationById(int id)
        {
            var organisation = _context.Organisations.Find(id);
            if (organisation == null)
            {
                return NotFound();
            }
            return Json(organisation);
        }

        [HttpPost]
        [HttpPost]
        public IActionResult CreateOrganisation([FromBody] Organisation organisation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_context.Organisations.Any(o => o.Name == organisation.Name))
                    {
                        return Conflict("Organization with the same name already exists.");
                    }

                    _context.Organisations.Add(organisation);
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


        [HttpPut("{id}")]
        public IActionResult UpdateOrganisation(int id, [FromBody] Organisation organisation)
        {
            var existingOrganisation = _context.Organisations.Find(id);
            if (existingOrganisation == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingOrganisation.Name = organisation.Name;
                    existingOrganisation.Type = organisation.Type;
                    existingOrganisation.Description = organisation.Description;
                    existingOrganisation.Address = organisation.Address;
                    existingOrganisation.DistrictId = organisation.DistrictId;
                    existingOrganisation.Latitude = organisation.Latitude;
                    existingOrganisation.Longitude = organisation.Longitude;
                    existingOrganisation.ContactName = organisation.ContactName;
                    existingOrganisation.ContactNo = organisation.ContactNo;
                    existingOrganisation.EmailAddress = organisation.EmailAddress;
                    existingOrganisation.UpdatedBy = organisation.UpdatedBy;
                    existingOrganisation.UpdatedDate = DateTime.Now;

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
        public IActionResult DeleteOrganisation(int id)
        {
            var organisation = _context.Organisations.Find(id);
            if (organisation == null)
            {
                return NotFound();
            }

            try
            {
                _context.Organisations.Remove(organisation);
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
