namespace ExpenseSplitterAPI.Models
{
    // ── Matches: Users table ──────────────────────────────
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;  // stored as text "890"
    }

    // ── Matches: Groups table ─────────────────────────────
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int CreatedBy { get; set; }  // FK → Users.UserID
    }

    // ── Matches: GroupMembers table ───────────────────────
    public class GroupMember
    {
        public int GroupID { get; set; }   // FK → Groups.GroupID
        public int UserID { get; set; }    // FK → Users.UserID
    }

    // ── Matches: Expenses table ───────────────────────────
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int GroupID { get; set; }       // FK → Groups.GroupID
        public int PaidBy { get; set; }        // FK → Users.UserID (who paid)
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    // ── Matches: ExpenseSplit table ───────────────────────
    public class ExpenseSplit
    {
        public int SplitID { get; set; }
        public int ExpenseID { get; set; }     // FK → Expenses.ExpenseID
        public int UserID { get; set; }        // FK → Users.UserID (who owes)
        public decimal AmountOwed { get; set; }
        public int FromUser { get; set; }      // who owes
        public int ToUser { get; set; }        // who is owed
    }

    // ── Request DTOs (what the frontend sends) ────────────

    public class RegisterRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateGroupRequest
    {
        public string Name { get; set; } = string.Empty;
    }

    public class AddExpenseRequest
    {
        public int GroupID { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class SplitExpenseRequest
    {
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public int GroupID { get; set; }
    }
}
