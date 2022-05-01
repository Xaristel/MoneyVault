using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class ExpenseGeneralViewModel : BaseViewModel
    {
        private RelayCommand _selectMonthCommand;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private List<string> _yearsList;
        private string _currentYear;
        private string _currentMonth;
        private bool _isEnableMonthsButtons;

        private List<string> _monthsList = new List<string>
        {
        "Январь",
        "Февраль",
        "Март",
        "Апрель",
        "Май",
        "Июнь",
        "Июль",
        "Август",
        "Сентябрь",
        "Октябрь",
        "Ноябрь",
        "Декабрь",
        "Полный год"
        };

        private IEnumerable<ExpenseCommonListItem> _expensesList;

        public string CurrentYear
        {
            get => _currentYear;
            set
            {
                _currentYear = value;
                OnPropertyChanged("CurrentYear");

                if (_currentMonth != null)
                {
                    UpdateData();
                }
            }
        }

        public string CurrentMonth
        {
            get => _currentMonth;
            set
            {
                _currentMonth = value;
                OnPropertyChanged("CurrentMonth");

                if (_currentYear != null)
                {
                    UpdateData();
                }
            }
        }

        public List<string> YearsList
        {
            get => _yearsList;
            set
            {
                _yearsList = value;
                OnPropertyChanged("YearsList");
            }
        }

        public IEnumerable<ExpenseCommonListItem> ExpensesList
        {
            get => _expensesList;
            set
            {
                _expensesList = value;
                OnPropertyChanged("ExpensesList");
            }
        }

        public RelayCommand SelectMonthCommand
        {
            get
            {
                return _selectMonthCommand ?? (_selectMonthCommand = new RelayCommand(obj =>
                {
                    CurrentMonth = obj as string;
                }));
            }
        }

        public bool IsEnableMonthsButtons
        {
            get => _isEnableMonthsButtons;
            set
            {
                _isEnableMonthsButtons = value;
                OnPropertyChanged("IsEnableMonthsButtons");
            }
        }

        public RelayCommand ShowAddFrameCommand
        {
            get
            {
                return _showAddFrameCommand ?? (_showAddFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var expenseGeneralAddViewModel = new ExpenseGeneralAddViewModel();
                    _displayRootRegistry.ShowPresentation(expenseGeneralAddViewModel);
                }));
            }
        }
        public RelayCommand ShowEditFrameCommand
        {
            get
            {
                return _showEditFrameCommand ?? (_showEditFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var expenseGeneralEditViewModel = new ExpenseGeneralEditViewModel();
                    _displayRootRegistry.ShowPresentation(expenseGeneralEditViewModel);

                }));
            }
        }
        public RelayCommand ShowDeleteFrameCommand
        {
            get
            {
                return _showDeleteFrameCommand ?? (_showDeleteFrameCommand = new RelayCommand(async (args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var messageViewModel = new MessageViewModel(
                        "Внимание",
                        "Вы действительно хотите удалить данную запись?");

                    await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                    if (messageViewModel.Result)
                    {
                        //
                    }
                }));
            }
        }

        public ExpenseGeneralViewModel()
        {
            CurrentMonth = _monthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);
            IsEnableMonthsButtons = true;

            FillYearsList();
        }

        public void UpdateData()
        {
            if (CurrentYear == "Все годы")
            {
                if (CurrentMonth != "Полный год")
                {
                    CurrentMonth = "Полный год";
                }
                IsEnableMonthsButtons = false;
            }
            else
            {
                IsEnableMonthsButtons = true;
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                List<ExpenseCommonListItem> tempList = new List<ExpenseCommonListItem>();

                foreach (var item in database.Expenses.ToList())
                {
                    if (item.Date.Contains($".{_monthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                        || item.Date.Contains($".0{_monthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                        || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                        || CurrentYear == "Все годы")
                    {
                        tempList.Add(new ExpenseCommonListItem()
                        {
                            Id = item.Id,
                            TypeName = database.Expense_Types.ToList().Find(x => x.Id == item.Expense_Type_Id).Name,
                            Amount = AdditionalFunctions.ConvertToCurrencyFormat(item.Total_Price),
                            ShopName = database.Shops.ToList().Find(x => x.Id == item.Shop_Id).Name,
                            Date = new DateTime(Convert.ToInt32(item.Date.Split('.')[2]), Convert.ToInt32(item.Date.Split('.')[1]), Convert.ToInt32(item.Date.Split('.')[0])),
                            Note = item.Note
                        });
                    }
                }

                ExpensesList = tempList;
            }
        }

        private void FillYearsList()
        {
            YearsList = new List<string>
            {
                CurrentYear
            };

            using (DatabaseContext database = new DatabaseContext())
            {
                foreach (var item in database.Expenses.ToList())
                {
                    //try to get year from date (14.05.2022 -> 2022)
                    string tempYear = item.Date.Split('.')[2];

                    if (!YearsList.Contains(tempYear))
                    {
                        YearsList.Add(tempYear);
                    }
                }
            }

            YearsList.Sort();
            YearsList.Add("Все годы");
        }
    }
}
