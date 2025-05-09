using Money_Vault.Database;
using Money_Vault.Properties;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        private string _login;
        private bool _isKeepLogin;
        private string _password;
        private bool _isKeepPassword;

        private RelayCommand _registerCommand;
        private RelayCommand _authCommand;

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

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public bool IsKeepPassword
        {
            get => _isKeepPassword;
            set
            {
                _isKeepPassword = value;
                OnPropertyChanged("IsKeepPassword");
            }
        }

        public AuthViewModel()
        {
            IsKeepLogin = Convert.ToBoolean(Settings.Default["isKeepLogin"]);
            IsKeepPassword = Convert.ToBoolean(Settings.Default["isKeepPassword"]);

            if (IsKeepLogin)
            {
                Login = Settings.Default["keepLogin"].ToString();
            }

            if (IsKeepPassword)
            {
                Password = Settings.Default["keepPassword"].ToString();
            }

            if (!File.Exists(@"Database/Database.db"))
            {
                using (DatabaseContext databaseContext = new DatabaseContext())
                {
                    databaseContext.Database.Initialize(true);
                    databaseContext.Database.ExecuteSqlCommand(DatabaseCreateScript.DatabaseCreateCommand);
                }
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
                    string login = ((Tuple<string, string, bool, bool>)tuple).Item1;
                    string password = ((Tuple<string, string, bool, bool>)tuple).Item2;
                    IsKeepLogin = ((Tuple<string, string, bool, bool>)tuple).Item3;
                    IsKeepPassword = ((Tuple<string, string, bool, bool>)tuple).Item4;

                    string secretPassword = Convert.ToString(login.GetHashCode() + password.GetHashCode());
                    Settings.Default["isKeepLogin"] = IsKeepLogin;
                    Settings.Default["keepLogin"] = login;
                    Settings.Default["isKeepPassword"] = IsKeepPassword;
                    Settings.Default["keepPassword"] = password;

                    using (DatabaseContext database = new DatabaseContext())
                    {
                        var result = from item in database.Users.ToList()
                                     where item.Login == login && item.Password == secretPassword
                                     select item;

                        if (result.Count() != 0)
                        {
                            Settings.Default["currentUserId"] = result.ToList()[0].Id;
                            Settings.Default.Save();

                            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                            var mainViewModel = new MainViewModel();

                            displayRootRegistry.ShowPresentation(mainViewModel);
                            Thread.Sleep(150);
                            displayRootRegistry.HidePresentation(this);
                        }
                        else
                        {
                            await AdditionalFunctions.CallModalMessage("Ошибка", "Пользователя не существует или были введены неверные данные.");
                        }
                    }
                }));
            }
        }
    }
}
