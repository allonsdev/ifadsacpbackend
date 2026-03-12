using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DisaggregationLabelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DisaggregationLabelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetluDisaggregationLabels()
        {
            var labels = await _context.luDisaggregationLabels.ToListAsync();
            return Ok(labels);
        }

        // GET: api/DisaggregationLabel/5
        [HttpGet("{unit}")]
        public async Task<IActionResult> GetDisaggregationLabel(string unit)
        {
            var labels = await _context.luDisaggregationLabels.Where(lbl => lbl.UnitOfMeasurement == unit).ToListAsync();

            if (labels == null)
            {
                return NotFound();
            }

            return Ok(labels);
        }

        // POST: api/DisaggregationLabel
        [HttpPost]
        public async Task<IActionResult> AddDisaggregationLabel([FromBody] DisaggregationLabel label)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Remove leading and trailing whitespaces from the label name
            label.Name = label.Name.Trim();

            // Check if a label with the same name (case-insensitive and ignoring spaces) already exists
            var existingLabel = await _context.luDisaggregationLabels.FirstOrDefaultAsync(l =>
                string.Equals(l.Name.Replace(" ", ""), label.Name.Replace(" ", ""), StringComparison.OrdinalIgnoreCase));

            if (existingLabel != null)
            {
                ModelState.AddModelError("Name", "A label with the same name already exists.");
                return BadRequest(ModelState);
            }

            _context.luDisaggregationLabels.Add(label);
            await _context.SaveChangesAsync();

            return StatusCode(201);
        }


        // PUT: api/DisaggregationLabel/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDisaggregationLabel(int id, [FromBody] DisaggregationLabel label)
        {
            if (id != label.Id)
            {
                return BadRequest();
            }

            _context.Entry(label).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisaggregationLabelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(200);
        }

        // DELETE: api/DisaggregationLabel/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisaggregationLabel(int id)
        {
            var label = await _context.luDisaggregationLabels.FindAsync(id);
            if (label == null)
            {
                return NotFound();
            }

            _context.luDisaggregationLabels.Remove(label);
            await _context.SaveChangesAsync();

            return StatusCode(200);
        }

        private bool DisaggregationLabelExists(int id)
        {
            return _context.luDisaggregationLabels.Any(e => e.Id == id);
        }
    }
}
