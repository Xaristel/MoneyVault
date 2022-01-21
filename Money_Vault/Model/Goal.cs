using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Goals")]
    public partial class Goal
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        public int Account_Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Required_Amount { get; set; }
    }
}
