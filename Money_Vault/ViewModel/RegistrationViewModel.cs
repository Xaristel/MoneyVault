using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class RegistrationViewModel : BaseViewModel
    {
        private RelayCommand _registerNewUserCommand;

        private string regLogin;
        private string regPassword;
        private string regName;
        private string regSurname;

        public string RegLogin
        {
            get => regLogin;
            set
            {
                regLogin = value;
                OnPropertyChanged("RegLogin");
            }
        }

        public string RegPassword
        {
            get => regPassword;
            set
            {
                regPassword = value;
                OnPropertyChanged("RegPassword");
            }
        }

        public string RegName
        {
            get => regName;
            set
            {
                regName = value;
                OnPropertyChanged("RegName");
            }
        }

        public string RegSurname
        {
            get => regSurname;
            set
            {
                regSurname = value;
                OnPropertyChanged("RegSurname");
            }
        }

        public RelayCommand RegisterNewUserCommand
        {
            get
            {
                return _registerNewUserCommand ?? (_registerNewUserCommand = new RelayCommand((args) =>
                {
                    try
                    {
                        using (DatabaseContext dbContext = new DatabaseContext())
                        {
                            User newUser = new User()
                            {
                                Login = RegLogin,
                                Password = Convert.ToString(RegPassword.GetHashCode() + RegLogin.GetHashCode()),
                                Name = RegName,
                                Surname = RegSurname
                            };

                            dbContext.Users.Add(newUser);
                            dbContext.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    var messageViewModel = new MessageViewModel("", $"Пользователь {RegLogin} успешно зарегистрирован.");
                    (Application.Current as App).displayRootRegistry.ShowPresentation(messageViewModel);

                    (Application.Current as App).displayRootRegistry.HidePresentation(this);
                }));
            }
        }
    }
}
