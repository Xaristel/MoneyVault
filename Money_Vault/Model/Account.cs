using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Accounts")]
    public partial class Account
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        public int Number { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Current_Amount { get; set; }
    }
}
