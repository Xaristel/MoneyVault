using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;

namespace Money_Vault.ViewModel 
{
    /// <summary>
    /// ViewModel для AuthWindow.xaml. Содержит взаимодействие с БД (таблица User)
    /// </summary>
    internal class AuthViewModel : INotifyPropertyChanged
    {
        private DatabaseContext _database;

        private RelayCommand _registerCommand;
        private RelayCommand _authCommand;

        private bool _isKeepAuthData;

        private IEnumerable<User> _users;

        public IEnumerable<User> Users 
        { 
            get => _users;
            set 
            {
                _users = value;
                OnPropertyChanged("Users");
            } 
        }

        public RelayCommand RegisterCommand 
        { 
            get => _registerCommand; 
        }

        public RelayCommand AuthCommand 
        { 
            get
            {
                return _authCommand ??
                (_authCommand = new RelayCommand(tuple =>
                {
                    string login = ((Tuple<string, string>)tuple).Item1;
                    string password = ((Tuple<string, string>)tuple).Item2;

                    
                }));
            }
        }

        public AuthViewModel()
        {
            _database = new DatabaseContext();

            Users = _database.User.ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
