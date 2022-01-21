using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Products")]
    public partial class Product
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Type_Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
