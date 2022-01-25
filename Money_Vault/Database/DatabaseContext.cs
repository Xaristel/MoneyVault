using System.Data.Entity;
using Money_Vault.Model;

namespace Money_Vault.Database
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {

        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Account_Operation> Account_Operations { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Expense_Type> Expense_Types { get; set; }
        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<Income_Type> Income_Types { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Product_List> Product_Lists { get; set; }
        public virtual DbSet<Product_Type> Product_Types { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
