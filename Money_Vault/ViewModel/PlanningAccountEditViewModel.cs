using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Linq;

namespace Money_Vault.ViewModel
{
    public class PlanningAccountEditViewModel : BaseViewModel
    {
        private int _id;
        private string _name;
        private string _amount;
        private string _number;

        private RelayCommand<IClosable> _editCommand;

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

        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged("Number");
            }
        }

        public RelayCommand<IClosable> EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && Name.ToString() != ""
                    && Number.ToString() != ""
                    && int.TryParse(Number, out int result))
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            Account currentAccount = database.Accounts.ToList().Find(x => x.Id == _id);

                            currentAccount.User_Id = Convert.ToInt32(Settings.Default["currentUserId"]);
                            currentAccount.Name = Name;
                            currentAccount.Current_Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount);
                            currentAccount.Number = Convert.ToInt32(Number);

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

        public PlanningAccountEditViewModel() { }

        public PlanningAccountEditViewModel(int id, string name, int number, int amount)
        {
            _id = id;
            Name = name;
            Number = number.ToString();
            Amount = AdditionalFunctions.ConvertToCurrencyFormat(amount).ToString();
        }
    }
}
