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
    public class GeneralViewModel : BaseViewModel
    {
        private DatabaseContext _database;

        private RelayCommand _removeTypeCommand;

        private bool isRemoveIncomes;
        private bool isRemoveExpenses;

        private IEnumerable<Income_Type> _income_Types;
        private IEnumerable<Expense_Type> _expense_Types;
        private IEnumerable<Income> _incomes;
        private IEnumerable<Expense> _expenses;
        private IEnumerable<Account> _accounts;
        private IEnumerable<ListItem> _incomesList;
        private IEnumerable<ListItem> _expensesList;
        private IEnumerable<ListItem> _typesList;

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

                UpdateData();
            }
        }

        public string CurrentMonth
        {
            get => _currentMonth;
            set
            {
                _currentMonth = value;
                OnPropertyChanged("CurrentMonth");

                UpdateData();
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

        public IEnumerable<ListItem> TypesList
        {
            get => _typesList;
            set
            {
                _typesList = value;
                OnPropertyChanged("TypesList");
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
            get => isRemoveIncomes;
            set
            {
                isRemoveIncomes = value;
                OnPropertyChanged("IsRemoveIncomes");
            }
        }
        public bool IsRemoveExpenses
        {
            get => isRemoveExpenses;
            set
            {
                isRemoveExpenses = value;
                OnPropertyChanged("IsRemoveExpenses");
            }
        }

        public void UpdateData()
        {
            if (CurrentYear == "Все года" && CurrentMonth != "Полный год")
            {
                CurrentMonth = "Полный год";
            }

            if (IsRemoveExpenses && !IsRemoveIncomes)
            {
                FillIncomesList();
                FillTypesList();
                ExpensesList = new List<ListItem>();
            }
            else if (IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillExpensesList();
                FillTypesList();
                IncomesList = new List<ListItem>();
            }
            else if (!IsRemoveIncomes && !IsRemoveExpenses)
            {
                FillIncomesList();
                FillExpensesList();
                FillTypesList();
            }
            else
            {
                IncomesList = new List<ListItem>();
                ExpensesList = new List<ListItem>();
                FillTypesList();
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

            FillTypesList();
            FillYearsList();

            CurrentMonth = MonthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);

            UpdateData();
        }

        private void FillTypesList()
        {
            List<ListItem> tempList = new List<ListItem>();

            if (!IsRemoveIncomes)
            {
                foreach (var item in Income_Types)
                {
                    tempList.Add(new ListItem() { TypeName = item.Name, TotalAmount = "0" });
                }
            }

            if (!IsRemoveExpenses)
            {
                foreach (var item in Expense_Types)
                {
                    tempList.Add(new ListItem() { TypeName = item.Name, TotalAmount = "0" });
                }
            }

            TypesList = tempList;
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
            YearsList.Add("Все года");
        }

        private void FillIncomesList()
        {
            var query = from income in Incomes
                        group income by income.Income_Type_Id into incomeListItem
                        select new
                        {
                            TypeId = incomeListItem.Key,
                            TotalAmount = (from item in incomeListItem
                                           where item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth)}.{CurrentYear}")
                                           || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth)}.{CurrentYear}")
                                           || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                                           || CurrentYear == "Все года"
                                           select item.Total_Amount).Sum()
                        };

            List<ListItem> tempList = new List<ListItem>();

            int totalSum = 0;
            int forecast = 0;

            foreach (var item in query)
            {
                tempList.Add(new ListItem()
                {
                    TypeName = Income_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                    TotalAmount = item.TotalAmount.ToString()
                });

                totalSum += item.TotalAmount;
            }

            tempList.Add(new ListItem()
            {
                TypeName = "Итого",
                TotalAmount = Convert.ToString(totalSum)
            });

            tempList.Add(new ListItem()
            {
                TypeName = "Прогноз",
                TotalAmount = Convert.ToString(forecast)
            });

            tempList.Add(new ListItem()
            {
                TypeName = "Разница",
                TotalAmount = Convert.ToString(totalSum - forecast)
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
                                           where item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth)}.{CurrentYear}")
                                           || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth)}.{CurrentYear}")
                                           || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                                           || CurrentYear == "Все года"
                                           select item.Total_Price).Sum()
                        };

            List<ListItem> tempList = new List<ListItem>();

            int totalSum = 0;
            int forecast = 0;

            foreach (var item in query)
            {
                tempList.Add(new ListItem()
                {
                    TypeName = Expense_Types.ToList().Find(x => x.Id == item.TypeId).Name,
                    TotalAmount = item.TotalAmount.ToString()
                });
            }

            tempList.Add(new ListItem()
            {
                TypeName = "Итого",
                TotalAmount = Convert.ToString(totalSum)
            });

            tempList.Add(new ListItem()
            {
                TypeName = "Прогноз",
                TotalAmount = Convert.ToString(forecast)
            });

            tempList.Add(new ListItem()
            {
                TypeName = "Разница",
                TotalAmount = Convert.ToString(totalSum - forecast)
            });

            ExpensesList = tempList;
        }
    }
}
