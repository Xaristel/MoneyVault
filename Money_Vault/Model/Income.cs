using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Incomes")]
    public partial class Income
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        public int Income_Type_Id { get; set; }

        [Required]
        public int Total_Amount { get; set; }

        [Required]
        public string Date { get; set; }

        public string Note { get; set; }
    }
}
