using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using sacpapi.Data;
using sacpapi.Models;
using System.Collections.Generic;
using System.Linq;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class WaterPointController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public WaterPointController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var waterPoints = _context.WaterPoints.ToList();
            return Ok(waterPoints);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var waterPoint = _context.WaterPoints.Find(id);

            if (waterPoint == null)
            {
                return NotFound();
            }

            return Ok(waterPoint);
        }

        [HttpPost]
        public IActionResult Create([FromBody] WaterPoint waterPoint)
        {
            try
            {
                if (waterPoint == null)
                {
                    return BadRequest();
                }

                // Check for duplicate company name
                if (_context.WaterPoints.Any(wp => wp.Name == waterPoint.Name))
                {
                    // You may choose to return a specific response for duplicates
                    return Conflict("Duplicate WaterPoint Name. Please use a different name.");
                }

                _context.WaterPoints.Add(waterPoint);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new { id = waterPoint.Id }, waterPoint);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while processing the request!");
            }
        }



        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] WaterPoint updatedWaterPoint)
        {
            if (updatedWaterPoint == null || id != updatedWaterPoint.Id)
            {
                return BadRequest();
            }

            var existingWaterPoint = _context.WaterPoints.Find(id);

            if (existingWaterPoint == null)
            {
                return NotFound();
            }

            existingWaterPoint.Name = updatedWaterPoint.Name;
            existingWaterPoint.Province = updatedWaterPoint.Province;
            existingWaterPoint.District = updatedWaterPoint.District;
            existingWaterPoint.Ward = updatedWaterPoint.Ward;
            existingWaterPoint.Village = updatedWaterPoint.Village;
            existingWaterPoint.Latitude = updatedWaterPoint.Latitude;
            existingWaterPoint.Longitude = updatedWaterPoint.Longitude;
            existingWaterPoint.NumberOfHouseholds = updatedWaterPoint.NumberOfHouseholds;
            existingWaterPoint.NumberOfIndividuals = updatedWaterPoint.NumberOfIndividuals;
            existingWaterPoint.Men = updatedWaterPoint.Men;
            existingWaterPoint.Women = updatedWaterPoint.Women;
            existingWaterPoint.Youth = updatedWaterPoint.Youth;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var waterPoint = _context.WaterPoints.Find(id);

            if (waterPoint == null)
            {
                return NotFound();
            }

            _context.WaterPoints.Remove(waterPoint);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
