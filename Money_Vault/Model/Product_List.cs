using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Product_Lists")]
    public partial class Product_List
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Expense_Id { get; set; }

        [Required]
        public int Product_Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
