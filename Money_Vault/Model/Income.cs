using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Incomes")]
    public partial class Income : BaseModel
    {
        private int _id;
        private int _user_Id;
        private int _income_Type_Id;
        private int _total_Amount;
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
        public int Income_Type_Id
        {
            get => _income_Type_Id;
            set
            {
                _income_Type_Id = value;
                OnPropertyChanged("Income_Type_Id");
            }
        }

        [Required]
        public int Total_Amount
        {
            get => _total_Amount;
            set
            {
                _total_Amount = value;
                OnPropertyChanged("Total_Amount");
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
