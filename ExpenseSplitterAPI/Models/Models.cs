namespace ExpenseSplitterAPI.Models
{

    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
    }

    
    public class Group
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int CreatedBy { get; set; }  
    }

    public class GroupMember
    {
        public int GroupID { get; set; }   
        public int UserID { get; set; }    
    }

   
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int GroupID { get; set; }      
        public int PaidBy { get; set; }        
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

   
    public class ExpenseSplit
    {
        public int SplitID { get; set; }
        public int ExpenseID { get; set; }    
        public int UserID { get; set; }        
        public decimal AmountOwed { get; set; }
        public int FromUser { get; set; }      
        public int ToUser { get; set; }       
    }

    

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
