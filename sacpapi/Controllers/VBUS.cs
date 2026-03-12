using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data;

namespace sacpapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VBU : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VBU(ApplicationDbContext context)
        {
            _context = context;
        }

      

        // GET: api/VBU
        [HttpGet("GetVBU")]
        public async Task<ActionResult<IEnumerable<VBU>>> GetVBU()
        {
            var VBU = await _context.VBU.ToListAsync();

            return Ok(VBU);
        }

        // GET: api/VBU/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VBU>> GetVBU(int id)
        {
            var VBU = await _context.VBU.FindAsync(id);

            if (VBU == null)
            {
                return NotFound();
            }

            return Ok(VBU);
        }

       
        // DELETE: api/VBU/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVBU(int id)
        {
            var VBU = await _context.VBU.FindAsync(id);
            if (VBU == null)
            {
                return NotFound();
            }

            _context.VBU.Remove(VBU);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VBUExists(int id)
        {
            return _context.VBU.Any(e => e.Id == id);
        }

    }
}
