using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;

namespace Money_Vault.ViewModel
{
    /// <summary>
    /// Логика (ViewModel) для AuthWindow.xaml
    /// </summary>
    public class AuthViewModel : BaseViewModel
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
            get
            {
                return _registerCommand ?? (_registerCommand = new RelayCommand((args) =>
                {
                    var displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var registrationViewModel = new RegistrationViewModel();
                    displayRootRegistry.ShowPresentation(registrationViewModel);
                }));
            }
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
                    string secretPassword = Convert.ToString(login.GetHashCode() + password.GetHashCode());

                    try
                    {
                        var result = from item in _database.Users.ToList()
                                     where item.Login == login && item.Password == secretPassword
                                     select item;

                        if (result.Count() != 0)
                        {
                            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                            var mainViewModel = new MainViewModel();

                            displayRootRegistry.ShowPresentation(mainViewModel);
                            Thread.Sleep(200);
                            displayRootRegistry.HidePresentation(this);
                        }
                        else
                        {
                            //открытие модального окна с ошибкой
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка при выполнении SQL-запроса к БД.", ex);
                    }
                }));
            }
        }

        public AuthViewModel()
        {
            _database = new DatabaseContext();

            Users = _database.Users.ToList();
        }
    }
}
