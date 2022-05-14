using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class PlanningAccountsOperationAddViewModel : BaseViewModel
    {
        private int _currentAccountId;
        private string _selectedOperationType;
        private string _amount;
        private DateTime _date;
        private Visibility _isVisibleLabelPlaceHolderOperationType;

        private RelayCommand<IClosable> _addCommand;

        private IEnumerable<string> _operationTypesList = new List<string>() { "Пополнение", "Снятие" };

        public string SelectedOperationType
        {
            get => _selectedOperationType;
            set
            {
                _selectedOperationType = value;
                OnPropertyChanged("SelectedOperationType");

                IsVisibleLabelPlaceHolderOperationType = Visibility.Hidden;
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

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public Visibility IsVisibleLabelPlaceHolderOperationType
        {
            get => _isVisibleLabelPlaceHolderOperationType;
            set
            {
                _isVisibleLabelPlaceHolderOperationType = value;
                OnPropertyChanged("IsVisibleLabelPlaceHolderOperationType");
            }
        }

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && SelectedOperationType != null
                    && Date.ToString() != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            database.Account_Operations.Add(new Account_Operation()
                            {
                                Account_Id = _currentAccountId,
                                Type = SelectedOperationType,
                                Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount),
                                Date = Date.ToString("dd.MM.yyyy")
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

        public IEnumerable<string> OperationTypesList
        {
            get => _operationTypesList;
            set
            {
                _operationTypesList = value;
                OnPropertyChanged("OperationTypesList");
            }
        }

        public PlanningAccountsOperationAddViewModel() { }

        public PlanningAccountsOperationAddViewModel(int accountId)
        {
            _currentAccountId = accountId;
            Date = System.DateTime.Now;
            Amount = "";
        }
    }
}
