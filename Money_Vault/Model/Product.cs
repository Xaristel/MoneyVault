using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Products")]
    public partial class Product : BaseModel
    {
        private int _id;
        private int _type_Id;
        private string _name;
        private int _price;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        [Required]
        public int Type_Id
        {
            get => _type_Id;
            set
            {
                _type_Id = value;
                OnPropertyChanged("Type_Id");
            }
        }

        [Required]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        [Required]
        public int Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("Price");
            }
        }
    }
}
