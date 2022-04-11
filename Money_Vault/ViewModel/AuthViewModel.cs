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

        public AuthViewModel()
        {
            _database = new DatabaseContext();

            Users = _database.Users.ToList();

            if (Convert.ToBoolean(Settings.Default["isKeepAuthData"]))
            {

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
                    string login = ((Tuple<string, string, bool>)tuple).Item1;
                    string password = ((Tuple<string, string, bool>)tuple).Item2;
                    bool isKeepAuthData = ((Tuple<string, string, bool>)tuple).Item3;

                    string secretPassword = Convert.ToString(login.GetHashCode() + password.GetHashCode());
                    Settings.Default["isKeepAuthData"] = isKeepAuthData;
                    Settings.Default["login"] = login;

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
                            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                            var messageViewModel = new MessageViewModel("Ошибка", "Пользователя не существует или были введены неверные данные.");
                            displayRootRegistry.ShowPresentation(messageViewModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Ошибка при выполнении SQL-запроса к БД.", ex);
                    }
                }));
            }
        }
    }
}
