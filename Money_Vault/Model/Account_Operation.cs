using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Account_Operations")]
    public partial class Account_Operation
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Account_Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public int Amount { get; set; }

    }
}
