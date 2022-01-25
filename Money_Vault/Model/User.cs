using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Money_Vault.Model
{
    [Table("Users")]
    public partial class User : BaseModel
    {
        private int _id;
        private string _login;
        private string _password;
        private string _name;
        private string _surname;

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
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        [Required]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
    }
}
