using Microsoft.AspNetCore.Mvc;
using sacpapi.Data;
using sacpapi.Models;

namespace sacpapi.Controllers
{
    [Route("/[controller]")]
    public class GroupMemberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupMemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Index(int id)
        {
            var result = _context.GroupMembers
                .Where(g => g.GroupId == id)
                .Select(g => g.BeneficiaryId)
                .ToArray();

            var members = _context.Beneficiaries
                .Where(item => result.Contains(item.Id))
                .ToList();

            return Json(members);
        }

        [HttpPost("{groupId}")]
        public IActionResult Craete(int groupId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        var exists = _context.GroupMembers.FirstOrDefault(g => g.BeneficiaryId == id && g.GroupId == groupId);

                        if (exists == null)
                        {
                            GroupMember groupMember = new GroupMember();
                            groupMember.BeneficiaryId = id;
                            groupMember.GroupId = groupId;

                            _context.GroupMembers.Add(groupMember);
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

        [HttpPost("remove/{groupId}")]
        public IActionResult Remove(int groupId, [FromBody] List<int> ids)
        {
            try
            {
                if (ids != null && ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        var groupMember = _context.GroupMembers.FirstOrDefault(g => g.BeneficiaryId == id && g.GroupId == groupId);

                        if (groupMember != null)
                        {
                            _context.GroupMembers.Remove(groupMember);
                        }

                        continue;
                    }

                    _context.SaveChanges();

                    return StatusCode(204);
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
