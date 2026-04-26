using ExpenseSplitterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseSplitterAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // These names must exactly match Disha's table names in ExpenseDB
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseSplit> ExpenseSplit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // GroupMembers has a composite key (GroupID + UserID together = unique row)
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => new { gm.GroupID, gm.UserID });

            // Tell EF the primary key column names (they use ID not Id)
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<Group>()
                .HasKey(g => g.GroupID);

            modelBuilder.Entity<Expense>()
                .HasKey(e => e.ExpenseID);

            modelBuilder.Entity<ExpenseSplit>()
                .HasKey(s => s.SplitID);
        }
    }
}
