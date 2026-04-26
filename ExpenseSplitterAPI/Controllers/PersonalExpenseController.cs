using ExpenseSplitterAPI.Data;
using ExpenseSplitterAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseSplitterAPI.Controllers
{
    [ApiController]
    [Route("api/personalexpense")]
    [Authorize]
    public class PersonalExpenseController : ControllerBase
    {
        private readonly AppDbContext _db;

        public PersonalExpenseController(AppDbContext db)
        {
            _db = db;
        }

        // POST /api/personalexpense
        // Frontend expense.html sends: { title, amount }
        // We map title → Description, amount → Amount
        [HttpPost]
        public IActionResult AddExpense([FromBody] AddExpenseRequest req)
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var expense = new Expense
            {
                GroupID = req.GroupID,
                PaidBy = userID,
                Amount = req.Amount,
                Description = req.Description
            };

            _db.Expenses.Add(expense);
            _db.SaveChanges();

            return Ok(new { message = "Expense added!", expense.ExpenseID });
        }

        // GET /api/personalexpense
        // Returns all expenses paid by the logged-in user
        [HttpGet]
        public IActionResult GetMyExpenses()
        {
            var userID = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var expenses = _db.Expenses
                              .Where(e => e.PaidBy == userID)
                              .ToList();

            return Ok(expenses);
        }
    }
}
