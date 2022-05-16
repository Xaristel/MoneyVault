using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class PlanningGoalAddViewModel : BaseViewModel
    {
        private string _name;
        private string _account;
        private string _amount;
        private string _note;
        private Visibility _isVisibleLabelPlaceHolderAccount;

        private RelayCommand<IClosable> _addCommand;

        private IEnumerable<string> _accountsList;

        public string Account
        {
            get => _account;
            set
            {
                _account = value;
                OnPropertyChanged("Account");

                IsVisibleLabelPlaceHolderAccount = Visibility.Hidden;
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

        public string Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
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

        public Visibility IsVisibleLabelPlaceHolderAccount
        {
            get => _isVisibleLabelPlaceHolderAccount;
            set
            {
                _isVisibleLabelPlaceHolderAccount = value;
                OnPropertyChanged("IsVisibleLabelPlaceHolderAccount");
            }
        }

        public IEnumerable<string> AccountsList
        {
            get => _accountsList;
            set
            {
                _accountsList = value;
                OnPropertyChanged("AccountsList");
            }
        }

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && Name != ""
                    && Account != null)
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            database.Goals.Add(new Goal()
                            {
                                User_Id = Convert.ToInt32(Settings.Default["currentUserId"]),
                                Name = Name,
                                Required_Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount),
                                Account_Id = database.Accounts.ToList().Find(x => x.Name == Account).Id,
                                Note = Note
                            });

                            database.SaveChanges();
                        }

                        if (window != null)
                        {
                            window.Close();
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Ошибка", "Заполнены не все поля или введены некорректные значения.");
                    }
                }));
            }
        }

        public PlanningGoalAddViewModel()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                AccountsList = from item in database.Accounts.ToList()
                               where item.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                               select item.Name;
            }

            Name = "";
            Amount = "";
        }
    }
}
