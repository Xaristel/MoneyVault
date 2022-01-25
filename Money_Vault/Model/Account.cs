using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Money_Vault.Model
{
    [Table("Accounts")]
    public partial class Account : BaseModel
    {
        private int _id;
        private int _user_Id;
        private string _name;
        private int _number;
        private int _current_Amount;

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
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public int Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }

        [Required]
        public int Current_Amount
        {
            get => _current_Amount;
            set
            {
                _current_Amount = value;
                OnPropertyChanged("Current_Amount");
            }
        }
    }
}
