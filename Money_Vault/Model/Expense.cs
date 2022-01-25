using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Expenses")]
    public partial class Expense : BaseModel
    {
        private int _id;
        private int _user_Id;
        private int _expense_Type_Id;
        private int _shop_Id;
        private int _total_Price;
        private string _date;
        private string _note;

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
        public int User_Id
        {
            get => _user_Id;
            set
            {
                _user_Id = value;
                OnPropertyChanged("User_Id");
            }
        }

        [Required]
        public int Expense_Type_Id
        {
            get => _expense_Type_Id;
            set
            {
                _expense_Type_Id = value;
                OnPropertyChanged("Expense_Type_Id");
            }
        }

        [Required]
        public int Shop_Id
        {
            get => _shop_Id;
            set
            {
                _shop_Id = value;
                OnPropertyChanged("Shop_Id");
            }
        }

        [Required]
        public int Total_Price
        {
            get => _total_Price;
            set
            {
                _total_Price = value;
                OnPropertyChanged("Total_Price");
            }
        }

        [Required]
        public string Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged("Note");
            }
        }
    }
}
