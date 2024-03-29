using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Account_Operations")]
    public partial class Account_Operation : BaseModel
    {
        private int _id;
        private int _account_Id;
        private string _type;
        private int _amount;
        private string _date;

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
        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }

        [Required]
        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
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
    }
}
