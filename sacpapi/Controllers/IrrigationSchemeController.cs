using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sacpapi.Data;
using sacpapi.Models;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class IrrigationSchemeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public IrrigationSchemeController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        // GET: /IrrigationScheme
        [HttpGet]
        public IActionResult GetAll()
        {
            var schemes = _context.IrrigationSchemes.ToList();
            return Ok(schemes);
        }

        // GET: /IrrigationScheme/{id}
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var scheme = _context.IrrigationSchemes.Find(id);

            if (scheme == null)
            {
                return NotFound();
            }

            return Ok(scheme);
        }

        // POST: /IrrigationScheme
        [HttpPost]
        public IActionResult Create([FromBody] IrrigationScheme scheme)
        {
            if (scheme == null)
            {
                return BadRequest("Invalid data");
            }

            _context.IrrigationSchemes.Add(scheme);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = scheme.Id }, scheme);
        }

        // PUT: /IrrigationScheme/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] IrrigationScheme updatedScheme)
        {
            var existingScheme = _context.IrrigationSchemes.Find(id);

            if (existingScheme == null)
            {
                return NotFound();
            }

            existingScheme.Name = updatedScheme.Name;
            existingScheme.Province = updatedScheme.Province;
            existingScheme.AgroEcologicalRegion = updatedScheme.AgroEcologicalRegion;
            existingScheme.District = updatedScheme.District;
            existingScheme.Ward = updatedScheme.Ward;
            existingScheme.SchemeManagementModel = updatedScheme.SchemeManagementModel;
            existingScheme.DateEstablished = updatedScheme.DateEstablished;
            existingScheme.TotalDevelopedAreaToDate = updatedScheme.TotalDevelopedAreaToDate;
            existingScheme.AreaUnderIrrigation = updatedScheme.AreaUnderIrrigation;
            existingScheme.PotentialAreaOfScheme = updatedScheme.PotentialAreaOfScheme;
            existingScheme.IrrigationSchemeStatus = updatedScheme.IrrigationSchemeStatus;
            existingScheme.Longitude = updatedScheme.Longitude;
            existingScheme.Latitude = updatedScheme.Latitude;
            existingScheme.Women = updatedScheme.Women;
            existingScheme.Men = updatedScheme.Men;
            existingScheme.NumberOfIndividuals = updatedScheme.NumberOfIndividuals;

            _context.SaveChanges();

            return Ok(existingScheme);
        }

        // DELETE: /IrrigationScheme/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var scheme = _context.IrrigationSchemes.Find(id);

            if (scheme == null)
            {
                return NotFound();
            }

            _context.IrrigationSchemes.Remove(scheme);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
