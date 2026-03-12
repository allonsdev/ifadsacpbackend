using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data;
using sacpapi.Models; // Assuming the DbContext is in this namespace

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProjectSiteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectSiteController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSite>>> GetAllProjectSites()
        {
            return await _context.ProjectSites.ToListAsync();
        }

        [HttpGet("mixed")]
        public IEnumerable<object> GetAllMixedEntries()
        {
            var mixedEntries = new List<object>();

            // Get all ProjectSites
            var projectSites = _context.ProjectSites
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.SiteType,
                    p.District,
                    p.Longitude,
                    p.Latitude,
                    NumberOfIndividuals = 0, // No direct count available for Project Sites
                    Men = 0, // No direct count available for Project Sites
                    Women = 0, // No direct count available for Project Sites
                    Youth = 0 // No direct count available for Project Sites
                })
                .ToList();

            // Get all WaterPoints
            var waterPoints = _context.WaterPoints
                .Select(w => new
                {
                    w.Id,
                    w.Name,
                    SiteType = "Water Point",
                    w.District,
                    w.Longitude,
                    w.Latitude,
                    w.NumberOfIndividuals,
                    w.Men,
                    w.Women,
                    w.Youth
                })
                .ToList();

            // Get all IrrigationSchemes
            var irrigationSchemes = _context.IrrigationSchemes
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    SiteType = "Irrigation Scheme",
                    i.District,
                    i.Longitude,
                    i.Latitude,
                    i.NumberOfIndividuals,
                    i.Men,
                    i.Women,
                    i.Youth
                })
                .ToList();

            // Add all entries to mixedEntries list
            mixedEntries.AddRange(projectSites);
            mixedEntries.AddRange(waterPoints);
            mixedEntries.AddRange(irrigationSchemes);

            return mixedEntries;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectSite>> GetProjectSiteById(int id)
        {
            var projectSite = await _context.ProjectSites.FindAsync(id);

            if (projectSite == null)
            {
                return NotFound();
            }

            return projectSite;
        }

        [HttpPost]
        public async Task<ActionResult<ProjectSite>> CreateProjectSite(ProjectSite projectSite)
        {
            _context.ProjectSites.Add(projectSite);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectSiteById), new { id = projectSite.Id }, projectSite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProjectSite(int id, ProjectSite projectSite)
        {
            if (id != projectSite.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectSite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectSiteExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectSite(int id)
        {
            var projectSite = await _context.ProjectSites.FindAsync(id);
            if (projectSite == null)
            {
                return NotFound();
            }

            _context.ProjectSites.Remove(projectSite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectSiteExists(int id)
        {
            return _context.ProjectSites.Any(e => e.Id == id);
        }
    }
}
