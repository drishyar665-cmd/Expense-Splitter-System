using ExpenseSplitterAPI.Data;
using ExpenseSplitterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseSplitterAPI.Controllers
{
    [ApiController]
    [Route("api/group")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly AppDbContext _db;

        public GroupController(AppDbContext db)
        {
            _db = db;
        }

        // POST /api/group
        // Frontend group.html sends: { name }
        [HttpPost]
        public IActionResult CreateGroup([FromBody] CreateGroupRequest req)
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Create the group
            var group = new Group
            {
                GroupName = req.Name,
                CreatedBy = userID
            };

            _db.Groups.Add(group);
            _db.SaveChanges();

            // Also add the creator as a member of their own group
            var member = new GroupMember
            {
                GroupID = group.GroupID,
                UserID = userID
            };

            _db.GroupMembers.Add(member);
            _db.SaveChanges();

            return Ok(new { message = "Group created!", group.GroupID, group.GroupName });
        }

        // GET /api/group
        // Returns all groups the logged-in user belongs to
        [HttpGet]
        public IActionResult GetMyGroups()
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var myGroupIDs = _db.GroupMembers
                               .Where(gm => gm.UserID == userID)
                               .Select(gm => gm.GroupID)
                               .ToList();

            var groups = _db.Groups
                            .Where(g => myGroupIDs.Contains(g.GroupID))
                            .ToList();

            return Ok(groups);
        }
    }
}
