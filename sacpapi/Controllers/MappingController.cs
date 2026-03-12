using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sacpapi.Data;
using sacpapi.Models;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class MappingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string? connectionString;
        public MappingController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnectionString");
        }

        [HttpGet("SubActivities")]
        public async Task<IActionResult> GetAllSubActivities()
        {
            var subActivities = await _context.SubActivities
                .Select(sa => new
                {
                    id = sa.Id,
                    subActivityId = sa.SubActivityId,
                    description = sa.Description,
                    activityId = sa.ActivityId
                })
                .ToListAsync();

            return Ok(subActivities);
        }

        // Optional: Filter by ActivityId
        // GET: api/Parameter/SubActivities/byActivity/{activityId}
        [HttpGet("SubActivities/byActivity/{activityId}")]
        public async Task<IActionResult> GetSubActivitiesByActivity(string activityId)
        {
            var subActivities = await _context.SubActivities
                .Where(sa => sa.ActivityId == activityId)
                .Select(sa => new
                {
                    id = sa.Id,
                    subActivityId = sa.SubActivityId,
                    description = sa.Description,
                    activityId = sa.ActivityId
                })
                .ToListAsync();

            return Ok(subActivities);
        }

        // =========================
        // Get all AWPActivities
        // =========================
        // GET: api/Parameter/AWPActivities
        [HttpGet("AWPActivities")]
        public async Task<IActionResult> GetAllAWPActivities()
        {
            var awpActivities = await _context.AWPActivities
                .Select(a => new
                {
                    id = a.Id,
                    subComponentId = a.SubComponentId,
                    description = a.Description
                })
                .ToListAsync();

            return Ok(awpActivities);
        }



        // Optional: Filter by SubComponentId
        // GET: api/Parameter/AWPActivities/bySubComponent/{subComponentId}
        [HttpGet("AWPActivities/bySubComponent/{subComponentId}")]
        public async Task<IActionResult> GetAWPActivitiesBySubComponent(string subComponentId)
        {
            var awpActivities = await _context.AWPActivities
                .Where(a => a.SubComponentId == subComponentId)
                .Select(a => new
                {
                    id = a.Id,
                    subComponentId = a.SubComponentId,
                    description = a.Description
                })
                .ToListAsync();

            return Ok(awpActivities);
        }


        [HttpGet("activitiessubcomponent")]
        public IActionResult GetAll()
        {
            var activitiesSubComponent = _context.ActivitiesSubComponent.ToList();

            return Json(activitiesSubComponent);
        }






        [HttpGet("activitiessubcomponent/{id}")]
        public IActionResult GetActivitiesSubComponentById(int id)
        {
            var activitiesSubComponent = _context.ActivitiesSubComponent
                .Where(ascds => ascds.SubComponentId == id)
                .Select(ascds => ascds.ActivityId)
                .ToArray();

            return Json(activitiesSubComponent);
        }

        [HttpPost("activitiessubcomponent/{subComponentId}")]
        public IActionResult ActivitiesSubComponent(int subComponentId, [FromBody] List<int> ids)
        {
            try
            {
                var itemsToRemove = _context.ActivitiesSubComponent
                .Where(a => a.SubComponentId == subComponentId);

                _context.ActivitiesSubComponent.RemoveRange(itemsToRemove);
                _context.SaveChanges();

                if (ids != null && ids.Count > 0)
                {

                    foreach (var id in ids)
                    {
                        var exists = _context.ActivitiesSubComponent.FirstOrDefault(g => g.ActivityId == id && g.SubComponentId == subComponentId);

                        if (exists == null)
                        {
                            ActivitySubComponent activitySubComponent = new ActivitySubComponent();
                            activitySubComponent.ActivityId = id;
                            activitySubComponent.SubComponentId = subComponentId;

                            _context.ActivitiesSubComponent.Add(activitySubComponent);
                        }

                        continue;
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





        [HttpGet("documentobject/{objectTypeId}/{fileId}")]
        public IActionResult GetDocumentObjectById(int objectTypeId, int fileId)
        {
            var activitiesSubComponent = _context.DocumentsObjects
                .Where(ascds => ascds.ObjectType == objectTypeId && ascds.FileId == fileId)
                .Select(ascds => ascds.ObjectId)
                .ToArray();

            return Json(activitiesSubComponent);
        }




        [HttpPost("documentobject/{objectTypeId}/{fileId}")]
        public IActionResult DocumentObject(int objectTypeId, int fileId, [FromBody] List<int> ids)
        {
            try
            {
                var itemsToRemove = _context.DocumentsObjects
                    .Where(g => g.FileId == fileId && g.ObjectType == objectTypeId);

                _context.DocumentsObjects.RemoveRange(itemsToRemove);
                _context.SaveChanges();

                if (ids != null && ids.Count > 0)
                {

                    foreach (var id in ids)
                    {
                        var exists = _context.DocumentsObjects.FirstOrDefault(g => g.ObjectId == id && g.FileId == fileId && g.ObjectType == objectTypeId);

                        if (exists == null)
                        {
                            DocumentObject documentObject = new DocumentObject();
                            documentObject.ObjectId = id;
                            documentObject.ObjectType = objectTypeId;
                            documentObject.FileId = fileId;

                            _context.DocumentsObjects.Add(documentObject);
                        }

                        continue;
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



        [HttpGet("clusterwards")]
        public IActionResult GetClusterWards()
        {
            var clusterWards = _context.ClusterWards.ToList();

            return Json(clusterWards);
        }





        [HttpGet("clusterwards/{id}")]
        public IActionResult GetClusterWardsById(int id)
        {
            var clusterwards = _context.ClusterWards
                .Where(ascds => ascds.ClusterId == id)
                .Select(ascds => ascds.WardCode)
                .ToArray();

            return Json(clusterwards);
        }





        [HttpPost("clusterwards/{clusterId}")]
        public IActionResult clusterWards(int clusterId, [FromBody] List<int> ids)
        {
            try
            {
                var itemsToRemove = _context.ClusterWards
                .Where(a => a.ClusterId == clusterId);

                _context.ClusterWards.RemoveRange(itemsToRemove);
                _context.SaveChanges();

                if (ids != null && ids.Count > 0)
                {

                    foreach (var id in ids)
                    {
                        var exists = _context.ClusterWards.FirstOrDefault(g => g.WardCode == id && g.ClusterId == clusterId);

                        if (exists == null)
                        {
                            ClusterWard clusterWard = new ClusterWard();
                            clusterWard.WardCode = id;
                            clusterWard.ClusterId = clusterId;

                            _context.ClusterWards.Add(clusterWard);
                        }

                        continue;
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

    }
}
