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
using Money_Vault.Properties;

namespace Money_Vault.ViewModel
{
    public class GeneralViewModel : BaseViewModel
    {
        private RelayCommand _removeTypeCommand;

        private bool _isRemoveIncomes;
        private bool _isRemoveExpenses;
        private bool _isEnableComboBoxMonth;

        private int _incomesForecast;
        private int _expensesForecast;
        private string _minIncomesDate;
        private string _minExpensesDate;

        private IEnumerable<TotalListItem> _incomesList;
        private IEnumerable<TotalListItem> _expensesList;
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

        public List<string> MonthsList
        {
            get => _monthsList;
            set
            {
                _monthsList = value;
                OnPropertyChanged("MonthList");
            }
        }

        public IEnumerable<TotalListItem> IncomesList
        {
            get => _incomesList;
            set
            {
                _incomesList = value;
                OnPropertyChanged("IncomesList");
            }
        }

        public IEnumerable<TotalListItem> ExpensesList
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

        public bool IsEnableComboBoxMonth
        {
            get => _isEnableComboBoxMonth;
            set
            {
                _isEnableComboBoxMonth = value;
                OnPropertyChanged("IsEnableComboBoxMonth");
            }
        }

        public GeneralViewModel()
        {
            CurrentMonth = MonthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);
            IsEnableComboBoxMonth = true;

            FillYearsList();
        }

        public void UpdateData()
        {
            _minIncomesDate = GetMinDate(true);
            _minExpensesDate = GetMinDate(false);

            if (CurrentYear == "Все годы")
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

            if (IsRemoveExpenses && !IsRemoveIncomes)
            {
                FillIncomesTotalList();
                ExpensesList = new List<TotalListItem>();
            }
            else if (IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillExpensesTotalList();
                IncomesList = new List<TotalListItem>();
            }
            else if (!IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillIncomesTotalList();
                FillExpensesTotalList();
            }
            else
            {
                IncomesList = new List<TotalListItem>();
                ExpensesList = new List<TotalListItem>();
            }

            FillPieChartData();
        }

        private void FillYearsList()
        {
            YearsList = new List<string>
            {
                _currentYear
            };

            using (DatabaseContext database = new DatabaseContext())
            {
                foreach (var item in database.Incomes.ToList())
                {
                    if (item.User_Id == Convert.ToInt32(Settings.Default["currentUserId"]))
                    {
                        //try to get year from date (14.05.2022 -> 2022)
                        string tempYear = item.Date.Split('.')[2];

                        if (!YearsList.Contains(tempYear))
                        {
                            YearsList.Add(tempYear);
                        }
                    }

                }

                foreach (var item in database.Expenses.ToList())
                {
                    if (item.User_Id == Convert.ToInt32(Settings.Default["currentUserId"]))
                    {
                        //try to get year from date (14.05.2022 -> 2022)
                        string tempYear = item.Date.Split('.')[2];

                        if (!YearsList.Contains(tempYear))
                        {
                            YearsList.Add(tempYear);
                        }
                    }
                }
            }


            YearsList.Sort();
            YearsList.Add("Все годы");
        }

        private void FillIncomesTotalList()
        {
            List<TotalListItem> tempList = new List<TotalListItem>();
            int totalSum = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                var query = from income in database.Incomes.ToList()
                            where income.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
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

                foreach (var item in query)
                {
                    if (item.TotalAmount != 0)
                    {
                        tempList.Add(new TotalListItem()
                        {
                            TypeName = database.Income_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                            TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(item.TotalAmount)
                        });

                        totalSum += item.TotalAmount;
                    }
                }
            }

            tempList.Add(new TotalListItem()
            {
                TypeName = "Итого",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(totalSum)
            });

            _incomesForecast = CalculateForecast(true);

            tempList.Add(new TotalListItem()
            {
                TypeName = "Прогноз",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(_incomesForecast)
            });

            tempList.Add(new TotalListItem()
            {
                TypeName = "Разница",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(Math.Abs(totalSum - _incomesForecast))
            });

            IncomesList = tempList;
        }

        private void FillExpensesTotalList()
        {
            List<TotalListItem> tempList = new List<TotalListItem>();
            int totalSum = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                var query = from expense in database.Expenses.ToList()
                            where expense.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
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

                foreach (var item in query)
                {
                    if (item.TotalAmount != 0)
                    {
                        tempList.Add(new TotalListItem()
                        {
                            TypeName = database.Expense_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                            TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(item.TotalAmount)
                        });

                        totalSum += item.TotalAmount;
                    }
                }
            }

            tempList.Add(new TotalListItem()
            {
                TypeName = "Итого",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(totalSum)
            });

            _expensesForecast = CalculateForecast(false);

            tempList.Add(new TotalListItem()
            {
                TypeName = "Прогноз",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(_expensesForecast)
            });

            tempList.Add(new TotalListItem()
            {
                TypeName = "Разница",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(Math.Abs(totalSum - _expensesForecast))
            });

            ExpensesList = tempList;
        }

        private int CalculateForecast(bool isIncomeForecast)
        {
            int timeDiff = 0;
            int tempTotalAmount = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                if (CurrentMonth != "Полный год")
                {
                    if (isIncomeForecast)
                    {
                        timeDiff = (Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minIncomesDate.Split('.')[2])) * 12
                        + MonthsList.IndexOf(CurrentMonth) + 1 - Convert.ToInt32(_minIncomesDate.Split('.')[1]);

                        tempTotalAmount = (from income in database.Incomes.ToList()
                                           where income.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                           && AdditionalFunctions.FindLessDate(income.Date, $"01.{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                           select income.Total_Amount).Sum();
                    }
                    else
                    {
                        timeDiff = (Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minExpensesDate.Split('.')[2])) * 12
                        + MonthsList.IndexOf(CurrentMonth) + 1 - Convert.ToInt32(_minExpensesDate.Split('.')[1]);

                        tempTotalAmount = (from expense in database.Expenses.ToList()
                                           where expense.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                           && AdditionalFunctions.FindLessDate(expense.Date, $"01.{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
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

                            tempTotalAmount = (from income in database.Incomes.ToList()
                                               where income.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                               && AdditionalFunctions.FindLessDate(income.Date, $"01.01.{CurrentYear}")
                                               select income.Total_Amount).Sum();
                        }
                        else
                        {
                            timeDiff = Convert.ToInt32(CurrentYear) - Convert.ToInt32(_minExpensesDate.Split('.')[2]);

                            tempTotalAmount = (from expense in database.Expenses.ToList()
                                               where expense.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                               && AdditionalFunctions.FindLessDate(expense.Date, $"01.01.{CurrentYear}")
                                               select expense.Total_Price).Sum();
                        }
                    }
                    else
                    {
                        timeDiff = 0;
                        tempTotalAmount = 0;
                    }
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

            using (DatabaseContext database = new DatabaseContext())
            {
                if (isIncomesList)
                {
                    tempDates = from income in database.Incomes.ToList()
                                where income.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                select income.Date;
                }
                else
                {
                    tempDates = from expense in database.Expenses.ToList()
                                where expense.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                select expense.Date;
                }
            }

            foreach (var date in tempDates)
            {
                if (AdditionalFunctions.FindLessDate(date, minDate))
                {
                    minDate = date;
                }
            }

            return minDate;
        }

        private void FillPieChartData()
        {
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

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
                            new ObservableValue(AdditionalFunctions.ConvertToCurrencyFormat(_incomesForecast))
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
                            new ObservableValue(AdditionalFunctions.ConvertToCurrencyFormat(_expensesForecast))
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
    }
}
