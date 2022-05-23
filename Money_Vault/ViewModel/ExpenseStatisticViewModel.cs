using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class ExpenseStatisticViewModel : BaseViewModel
    {
        private string _buttonContentCurrentMode;

        public string ButtonContentCurrentMode
        {
            get => _buttonContentCurrentMode;
            set
            {
                _buttonContentCurrentMode = value;
                OnPropertyChanged("ButtonContentCurrentMode");
            }
        }

        private bool _isEnableComboBoxMonth;

        private IEnumerable<TotalListItem> _expensesList;
        private SeriesCollection _seriesGeneral;

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

        public IEnumerable<TotalListItem> ExpensesList
        {
            get => _expensesList;
            set
            {
                _expensesList = value;
                OnPropertyChanged("ExpensesList");
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

        public ExpenseStatisticViewModel()
        {
            if (Convert.ToBoolean(Settings.Default["currentExpenseMode"]))
            {
                ButtonContentCurrentMode = "Сменить\nрежим на\nКраткий";
            }
            else
            {
                ButtonContentCurrentMode = "Сменить\nрежим на\nПолный";
            }

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

            FillExpensesTotalList();
            FillPieChartData();
        }

        private void FillYearsList()
        {
            YearsList = new List<string>();

            using (DatabaseContext database = new DatabaseContext())
            {
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

        private void FillExpensesTotalList()
        {
            List<TotalListItem> tempList = new List<TotalListItem>();
            int totalSum = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                List<Expense> expenses = (from item in database.Expenses.ToList()
                                          where item.User_Id == Convert.ToInt32(Settings.Default["currentUserId"])
                                          select item).ToList();

                if (expenses.Count != 0)
                {
                    var query = from item in database.Expense_Items.ToList()
                                where expenses.Find(x => x.Id == item.Expense_Id).Date.Contains($".{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                                   || expenses.Find(x => x.Id == item.Expense_Id).Date.Contains($".0{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                                   || (CurrentMonth == "Полный год" && expenses.Find(x => x.Id == item.Expense_Id).Date.Contains($".{CurrentYear}"))
                                                   || CurrentYear == "Все годы"
                                group item by item.Expense_Subtype_Id into expenseListItem
                                select new
                                {
                                    SubtypeId = expenseListItem.Key,
                                    TotalAmount = (from item in expenseListItem
                                                   select item.Price).Sum()
                                };

                    foreach (var item in query)
                    {
                        if (item.TotalAmount != 0)
                        {
                            tempList.Add(new TotalListItem()
                            {
                                TypeName = database.Expense_Subtypes.ToList().Find(x => x.Id == item.SubtypeId).Name,
                                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(item.TotalAmount)
                            });

                            totalSum += item.TotalAmount;
                        }
                    }
                }
            }

            tempList.Add(new TotalListItem()
            {
                TypeName = "Итого",
                TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(totalSum)
            });

            ExpensesList = tempList;
        }

        private void FillPieChartData()
        {
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

            SeriesCollection tempCollectionGeneral = new SeriesCollection();

            if (ExpensesList.Count() > 1)
            {
                for (int i = 0; i < ExpensesList.Count() - 1; i++)
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
            }

            if (ExpensesList.Count() == 1)
            {
                tempCollectionGeneral.Add(
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

            SeriesGeneral = tempCollectionGeneral;
        }
    }
}
