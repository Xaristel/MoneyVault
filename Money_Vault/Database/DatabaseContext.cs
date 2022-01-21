using System.Data.Entity;
using Money_Vault.Model;

namespace Money_Vault.Database
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {

        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Account_Operation> Account_Operation { get; set; }
        public virtual DbSet<Expense> Expense { get; set; }
        public virtual DbSet<Expense_Type> Expense_Type { get; set; }
        public virtual DbSet<Goal> Goal { get; set; }
        public virtual DbSet<Income> Income { get; set; }
        public virtual DbSet<Income_Type> Income_Type { get; set; }
        public virtual DbSet<Note> Note { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Product_List> Product_List { get; set; }
        public virtual DbSet<Product_Type> Product_Type { get; set; }
        public virtual DbSet<Shop> Shop { get; set; }
        public virtual DbSet<User> User { get; set; }
    }
}
