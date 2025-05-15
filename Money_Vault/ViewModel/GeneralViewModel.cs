using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Money_Vault.Database;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class GeneralViewModel : BaseViewModel
    {
        private RelayCommand _removeTypeCommand;

        private bool _isRemoveIncomes;
        private bool _isRemoveExpenses;
        private bool _isEnableComboBoxMonth;

        private IEnumerable<TotalListItem> _incomesList;
        private IEnumerable<TotalListItem> _expensesList;
        private SeriesCollection _seriesGeneral = new SeriesCollection();

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
                ExpensesList = Enumerable.Empty<TotalListItem>();
            }
            else if (IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillExpensesTotalList();
                IncomesList = Enumerable.Empty<TotalListItem>();
            }
            else if (!IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillIncomesTotalList();
                FillExpensesTotalList();
            }
            else
            {
                IncomesList = Enumerable.Empty<TotalListItem>();
                ExpensesList = Enumerable.Empty<TotalListItem>();
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
            IncomesList = Enumerable.Empty<TotalListItem>();
            var incomesList = new List<TotalListItem>();
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
                        incomesList.Add(new TotalListItem()
                        {
                            TypeName = database.Income_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                            TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(item.TotalAmount)
                        });

                        totalSum += item.TotalAmount;
                    }
                }
            }

            incomesList.Sort();
            incomesList.Add(new TotalListItem()
            {
                TypeName = "Итого",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(totalSum)
            });
            IncomesList = incomesList;
        }

        private void FillExpensesTotalList()
        {
            ExpensesList = Enumerable.Empty<TotalListItem>();
            var expensesList = new List<TotalListItem>();
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
                        expensesList.Add(new TotalListItem()
                        {
                            TypeName = database.Expense_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                            TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(item.TotalAmount)
                        });

                        totalSum += item.TotalAmount;
                    }
                }
            }

            expensesList.Sort();
            expensesList.Add(new TotalListItem()
            {
                TypeName = "Итого",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(totalSum)
            });
            ExpensesList = expensesList;
        }

        private void FillPieChartData()
        {
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

            SeriesGeneral.Clear();

            if (IncomesList.Count() > 1)
            {
                for (int i = 0; i < IncomesList.Count() - 1; i++)
                {
                    SeriesGeneral.Add(
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
            }

            if (ExpensesList.Count() > 1)
            {
                for (int i = 0; i < ExpensesList.Count() - 1; i++)
                {
                    SeriesGeneral.Add(
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
            }

            if (ExpensesList.Count() <= 1 && IncomesList.Count() <= 1)
            {
                SeriesGeneral.Add(
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
        }
    }
}
