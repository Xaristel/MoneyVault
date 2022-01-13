using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Account_Operation)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.Account_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Goal)
                .WithRequired(e => e.Account)
                .HasForeignKey(e => e.Account_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account_Operation>()
                .Property(e => e.Type)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .HasMany(e => e.Product_List)
                .WithRequired(e => e.Expense)
                .HasForeignKey(e => e.Expense_Id);

            modelBuilder.Entity<Expense_Type>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Expense_Type>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Expense_Type>()
                .HasMany(e => e.Expense)
                .WithRequired(e => e.Expense_Type)
                .HasForeignKey(e => e.Expense_Type_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Goal>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Income>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Income_Type>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Income_Type>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Income_Type>()
                .HasMany(e => e.Income)
                .WithRequired(e => e.Income_Type)
                .HasForeignKey(e => e.Income_Type_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Note>()
                .Property(e => e.Text)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Product_List)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => e.Product_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product_Type>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Product_Type>()
                .HasMany(e => e.Product)
                .WithRequired(e => e.Product_Type)
                .HasForeignKey(e => e.Type_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shop>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Shop>()
                .HasMany(e => e.Expense)
                .WithRequired(e => e.Shop)
                .HasForeignKey(e => e.Shop_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Surname)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Account)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Expense)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Goal)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.User_Id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Income)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Note)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.User_Id)
                .WillCascadeOnDelete(false);
        }
    }
}
