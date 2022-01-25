using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Goals")]
    public partial class Goal : BaseModel
    {
        private int _id;
        private int _user_Id;
        private int _account_Id;
        private string _name;
        private int _required_Amount;

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
        public int Account_Id
        {
            get => _account_Id;
            set
            {
                _account_Id = value;
                OnPropertyChanged("Account_Id");
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
        public int Required_Amount
        {
            get => _required_Amount;
            set
            {
                _required_Amount = value;
                OnPropertyChanged("Required_Amount");
            }
        }
    }
}
