using Microsoft.Win32;
using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using System.Diagnostics;
using Money_Vault.Properties;

namespace Money_Vault.ViewModel
{
    public class ReportViewModel : BaseViewModel
    {
        private RelayCommand _createReportCommand;
        private RelayCommand _saveReportCommand;
        private RelayCommand _printReportCommand;

        private IEnumerable<IncomeCommonListItem> _incomeDataList;
        private IEnumerable<ExpenseCommonListItem> _expenseDataList;
        private IEnumerable<TotalListItem> _totalDataList;

        private Visibility _isVisibleIncomeSubMenu;
        private Visibility _isVisibleExpenseSubMenu;
        private Visibility _isVisibleDataGridIncome;
        private Visibility _isVisibleDataGridExpense;
        private Visibility _isVisibleDataGridTotal;
        private bool _isEnableComboBoxMonth;
        private bool _isEnableSaveButton;
        private bool _isEnablePrintButton;
        private string _buttonContentCurrentMode;

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

        private Microsoft.Office.Interop.Excel.Application _application;
        private Microsoft.Office.Interop.Excel.Workbook _workbook;
        private Microsoft.Office.Interop.Excel.Worksheet _worksheet;

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
                if (_currentOperationsType == "Расходы")
                {
                    ReportTypesList = new List<string>() { "Операции", "Категории", "Подкатегории" };
                }
                else
                {
                    ReportTypesList = new List<string>() { "Операции", "Категории" };
                }

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

        public Visibility IsVisibleIncomeSubMenu
        {
            get => _isVisibleIncomeSubMenu;
            set
            {
                _isVisibleIncomeSubMenu = value;
                OnPropertyChanged("IsVisibleIncomeSubMenu");
            }
        }

        public Visibility IsVisibleExpenseSubMenu
        {
            get => _isVisibleExpenseSubMenu;
            set
            {
                _isVisibleExpenseSubMenu = value;
                OnPropertyChanged("IsVisibleExpenseSubMenu");
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

        public bool IsEnableSaveButton
        {
            get => _isEnableSaveButton;
            set
            {
                _isEnableSaveButton = value;
                OnPropertyChanged("IsEnableSaveButton");
            }
        }

        public bool IsEnablePrintButton
        {
            get => _isEnablePrintButton;
            set
            {
                _isEnablePrintButton = value;
                OnPropertyChanged("IsEnablePrintButton");
            }
        }

        public string ButtonContentCurrentMode
        {
            get => _buttonContentCurrentMode;
            set
            {
                _buttonContentCurrentMode = value;
                OnPropertyChanged("ButtonContentCurrentMode");
            }
        }

        public RelayCommand CreateReportCommand
        {
            get
            {
                return _createReportCommand ?? (_createReportCommand = new RelayCommand((args) =>
                {
                    IsEnablePrintButton = true;
                    IsEnableSaveButton = true;

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
                    string path = "";
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "Файл Excel (*.xlsx)|*.xlsx|Все файлы (*.*)|*.*",
                        Title = "Сохранить отчёт как...",
                        AddExtension = true,
                        FileName = "Отчёт по " + CurrentOperationsType + " " + CurrentMonth + " " + CurrentYear
                    };

                    if (saveDialog.ShowDialog() == true)
                    {
                        path = saveDialog.FileName;
                        FillExcelFile();
                        _workbook.SaveAs(path);
                    }

                    if (_application != null)
                    {
                        _application.Quit();
                    }
                }));
            }
        }
        public RelayCommand PrintReportCommand
        {
            get
            {
                return _printReportCommand ?? (_printReportCommand = new RelayCommand((args) =>
                {
                    string tempPath = Path.GetTempPath() + "Отчёт.xlsx";
                    FillExcelFile();

                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }

                    _workbook.SaveAs(tempPath);

                    Process excel = new Process();
                    excel.StartInfo.FileName = tempPath;
                    excel.Start();

                    if (_application != null)
                    {
                        _application.Quit();
                    }
                }));
            }
        }

        public ReportViewModel()
        {
            CurrentMonth = _monthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);

            IsEnableSaveButton = false;
            IsEnablePrintButton = false;
            IsVisibleDataGridExpense = Visibility.Hidden;
            IsVisibleDataGridIncome = Visibility.Hidden;
            IsVisibleDataGridTotal = Visibility.Hidden;

            CurrentReportType = ReportTypesList[0];
            CurrentOperationsType = OperationsList[0];

            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
            {
                IsVisibleExpenseSubMenu = Visibility.Hidden;
                IsVisibleIncomeSubMenu = Visibility.Visible;
            }
            else
            {
                if (Convert.ToBoolean(Settings.Default["currentExpenseMode"]))
                {
                    ButtonContentCurrentMode = "Сменить\nрежим на\nКраткий";
                }
                else
                {
                    ButtonContentCurrentMode = "Сменить\nрежим на\nПолный";
                }
                IsVisibleExpenseSubMenu = Visibility.Visible;
                IsVisibleIncomeSubMenu = Visibility.Hidden;
            }
        }

        private void FillExcelFile()
        {
            _application = new Microsoft.Office.Interop.Excel.Application();
            _workbook = _application.Workbooks.Add();
            _worksheet = _workbook.Worksheets.get_Item(1);

            for (int i = 1; i < IncomeDataList.Count() + 1; i++)
            {
                _worksheet.Columns.ColumnWidth = 20;
            }

            if (IncomeDataList.Count() > 0)
            {
                _worksheet.Rows[1].Columns[1] = "Номер";
                _worksheet.Rows[1].Columns[2] = "Категория";
                _worksheet.Rows[1].Columns[3] = "Сумма (в руб)";
                _worksheet.Rows[1].Columns[4] = "Дата";
                _worksheet.Rows[1].Columns[5] = "Заметка";


                for (int i = 2; i < IncomeDataList.Count() + 2; i++)
                {
                    _worksheet.Rows[i].Columns[1] = IncomeDataList.ToList()[i - 2].Id;
                    _worksheet.Rows[i].Columns[2] = IncomeDataList.ToList()[i - 2].TypeName;
                    _worksheet.Rows[i].Columns[3] = IncomeDataList.ToList()[i - 2].Amount;
                    _worksheet.Rows[i].Columns[4] = IncomeDataList.ToList()[i - 2].Date;
                    _worksheet.Rows[i].Columns[5] = IncomeDataList.ToList()[i - 2].Note;
                }
            }
            else if (ExpenseDataList.Count() > 0)
            {
                _worksheet.Rows[1].Columns[1] = "Номер";
                _worksheet.Rows[1].Columns[2] = "Категория";
                _worksheet.Rows[1].Columns[3] = "Сумма (в руб)";
                _worksheet.Rows[1].Columns[4] = "Магазин";
                _worksheet.Rows[1].Columns[5] = "Дата";
                _worksheet.Rows[1].Columns[6] = "Заметка";

                for (int i = 2; i < ExpenseDataList.Count() + 2; i++)
                {
                    _worksheet.Rows[i].Columns[1] = ExpenseDataList.ToList()[i - 2].Id;
                    _worksheet.Rows[i].Columns[2] = ExpenseDataList.ToList()[i - 2].TypeName;
                    _worksheet.Rows[i].Columns[3] = ExpenseDataList.ToList()[i - 2].Amount;
                    _worksheet.Rows[i].Columns[4] = ExpenseDataList.ToList()[i - 2].ShopName;
                    _worksheet.Rows[i].Columns[5] = ExpenseDataList.ToList()[i - 2].Date;
                    _worksheet.Rows[i].Columns[6] = ExpenseDataList.ToList()[i - 2].Note;
                }
            }
            else
            {
                _worksheet.Rows[1].Columns[1] = "Категория";
                _worksheet.Rows[1].Columns[2] = "Сумма (в руб)";

                for (int i = 2; i < IncomeDataList.Count() + 2; i++)
                {
                    _worksheet.Rows[i].Columns[1] = TotalDataList.ToList()[i - 2].TypeName;
                    _worksheet.Rows[i].Columns[2] = TotalDataList.ToList()[i - 2].TotalAmount;
                }
            }
        }
        private void FillYearsList()
        {
            List<string> years = new List<string>();

            using (DatabaseContext database = new DatabaseContext())
            {
                switch (CurrentOperationsType)
                {
                    case "Доходы":
                        {
                            foreach (var item in database.Incomes.ToList())
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
                            foreach (var item in database.Expenses.ToList())
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

            using (DatabaseContext database = new DatabaseContext())
            {
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

                                        foreach (var item in database.Incomes.ToList())
                                        {
                                            if (item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                               || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                               || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                                               || CurrentYear == "Все годы")
                                            {
                                                listIncomes.Add(new IncomeCommonListItem()
                                                {
                                                    Id = item.Id,
                                                    TypeName = database.Income_Types.ToList().Find(x => x.Id == item.Income_Type_Id).Name,
                                                    Amount = AdditionalFunctions.ConvertToCurrencyFormat(item.Total_Amount),
                                                    Date = new DateTime(Convert.ToInt32(item.Date.Split('.')[2]), Convert.ToInt32(item.Date.Split('.')[1]), Convert.ToInt32(item.Date.Split('.')[0])),
                                                    Note = item.Note
                                                });
                                            }
                                        }
                                        break;
                                    }
                                case "Категории":
                                    {
                                        IsVisibleDataGridIncome = Visibility.Hidden;
                                        IsVisibleDataGridExpense = Visibility.Hidden;
                                        IsVisibleDataGridTotal = Visibility.Visible;

                                        listTotal = FillIncomesTotalList();
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

                                        foreach (var item in database.Expenses.ToList())
                                        {
                                            if (item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                               || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                                               || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                                               || CurrentYear == "Все годы")
                                            {
                                                listExpenses.Add(new ExpenseCommonListItem()
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
                                        break;
                                    }
                                case "Категории":
                                    {
                                        IsVisibleDataGridIncome = Visibility.Hidden;
                                        IsVisibleDataGridExpense = Visibility.Hidden;
                                        IsVisibleDataGridTotal = Visibility.Visible;

                                        listTotal = FillExpensesTotalList();
                                        break;
                                    }
                                case "Подкатегории":
                                    {
                                        IsVisibleDataGridIncome = Visibility.Hidden;
                                        IsVisibleDataGridExpense = Visibility.Hidden;
                                        IsVisibleDataGridTotal = Visibility.Visible;

                                        listTotal = FillExpenseSubtypesTotalList();
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
            }

            ExpenseDataList = listExpenses;
            IncomeDataList = listIncomes;
            TotalDataList = listTotal;
        }

        private List<TotalListItem> FillIncomesTotalList()
        {
            List<TotalListItem> tempList = new List<TotalListItem>();
            int totalSum = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                var query = from income in database.Incomes.ToList()
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

            return tempList;
        }

        private List<TotalListItem> FillExpensesTotalList()
        {
            List<TotalListItem> tempList = new List<TotalListItem>();
            int totalSum = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                var query = from expense in database.Expenses.ToList()
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

            return tempList;
        }

        private List<TotalListItem> FillExpenseSubtypesTotalList()
        {
            List<TotalListItem> tempList = new List<TotalListItem>();
            int totalSum = 0;

            using (DatabaseContext database = new DatabaseContext())
            {
                var subQuery = from item in database.Expenses.ToList()
                               where item.Date.Contains($".{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                               || item.Date.Contains($".0{MonthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                               || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                               || CurrentYear == "Все годы"
                               select item.Id;

                var query = from expense in database.Expense_Items.ToList()
                            where subQuery.Contains(expense.Expense_Id)
                            group expense by expense.Expense_Subtype_Id into expenseListItem
                            select new
                            {
                                TypeId = expenseListItem.Key,
                                TotalAmount = (from item in expenseListItem
                                               select item.Price).Sum()
                            };

                foreach (var item in query)
                {
                    if (item.TotalAmount != 0)
                    {
                        tempList.Add(new TotalListItem()
                        {
                            TypeName = database.Expense_Subtypes.ToList().Find(x => x.Id == item.TypeId).Name,
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

            return tempList;
        }
    }
}
