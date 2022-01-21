using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Notes")]
    public partial class Note
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int User_Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
