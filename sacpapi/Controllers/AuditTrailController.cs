using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Linq;

namespace sacpapi.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AuditTrailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuditTrailController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAuditTrail()
        {
            try
            {
                var auditTrailRecords = _context.AuditTrail.ToList();
                return Ok(auditTrailRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving audit trail records: {ex.Message}");
            }
        }
    }
}
