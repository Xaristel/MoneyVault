using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;

namespace Money_Vault.ViewModel
{
    public class PlanningAccountAddViewModel : BaseViewModel
    {
        private string _name;
        private string _amount;
        private string _number;

        private RelayCommand<IClosable> _addCommand;

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

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && Name.ToString() != ""
                    && Number.ToString() != ""
                    && int.TryParse(Number, out int result))
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            database.Accounts.Add(new Account()
                            {
                                User_Id = Convert.ToInt32(Settings.Default["currentUserId"]),
                                Name = Name,
                                Current_Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount),
                                Number = Convert.ToInt32(Number)
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

        public PlanningAccountAddViewModel()
        {
            Number = "";
            Name = "";
            Amount = "";
        }
    }
}
