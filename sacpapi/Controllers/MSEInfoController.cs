using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data; // Change this if your DbContext namespace is different
using sacpapi.Models; // Change this to the correct namespace where MSEInfo model is
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sacpapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MSEInfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MSEInfoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MSEInfo
        [HttpGet("GetMSE")]
        public async Task<ActionResult<IEnumerable<MSEInfo>>> GetMSEInfos()
        {
            return await _context.MSEInfos.ToListAsync();
        }

        // GET: api/MSEInfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MSEInfo>> GetMSEInfo(int id)
        {
            var mseInfo = await _context.MSEInfos.FindAsync(id);

            if (mseInfo == null)
            {
                return NotFound();
            }

            return mseInfo;
        }

        // POST: api/MSEInfo
        [HttpPost]
        public async Task<ActionResult<MSEInfo>> PostMSEInfo(MSEInfo mseInfo)
        {
            _context.MSEInfos.Add(mseInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMSEInfo), new { id = mseInfo.Id }, mseInfo);
        }

        // PUT: api/MSEInfo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMSEInfo(int id, MSEInfo mseInfo)
        {
            if (id != mseInfo.Id)
            {
                return BadRequest();
            }

            _context.Entry(mseInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MSEInfoExists(id))
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

        // DELETE: api/MSEInfo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMSEInfo(int id)
        {
            var mseInfo = await _context.MSEInfos.FindAsync(id);
            if (mseInfo == null)
            {
                return NotFound();
            }

            _context.MSEInfos.Remove(mseInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MSEInfoExists(int id)
        {
            return _context.MSEInfos.Any(e => e.Id == id);
        }
    }
}
