using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sacpapi.Data;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class vwBeneficiariesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public vwBeneficiariesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/vwBeneficiaries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vwBeneficiaries>>> GetvwBeneficiaries()
        {
            return await _context.vwBeneficiaries.ToListAsync();
        }

        // GET: api/vwBeneficiaries/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<vwBeneficiaries>> GetvwBeneficiary(string id)
        {
            var vwBeneficiary = await _context.vwBeneficiaries.FindAsync(id);

            if (vwBeneficiary == null)
            {
                return NotFound();
            }

            return vwBeneficiary;
        }

        // PUT: api/vwBeneficiaries/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutvwBeneficiary(string id, vwBeneficiaries vwBeneficiary)
        {
            if (id != vwBeneficiary._id)
            {
                return BadRequest();
            }

            _context.Entry(vwBeneficiary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vwBeneficiaryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/vwBeneficiaries
        [HttpPost]
        public async Task<ActionResult<vwBeneficiaries>> PostvwBeneficiary(vwBeneficiaries vwBeneficiary)
        {
            _context.vwBeneficiaries.Add(vwBeneficiary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetvwBeneficiary", new { id = vwBeneficiary._id }, vwBeneficiary);
        }

        // DELETE: api/vwBeneficiaries/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletevwBeneficiary(string id)
        {
            var vwBeneficiary = await _context.vwBeneficiaries.FindAsync(id);
            if (vwBeneficiary == null)
            {
                return NotFound();
            }

            _context.vwBeneficiaries.Remove(vwBeneficiary);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool vwBeneficiaryExists(string id)
        {
            return _context.vwBeneficiaries.Any(e => e._id == id);
        }
    }
}
