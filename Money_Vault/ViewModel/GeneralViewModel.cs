using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Media;
using System.Globalization;

namespace Money_Vault.ViewModel
{
    public class GeneralViewModel : BaseViewModel
    {
        private DatabaseContext _database;

        private RelayCommand _removeTypeCommand;

        private bool _isRemoveIncomes;
        private bool _isRemoveExpenses;

        private int _incomesForecast;
        private int _expensesForecast;
        private string _minIncomesDate;
        private string _minExpensesDate;

        private IEnumerable<Income_Type> _income_Types;
        private IEnumerable<Expense_Type> _expense_Types;
        private IEnumerable<Income> _incomes;
        private IEnumerable<Expense> _expenses;
        private IEnumerable<Account> _accounts;
        private IEnumerable<ListItem> _incomesList;
        private IEnumerable<ListItem> _expensesList;
        private SeriesCollection _seriesGeneral;
        private SeriesCollection _seriesIncomes;
        private SeriesCollection _seriesExpenses;

        private List<string> _yearsList;
        private string _currentYear;
        private string _currentMonth;
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

        public Func<ChartPoint, string> PointLabel { get; set; }

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

        public IEnumerable<Income_Type> Income_Types
        {
            get => _income_Types;
            set
            {
                _income_Types = value;
                OnPropertyChanged("Income_Types");
            }
        }

        public IEnumerable<Expense_Type> Expense_Types
        {
            get => _expense_Types;
            set
            {
                _expense_Types = value;
                OnPropertyChanged("Expense_Types");
            }
        }

        public IEnumerable<Income> Incomes
        {
            get => _incomes;
            set
            {
                _incomes = value;
                OnPropertyChanged("Incomes");
            }
        }

        public IEnumerable<Expense> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged("Expenses");
            }
        }

        public IEnumerable<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged("Accounts");
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

        public List<string> MonthsList
        {
            get => _monthsList;
            set
            {
                _monthsList = value;
                OnPropertyChanged("MonthList");
            }
        }

        public IEnumerable<ListItem> IncomesList
        {
            get => _incomesList;
            set
            {
                _incomesList = value;
                OnPropertyChanged("IncomesList");
            }
        }

        public IEnumerable<ListItem> ExpensesList
        {
            get => _expensesList;
            set
            {
                _expensesList = value;
                OnPropertyChanged("ExpensesList");
            }
        }

        public RelayCommand RemoveTypeCommand
        {
            get
            {
                return _removeTypeCommand ?? (_removeTypeCommand = new RelayCommand((args) =>
                {
                    UpdateData();
                }));
            }
        }

        public bool IsRemoveIncomes
        {
            get => _isRemoveIncomes;
            set
            {
                _isRemoveIncomes = value;
                OnPropertyChanged("IsRemoveIncomes");
            }
        }
        public bool IsRemoveExpenses
        {
            get => _isRemoveExpenses;
            set
            {
                _isRemoveExpenses = value;
                OnPropertyChanged("IsRemoveExpenses");
            }
        }

        public SeriesCollection SeriesGeneral
        {
            get => _seriesGeneral;
            set
            {
                _seriesGeneral = value;
                OnPropertyChanged("SeriesGeneral");
            }
        }
        public SeriesCollection SeriesIncomes
        {
            get => _seriesIncomes;
            set
            {
                _seriesIncomes = value;
                OnPropertyChanged("SeriesIncomes");
            }
        }
        public SeriesCollection SeriesExpenses
        {
            get => _seriesExpenses;
            set
            {
                _seriesExpenses = value;
                OnPropertyChanged("SeriesExpenses");
            }
        }

        public GeneralViewModel()
        {
            _database = new DatabaseContext();

            Income_Types = _database.Income_Types.ToList();
            Expense_Types = _database.Expense_Types.ToList();
            Incomes = _database.Incomes.ToList();
            Expenses = _database.Expenses.ToList();
            Accounts = _database.Accounts.ToList();

            CurrentMonth = MonthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);

            FillYearsList();
        }

        public void UpdateData()
        {
            _minIncomesDate = GetMinDate(true);
            _minExpensesDate = GetMinDate(false);

            if (CurrentYear == "Все годы" && CurrentMonth != "Полный год")
            {
                CurrentMonth = "Полный год";
            }

            if (IsRemoveExpenses && !IsRemoveIncomes)
            {
                FillIncomesList();
                ExpensesList = new List<ListItem>();
            }
            else if (IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillExpensesList();
                IncomesList = new List<ListItem>();
            }
            else if (!IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillIncomesList();
                FillExpensesList();
            }
            else
            {
                IncomesList = new List<ListItem>();
                ExpensesList = new List<ListItem>();
            }

            FillPieChartData();
        }

        private void FillYearsList()
        {
            YearsList = new List<string>
            {
                _currentYear
            };

            foreach (var item in Incomes)
            {
                //try to get year from date (14.05.2022 -> 2022)
                string tempYear = item.Date.Split('.')[2];

                if (!YearsList.Contains(tempYear))
                {
                    YearsList.Add(tempYear);
                }
            }

            foreach (var item in Expenses)
            {
                //try to get year from date (14.05.2022 -> 2022)
                string tempYear = item.Date.Split('.')[2];

                if (!YearsList.Contains(tempYear))
                {
                    YearsList.Add(tempYear);
                }
            }

            YearsList.Sort();
            YearsList.Add("Все годы");
        }

        private void FillIncomesList()
        {
            var query = from income in Incomes
                        group income by income.Income_Type_Id into incomeListItem
                        select new
                        {
                            TypeId = incomeListItem.Key,
                            TotalAmount = (from item in incomeListItem
                                           where item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                           || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                           || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                                           || CurrentYear == "Все годы"
                                           select item.Total_Amount).Sum()
                        };

            List<ListItem> tempList = new List<ListItem>();

            int totalSum = 0;

            foreach (var item in query)
            {
                if (item.TotalAmount != 0)
                {
                    tempList.Add(new ListItem()
                    {
                        TypeName = Income_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                        TotalAmount = ConvertToCurrencyFormat(item.TotalAmount)
                    });

                    totalSum += item.TotalAmount;
                }
            }

            tempList.Add(new ListItem()
            {
                TypeName = "Итого",
                TotalAmount = ConvertToCurrencyFormat(totalSum)
            });

            _incomesForecast = CalculateForecast(true);

            tempList.Add(new ListItem()
            {
                TypeName = "Прогноз",
                TotalAmount = ConvertToCurrencyFormat(_incomesForecast)
            });

            tempList.Add(new ListItem()
            {
                TypeName = "Разница",
                TotalAmount = ConvertToCurrencyFormat(Math.Abs(totalSum - _incomesForecast))
            });

            IncomesList = tempList;
        }

        private void FillExpensesList()
        {
            var query = from expense in Expenses
                        group expense by expense.Expense_Type_Id into expenseListItem
                        select new
                        {
                            TypeId = expenseListItem.Key,
                            TotalAmount = (from item in expenseListItem
                                           where item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                           || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                           || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                                           || CurrentYear == "Все годы"
                                           select item.Total_Price).Sum()
                        };

            List<ListItem> tempList = new List<ListItem>();

            int totalSum = 0;

            foreach (var item in query)
            {
                if (item.TotalAmount != 0)
                {
                    tempList.Add(new ListItem()
                    {
                        TypeName = Expense_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                        TotalAmount = ConvertToCurrencyFormat(item.TotalAmount)
                    });

                    totalSum += item.TotalAmount;
                }
            }

            tempList.Add(new ListItem()
            {
                TypeName = "Итого",
                TotalAmount = ConvertToCurrencyFormat(totalSum)
            });

            _expensesForecast = CalculateForecast(false);

            tempList.Add(new ListItem()
            {
                TypeName = "Прогноз",
                TotalAmount = ConvertToCurrencyFormat(_expensesForecast)
            });

            tempList.Add(new ListItem()
            {
                TypeName = "Разница",
                TotalAmount = ConvertToCurrencyFormat(Math.Abs(totalSum - _expensesForecast))
            });

            ExpensesList = tempList;
        }

        private int CalculateForecast(bool isIncomeForecast)
        {
            int timeDiff = 0;
            int tempTotalAmount = 0;

            if (CurrentMonth != "Полный год")
            {
                if (isIncomeForecast)
                {
                    timeDiff = (Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minIncomesDate.Split('.')[2])) * 12
                    + MonthsList.IndexOf(CurrentMonth) + 1 - Convert.ToInt32(_minIncomesDate.Split('.')[1]);

                    tempTotalAmount = (from income in Incomes
                                       where FindLessDate(income.Date, $"01.{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                       select income.Total_Amount).Sum();
                }
                else
                {
                    timeDiff = (Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minExpensesDate.Split('.')[2])) * 12
                    + MonthsList.IndexOf(CurrentMonth) + 1 - Convert.ToInt32(_minExpensesDate.Split('.')[1]);

                    tempTotalAmount = (from expense in Expenses
                                       where FindLessDate(expense.Date, $"01.{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                       select expense.Total_Price).Sum();
                }
            }
            else
            {
                if (CurrentYear != "Все годы")
                {
                    if (isIncomeForecast)
                    {
                        timeDiff = Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minIncomesDate.Split('.')[2]);

                        tempTotalAmount = (from income in Incomes
                                           where FindLessDate(income.Date, $"01.01.{CurrentYear}")
                                           select income.Total_Amount).Sum();
                    }
                    else
                    {
                        timeDiff = Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minExpensesDate.Split('.')[2]);

                        tempTotalAmount = (from expense in Expenses
                                           where FindLessDate(expense.Date, $"01.01.{CurrentYear}")
                                           select expense.Total_Price).Sum();
                    }
                }
                else
                {
                    timeDiff = 0;
                    tempTotalAmount = 0;
                }
            }

            if (tempTotalAmount == 0 || timeDiff == 0)
            {
                return tempTotalAmount;
            }

            return (int)((double)tempTotalAmount / (double)timeDiff);
        }

        private string GetMinDate(bool isIncomesList)
        {
            string minDate = "99.99.9999";

            IEnumerable<string> tempDates = new List<string>();

            if (isIncomesList)
            {
                tempDates = from income in Incomes
                            select income.Date;
            }
            else
            {
                tempDates = from expense in Expenses
                            select expense.Date;
            }

            foreach (var date in tempDates)
            {
                if (FindLessDate(date, minDate))
                {
                    minDate = date;
                }
            }

            return minDate;
        }

        private bool FindLessDate(string firstDate, string secondDate)
        {
            if (Convert.ToInt32(firstDate.Split('.')[2]) < Convert.ToInt32(secondDate.Split('.')[2])) //year
            {
                return true;
            }
            else if (Convert.ToInt32(firstDate.Split('.')[2]) == Convert.ToInt32(secondDate.Split('.')[2])) //year
            {
                if (Convert.ToInt32(firstDate.Split('.')[1]) < Convert.ToInt32(secondDate.Split('.')[1])) //month
                {
                    return true;
                }
                else if (Convert.ToInt32(firstDate.Split('.')[1]) == Convert.ToInt32(secondDate.Split('.')[1])) //month
                {
                    if (Convert.ToInt32(firstDate.Split('.')[0]) < Convert.ToInt32(secondDate.Split('.')[0])) //day
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void FillPieChartData()
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            provider.NumberDecimalDigits = 2;

            SeriesCollection tempCollectionIncomes = new SeriesCollection();
            SeriesCollection tempCollectionExpenses = new SeriesCollection();
            SeriesCollection tempCollectionGeneral = new SeriesCollection();

            if (IncomesList.Count() > 3)
            {
                for (int i = 0; i < IncomesList.Count() - 3; i++)
                {
                    tempCollectionGeneral.Add(
                        new PieSeries
                        {
                            Title = IncomesList.ToList()[i].TypeName,
                            Values = new ChartValues<ObservableValue>
                            {
                            new ObservableValue(Convert.ToDouble(IncomesList.ToList()[i].TotalAmount, provider))
                            },
                            DataLabels = false
                        });
                }

                tempCollectionIncomes.Add(
                    new PieSeries
                    {
                        Title = "Текущий доход",
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(Convert.ToDouble(IncomesList.ToList()[IncomesList.Count() - 3].TotalAmount, provider))
                        },
                        DataLabels = false
                    });
                tempCollectionIncomes.Add(
                    new PieSeries
                    {
                        Title = "Прогноз",
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(Convert.ToDouble(ConvertToCurrencyFormat(_incomesForecast), provider))
                        },
                        DataLabels = false
                    });
            }
            else
            {
                tempCollectionIncomes.Add(
                        new PieSeries
                        {
                            Title = "Нет доходов",
                            Values = new ChartValues<ObservableValue>
                            {
                            new ObservableValue(100)
                            },
                            DataLabels = false,
                            Fill = new SolidColorBrush(Colors.Gray)
                        });
            }

            if (ExpensesList.Count() > 3)
            {
                for (int i = 0; i < ExpensesList.Count() - 3; i++)
                {
                    tempCollectionGeneral.Add(
                        new PieSeries
                        {
                            Title = ExpensesList.ToList()[i].TypeName,
                            Values = new ChartValues<ObservableValue>
                            {
                            new ObservableValue(Convert.ToDouble(ExpensesList.ToList()[i].TotalAmount, provider))
                            },
                            DataLabels = false
                        });
                }

                tempCollectionExpenses.Add(
                    new PieSeries
                    {
                        Title = "Текущий расход",
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(Convert.ToDouble(ExpensesList.ToList()[ExpensesList.Count() - 3].TotalAmount, provider))
                        },
                        DataLabels = false
                    });

                tempCollectionExpenses.Add(
                    new PieSeries
                    {
                        Title = "Прогноз",
                        Values = new ChartValues<ObservableValue>
                        {
                            new ObservableValue(Convert.ToDouble(ConvertToCurrencyFormat(_expensesForecast), provider))
                        },
                        DataLabels = false
                    });
            }
            else
            {
                tempCollectionExpenses.Add(
                        new PieSeries
                        {
                            Title = "Нет расходов",
                            Values = new ChartValues<ObservableValue>
                            {
                            new ObservableValue(100)
                            },
                            DataLabels = false,
                            Fill = new SolidColorBrush(Colors.Gray)
                        });
            }

            if (ExpensesList.Count() <= 3 && IncomesList.Count() <= 3)
            {
                tempCollectionGeneral.Add(
                        new PieSeries
                        {
                            Title = "Нет расходов\n и доходов",
                            Values = new ChartValues<ObservableValue>
                            {
                            new ObservableValue(100)
                            },
                            DataLabels = false,
                            Fill = new SolidColorBrush(Colors.Gray)
                        });
            }

            SeriesIncomes = tempCollectionIncomes;
            SeriesExpenses = tempCollectionExpenses;
            SeriesGeneral = tempCollectionGeneral;
        }

        private string ConvertToCurrencyFormat(int value)
        {
            if (value == 0)
            {
                return value.ToString().Insert(1, ".00");
            }
            else
            {
                return value.ToString().Insert(value.ToString().Length - 2, ".");
            }
        }
    }
}
