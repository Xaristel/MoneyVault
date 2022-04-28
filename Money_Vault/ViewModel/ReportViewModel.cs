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
    public class ReportViewModel : BaseViewModel
    {
        private DatabaseContext _database;

        private RelayCommand _createReportCommand;
        private RelayCommand _saveReportCommand;
        private RelayCommand _printReportCommand;

        private IEnumerable<IncomeCommonListItem> _incomeDataList;
        private IEnumerable<ExpenseCommonListItem> _expenseDataList;
        private IEnumerable<TotalListItem> _totalDataList;

        private Visibility _isVisibleDataGridIncome;
        private Visibility _isVisibleDataGridExpense;
        private Visibility _isVisibleDataGridTotal;
        private bool _isEnableComboBoxMonth;

        private IEnumerable<string> _yearsList;
        private List<string> _operationsList = new List<string>
        {
        "Доходы",
        "Расходы"
        };
        private List<string> _reportTypesList = new List<string>
        {
        "Операции",
        "Категории"
        };
        private string _currentYear;
        private string _currentMonth;
        private string _currentOperationsType;
        private string _currentReportType;

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

        public string CurrentYear
        {
            get => _currentYear;
            set
            {
                _currentYear = value;
                OnPropertyChanged("CurrentYear");

                if (_currentYear == "Все годы")
                {
                    if (CurrentMonth != "Полный год")
                    {
                        CurrentMonth = "Полный год";
                    }
                    IsEnableComboBoxMonth = false;
                }
                else
                {
                    IsEnableComboBoxMonth = true;
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
            }
        }

        public string CurrentOperationsType
        {
            get => _currentOperationsType;
            set
            {
                _currentOperationsType = value;
                FillYearsList();

                OnPropertyChanged("CurrentOperationsType");
            }
        }

        public string CurrentReportType
        {
            get => _currentReportType;
            set
            {
                _currentReportType = value;
                OnPropertyChanged("CurrentReportType");
            }
        }

        public List<string> MonthsList
        {
            get => _monthsList;
            set
            {
                _monthsList = value;
                OnPropertyChanged("MonthList");
            }
        }

        public IEnumerable<string> YearsList
        {
            get => _yearsList;
            set
            {
                _yearsList = value;
                OnPropertyChanged("YearsList");
            }
        }

        public List<string> OperationsList
        {
            get => _operationsList;
            set
            {
                _operationsList = value;
                OnPropertyChanged("OperationsList");
            }
        }

        public List<string> ReportTypesList
        {
            get => _reportTypesList;
            set
            {
                _reportTypesList = value;
                OnPropertyChanged("ReportTypesList");
            }
        }

        public IEnumerable<IncomeCommonListItem> IncomeDataList
        {
            get => _incomeDataList;
            set
            {
                _incomeDataList = value;
                OnPropertyChanged("IncomeDataList");
            }
        }

        public IEnumerable<ExpenseCommonListItem> ExpenseDataList
        {
            get => _expenseDataList;
            set
            {
                _expenseDataList = value;
                OnPropertyChanged("ExpenseDataList");
            }
        }

        public IEnumerable<TotalListItem> TotalDataList
        {
            get => _totalDataList;
            set
            {
                _totalDataList = value;
                OnPropertyChanged("TotalDataList");
            }
        }

        public Visibility IsVisibleDataGridIncome
        {
            get => _isVisibleDataGridIncome;
            set
            {
                _isVisibleDataGridIncome = value;
                OnPropertyChanged("IsVisibleDataGridIncome");
            }
        }
        public Visibility IsVisibleDataGridExpense
        {
            get => _isVisibleDataGridExpense;
            set
            {
                _isVisibleDataGridExpense = value;
                OnPropertyChanged("IsVisibleDataGridExpense");
            }
        }
        public Visibility IsVisibleDataGridTotal
        {
            get => _isVisibleDataGridTotal;
            set
            {
                _isVisibleDataGridTotal = value;
                OnPropertyChanged("IsVisibleDataGridTotal");
            }
        }

        public bool IsEnableComboBoxMonth
        {
            get => _isEnableComboBoxMonth;
            set
            {
                _isEnableComboBoxMonth = value;
                OnPropertyChanged("IsEnableComboBoxMonth");
            }
        }

        public RelayCommand CreateReportCommand
        {
            get
            {
                return _createReportCommand ?? (_createReportCommand = new RelayCommand((args) =>
                {
                    UpdateData();
                }));
            }
        }
        public RelayCommand SaveReportCommand
        {
            get
            {
                return _saveReportCommand ?? (_saveReportCommand = new RelayCommand((args) =>
                {
                    //UpdateData();
                }));
            }
        }
        public RelayCommand PrintReportCommand
        {
            get
            {
                return _printReportCommand ?? (_printReportCommand = new RelayCommand((args) =>
                {
                    //UpdateData();
                }));
            }
        }

        public ReportViewModel()
        {
            _database = new DatabaseContext();

            CurrentMonth = _monthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);
        }

        private void FillYearsList()
        {
            List<string> years = new List<string>
            {
                CurrentYear
            };

            switch (CurrentOperationsType)
            {
                case "Доходы":
                    {
                        foreach (var item in _database.Incomes.ToList())
                        {
                            //try to get year from date (14.05.2022 -> 2022)
                            string tempYear = item.Date.Split('.')[2];

                            if (!years.Contains(tempYear))
                            {
                                years.Add(tempYear);
                            }
                        }
                        break;
                    }
                case "Расходы":
                    {
                        foreach (var item in _database.Expenses.ToList())
                        {
                            //try to get year from date (14.05.2022 -> 2022)
                            string tempYear = item.Date.Split('.')[2];

                            if (!years.Contains(tempYear))
                            {
                                years.Add(tempYear);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }

            years.Sort();
            years.Add("Все годы");
            YearsList = years;
        }

        public void UpdateData()
        {
            List<IncomeCommonListItem> listIncomes = new List<IncomeCommonListItem>();
            List<ExpenseCommonListItem> listExpenses = new List<ExpenseCommonListItem>();
            List<TotalListItem> listTotal = new List<TotalListItem>();

            switch (CurrentOperationsType)
            {
                case "Доходы":
                    {
                        switch (CurrentReportType)
                        {
                            case "Операции":
                                {
                                    IsVisibleDataGridIncome = Visibility.Visible;
                                    IsVisibleDataGridExpense = Visibility.Hidden;
                                    IsVisibleDataGridTotal = Visibility.Hidden;

                                    foreach (var item in _database.Incomes.ToList())
                                    {
                                        listIncomes.Add(new IncomeCommonListItem()
                                        {
                                            Id = item.Id,
                                            TypeName = _database.Income_Types.ToList().Find(x => x.Id == item.Income_Type_Id).Name,
                                            Amount = AdditionalFunctions.ConvertToCurrencyFormat(item.Total_Amount),
                                            Date = new DateTime(Convert.ToInt32(item.Date.Split('.')[2]), Convert.ToInt32(item.Date.Split('.')[1]), Convert.ToInt32(item.Date.Split('.')[0])),
                                            Note = item.Note
                                        });
                                    }
                                    break;
                                }
                            case "Категории":
                                {
                                    IsVisibleDataGridIncome = Visibility.Hidden;
                                    IsVisibleDataGridExpense = Visibility.Hidden;
                                    IsVisibleDataGridTotal = Visibility.Visible;

                                    //GeneralViewModel generalViewModel = new GeneralViewModel();

                                    //listTotal = generalViewModel.FillIncomesTotalList();
                                    break;
                                }
                            default:
                                break;
                        }

                        break;
                    }
                case "Расходы":
                    {
                        switch (CurrentReportType)
                        {
                            case "Операции":
                                {
                                    IsVisibleDataGridIncome = Visibility.Hidden;
                                    IsVisibleDataGridExpense = Visibility.Visible;
                                    IsVisibleDataGridTotal = Visibility.Hidden;

                                    foreach (var item in _database.Expenses.ToList())
                                    {
                                        listExpenses.Add(new ExpenseCommonListItem()
                                        {
                                            Id = item.Id,
                                            TypeName = _database.Expense_Types.ToList().Find(x => x.Id == item.Expense_Type_Id).Name,
                                            Amount = AdditionalFunctions.ConvertToCurrencyFormat(item.Total_Price),
                                            ShopName = _database.Shops.ToList().Find(x => x.Id == item.Shop_Id).Name,
                                            Date = new DateTime(Convert.ToInt32(item.Date.Split('.')[2]), Convert.ToInt32(item.Date.Split('.')[1]), Convert.ToInt32(item.Date.Split('.')[0])),
                                            Note = item.Note
                                        });
                                    }
                                    break;
                                }
                            case "Категории":
                                {
                                    IsVisibleDataGridIncome = Visibility.Hidden;
                                    IsVisibleDataGridExpense = Visibility.Hidden;
                                    IsVisibleDataGridTotal = Visibility.Visible;

                                    //GeneralViewModel generalViewModel = new GeneralViewModel();

                                    //listTotal = generalViewModel.FillExpensesTotalList();
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                default:
                    break;
            }

            ExpenseDataList = listExpenses;
            IncomeDataList = listIncomes;
            TotalDataList = listTotal;
        }
    }
}
