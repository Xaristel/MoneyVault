using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Expense_Items")]
    public partial class Expense_Item : BaseModel
    {
        private int _id;
        private int _expense_Id;
        private int _product_Id;
        private int _quantity;
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
        public int Expense_Id
        {
            get => _expense_Id;
            set
            {
                _expense_Id = value;
                OnPropertyChanged("Expense_Id");
            }
        }

        [Required]
        public int Product_Id
        {
            get => _product_Id;
            set
            {
                _product_Id = value;
                OnPropertyChanged("Product_Id");
            }
        }

        [Required]
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
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
