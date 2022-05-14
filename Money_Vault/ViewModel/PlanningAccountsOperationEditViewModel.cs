using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class PlanningAccountsOperationEditViewModel : BaseViewModel
    {
        private int _id;
        private string _selectedOperationType;
        private string _amount;
        private DateTime _date;
        private Visibility _isVisibleLabelPlaceHolderOperationType;

        private RelayCommand<IClosable> _editCommand;

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

        public RelayCommand<IClosable> EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && SelectedOperationType != null
                    && Date.ToString() != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            Account_Operation currentOperation = database.Account_Operations.ToList().Find(x => x.Id == _id);

                            currentOperation.Type = SelectedOperationType;
                            currentOperation.Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount);
                            currentOperation.Date = Date.ToString("dd.MM.yyyy");

                            database.SaveChanges();
                        };

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

        public PlanningAccountsOperationEditViewModel() { }

        public PlanningAccountsOperationEditViewModel(int id, string type, string amount, string date)
        {
            _id = id;
            SelectedOperationType = type;
            Date = new DateTime(
                Convert.ToInt32(date.Split('.')[2]),
                Convert.ToInt32(date.Split('.')[1]),
                Convert.ToInt32(date.Split('.')[0]));
            Amount = amount;
        }
    }
}
