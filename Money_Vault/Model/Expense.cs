using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Expenses")]
    public partial class Expense
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        public int Expense_Type_Id { get; set; }

        [Required]
        public int Shop_Id { get; set; }

        [Required]
        public int Total_Price { get; set; }

        [Required]
        public string Date { get; set; }

        public string Note { get; set; }
    }
}
