using ExpenseSplitterAPI.Data;
using ExpenseSplitterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseSplitterAPI.Controllers
{
    [ApiController]
    [Route("api/splitexpense")]
    [Authorize]
    public class SplitExpenseController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SplitExpenseController(AppDbContext db)
        {
            _db = db;
        }

        // POST /api/splitexpense
        // Frontend split.html sends: { description, totalAmount, groupID }
        // This creates the expense then splits it equally among group members
        [HttpPost]
        public IActionResult CreateSplit    ([FromBody] SplitExpenseRequest req)
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Step 1: Save the main expense first
            var expense = new Expense
            {
                GroupID = req.GroupID,
                PaidBy = userID,
                Amount = req.TotalAmount,
                Description = req.Description
            };

            _db.Expenses.Add(expense);
            _db.SaveChanges();

            // Step 2: Find all members of this group
            var memberIDs = _db.GroupMembers
                               .Where(gm => gm.GroupID == req.GroupID)
                               .Select(gm => gm.UserID)
                               .ToList();

            if (memberIDs.Count == 0)
                return BadRequest(new { message = "No members found in this group." });

            // Step 3: Split the total equally
            decimal sharePerPerson = req.TotalAmount / memberIDs.Count;

            // Step 4: Create a split record for each member (except the payer)
            foreach (var memberID in memberIDs)
            {
                if (memberID == userID) continue;  // payer doesn't owe themselves

                var split = new ExpenseSplit
                {
                    ExpenseID = expense.ExpenseID,
                    UserID = memberID,
                    AmountOwed = sharePerPerson,
                    FromUser = memberID,   // this person owes
                    ToUser = userID      // to the person who paid
                };

                _db.ExpenseSplit.Add(split);
            }

            _db.SaveChanges();

            return Ok(new
            {
                message = "Expense split successfully!",
                expense.ExpenseID,
                totalAmount = req.TotalAmount,
                splitAmong = memberIDs.Count,
                eachPersonOwes = sharePerPerson
            });
        }

        // GET /api/splitexpense
        // Returns all amounts the logged-in user owes to others
        [HttpGet]
        public IActionResult GetMySplits()
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var splits = _db.ExpenseSplit
                            .Where(s => s.FromUser == userID)
                            .ToList();

            return Ok(splits);
        }
    }
}
