using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Expense_Types")]
    public partial class Expense_Type
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Note { get; set; }
    }
}
