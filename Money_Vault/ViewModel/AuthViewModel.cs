﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;

namespace Money_Vault.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        private DatabaseContext _database;

        private string _login;
        private bool _isKeepLogin;

        private RelayCommand _registerCommand;
        private RelayCommand _authCommand;

        private IEnumerable<User> _users;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        public bool IsKeepLogin
        {
            get => _isKeepLogin;
            set
            {
                _isKeepLogin = value;
                OnPropertyChanged("IsKeepLogin");
            }
        }

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
            IsKeepLogin = Convert.ToBoolean(Settings.Default["isKeepLogin"]);

            if (IsKeepLogin)
            {
                Login = Settings.Default["keepLogin"].ToString();
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
                (_authCommand = new RelayCommand(async tuple =>
                {
                    string login = ((Tuple<string, string, bool>)tuple).Item1;
                    string password = ((Tuple<string, string, bool>)tuple).Item2;
                    IsKeepLogin = ((Tuple<string, string, bool>)tuple).Item3;

                    string secretPassword = Convert.ToString(login.GetHashCode() + password.GetHashCode());
                    Settings.Default["isKeepLogin"] = IsKeepLogin;
                    Settings.Default["keepLogin"] = login;
                    Settings.Default.Save();

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
                            var messageViewModel = new MessageViewModel(
                                "Ошибка",
                                "Пользователя не существует или были введены неверные данные.");
                            await displayRootRegistry.ShowModalPresentation(messageViewModel);
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
