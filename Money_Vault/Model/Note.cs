using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Money_Vault.Model
{
    [Table("Notes")]
    public partial class Note : BaseModel
    {
        private string _id;
        private int _user_Id;
        private string _text;

        public string Id
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

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }
    }
}
