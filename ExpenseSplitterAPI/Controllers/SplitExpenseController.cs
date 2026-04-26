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

        
        [HttpPost]
        public IActionResult CreateSplit    ([FromBody] SplitExpenseRequest req)
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var expense = new Expense
            {
                GroupID = req.GroupID,
                PaidBy = userID,
                Amount = req.TotalAmount,
                Description = req.Description
            };

            _db.Expenses.Add(expense);
            _db.SaveChanges();

            var memberIDs = _db.GroupMembers
                               .Where(gm => gm.GroupID == req.GroupID)
                               .Select(gm => gm.UserID)
                               .ToList();

            if (memberIDs.Count == 0)
                return BadRequest(new { message = "No members found in this group." });

            decimal sharePerPerson = req.TotalAmount / memberIDs.Count;

            foreach (var memberID in memberIDs)
            {
                if (memberID == userID) continue; 

                var split = new ExpenseSplit
                {
                    ExpenseID = expense.ExpenseID,
                    UserID = memberID,
                    AmountOwed = sharePerPerson,
                    FromUser = memberID,   
                    ToUser = userID      
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
