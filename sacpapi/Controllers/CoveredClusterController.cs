using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using sacpapi.Data;
using sacpapi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class CoveredClusterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;

        public CoveredClusterController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet]
        public IActionResult GetAllCoveredClusterIds()
        {
            var clusters = _context.luClusters;

            var coveredClustersIds = _context.CoveredClusters
                .Select(cc => cc.ClusterId)
                .ToArray();

            var coveredClusters = clusters
                .Where(item => coveredClustersIds.Contains(item.Id))
                .Select(item => item.Name)
                .ToArray();

            return Ok(coveredClusters);
        }

        [HttpGet("list")]
        public IActionResult GetAllCoveredClusters()
        {
            try
            {
                var coveredClusters = _context.CoveredClusters
                    .Join(_context.luClusters,
                        cc => cc.ClusterId,
                        lc => lc.Id,
                        (cc, lc) => new
                        {
                            cc.Id,
                            cc.ClusterId,
                            lc.Name
                        })
                    .ToList();
                return Json(coveredClusters);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving Coverage: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CoveredCluster>> GetCoveredClusterById(int id)
        {
            var coveredCluster = await _context.CoveredClusters.FindAsync(id);

            if (coveredCluster == null)
            {
                return NotFound();
            }

            return coveredCluster;
        }

        [HttpPost]
        public IActionResult Create([FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        var exists = _context.CoveredClusters.FirstOrDefault(g => g.ClusterId == id);

                        if (exists == null)
                        {
                            CoveredCluster coveredCluster = new CoveredCluster();
                            coveredCluster.ProjectId = 1;
                            coveredCluster.ClusterId = id;

                            _context.CoveredClusters.Add(coveredCluster);
                        }
                    }

                    _context.SaveChanges();

                    return StatusCode(201);
                }

                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCoveredCluster(int id, CoveredCluster coveredCluster)
        {
            if (id != coveredCluster.Id)
            {
                return BadRequest();
            }

            _context.Entry(coveredCluster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoveredClusterExists(id))
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

        [HttpGet("coveredclusters")]
        public IActionResult GetCoveredClustersWithWardCodes()
        {
            try
            {
                // Get all covered clusters
                var coveredClusters = _context.CoveredClusters.ToList();

                // Create a dictionary to store cluster names with their associated ward codes
                var clusterWardCodes = new Dictionary<string, List<int>>();

                // Iterate through each covered cluster
                foreach (var cluster in coveredClusters)
                {
                    // Retrieve ward codes associated with the current cluster
                    var wardCodes = _context.ClusterWards
                        .Where(wc => wc.ClusterId == cluster.ClusterId)
                        .Select(wc => wc.WardCode)
                        .ToList();

                    // Add cluster name and associated ward codes to the dictionary
                    clusterWardCodes.Add(cluster.ClusterId.ToString(), wardCodes);
                }

                return Json(clusterWardCodes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving covered clusters: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoveredCluster(int id)
        {
            var coveredCluster = await _context.CoveredClusters.FindAsync(id);
            if (coveredCluster == null)
            {
                return NotFound();
            }

            _context.CoveredClusters.Remove(coveredCluster);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CoveredClusterExists(int id)
        {
            return _context.CoveredClusters.Any(e => e.Id == id);
        }
    }
}
