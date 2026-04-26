using ExpenseSplitterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseSplitterAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseSplit> ExpenseSplit { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<GroupMember>()
                .HasKey(gm => new { gm.GroupID, gm.UserID });

           
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
