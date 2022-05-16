using LiveCharts;
using LiveCharts.Wpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class PlanningAccountViewModel : BaseViewModel
    {
        private RelayCommand _showAddOperationFrameCommand;
        private RelayCommand _showEditOperationFrameCommand;
        private RelayCommand _showDeleteOperationFrameCommand;

        private RelayCommand _showAddAccountFrameCommand;
        private RelayCommand _showEditAccountFrameCommand;
        private RelayCommand _showDeleteAccountFrameCommand;

        private IEnumerable<Account> _accountsList;
        private Account _selectedAccount;

        private IEnumerable<AccountOperationCommonListItem> _operationsList;
        private AccountOperationCommonListItem _selectedOperation;

        private string _currentNumberLabel;
        private double _initialAmount;
        private double _currentAmount;
        private double _currentForecastMonth;
        private double _currentForecastYear;

        private SeriesCollection _operationsSeries;
        private string[] _datesLabels;

        public IEnumerable<Account> AccountsList
        {
            get => _accountsList;
            set
            {
                _accountsList = value;
                OnPropertyChanged("AccountsList");
            }
        }

        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged("SelectedAccount");

                FillAccountData();
            }
        }

        public IEnumerable<AccountOperationCommonListItem> OperationsList
        {
            get => _operationsList;
            set
            {
                _operationsList = value;
                OnPropertyChanged("OperationsList");
            }
        }

        public AccountOperationCommonListItem SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                OnPropertyChanged("SelectedOperation");
            }
        }

        public double CurrentForecastMonth
        {
            get => _currentForecastMonth;
            set
            {
                _currentForecastMonth = value;
                OnPropertyChanged("CurrentForecastMonth");
            }
        }

        public double CurrentForecastYear
        {
            get => _currentForecastYear;
            set
            {
                _currentForecastYear = value;
                OnPropertyChanged("CurrentForecastYear");
            }
        }

        public double InitialAmount
        {
            get => _initialAmount;
            set
            {
                _initialAmount = value;
                OnPropertyChanged("InitialAmount");
            }
        }

        public double CurrentAmount
        {
            get => _currentAmount;
            set
            {
                _currentAmount = value;
                OnPropertyChanged("CurrentAmount");
            }
        }

        public string CurrentNumberLabel
        {
            get => _currentNumberLabel;
            set
            {
                _currentNumberLabel = value;
                OnPropertyChanged("CurrentNumberLabel");
            }
        }

        public SeriesCollection OperationsSeries
        {
            get => _operationsSeries;
            set
            {
                _operationsSeries = value;
                OnPropertyChanged("OperationsSeries");
            }
        }

        public string[] DatesLabels
        {
            get => _datesLabels;
            set
            {
                _datesLabels = value;
                OnPropertyChanged("DatesLabels");
            }
        }

        public RelayCommand ShowAddOperationFrameCommand
        {
            get
            {
                return _showAddOperationFrameCommand ?? (_showAddOperationFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                        var planningAccountsOperationAddViewModel = new PlanningAccountsOperationAddViewModel(SelectedAccount.Id);
                        await _displayRootRegistry.ShowModalPresentation(planningAccountsOperationAddViewModel);

                        FillAccountData();
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали счёт!");
                    }
                }));
            }
        }

        public RelayCommand ShowEditOperationFrameCommand
        {
            get
            {
                return _showEditOperationFrameCommand ?? (_showEditOperationFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        if (SelectedOperation != null)
                        {
                            var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                            var planningAccountsOperationEditViewModel = new PlanningAccountsOperationEditViewModel(
                                SelectedOperation.Id,
                                SelectedOperation.Type,
                                SelectedOperation.Amount.ToString(),
                                SelectedOperation.Date);

                            await _displayRootRegistry.ShowModalPresentation(planningAccountsOperationEditViewModel);

                            FillAccountData();
                        }
                        else
                        {
                            await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали операцию для редактирования.");
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали счёт!");
                    }
                }));
            }
        }

        public RelayCommand ShowDeleteOperationFrameCommand
        {
            get
            {
                return _showDeleteOperationFrameCommand ?? (_showDeleteOperationFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        if (SelectedOperation != null)
                        {
                            var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                            var messageViewModel = new MessageViewModel(
                                                    "Внимание",
                                                    "Вы действительно хотите удалить данную операцию?");

                            await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                            if (messageViewModel.Result)
                            {
                                using (DatabaseContext database = new DatabaseContext())
                                {
                                    Account_Operation item = database.Account_Operations.ToList().Find(x => x.Id == SelectedOperation.Id);
                                    database.Account_Operations.Remove(item);
                                    database.SaveChanges();
                                }
                                FillAccountData();
                            }
                        }
                        else
                        {
                            await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали операцию для удаления.");
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали счёт!");
                    }
                }));
            }
        }

        public RelayCommand ShowAddAccountFrameCommand
        {
            get
            {
                return _showAddAccountFrameCommand ?? (_showAddAccountFrameCommand = new RelayCommand(async (args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var planningAccountAddViewModel = new PlanningAccountAddViewModel();
                    await _displayRootRegistry.ShowModalPresentation(planningAccountAddViewModel);

                    UpdateData();
                }));
            }
        }

        public RelayCommand ShowEditAccountFrameCommand
        {
            get
            {
                return _showEditAccountFrameCommand ?? (_showEditAccountFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        int lastSelectedId = SelectedAccount.Id;

                        var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var planningAccountEditViewModel = new PlanningAccountEditViewModel(
                            SelectedAccount.Id,
                            SelectedAccount.Name,
                            SelectedAccount.Number,
                            SelectedAccount.Current_Amount);

                        await _displayRootRegistry.ShowModalPresentation(planningAccountEditViewModel);

                        UpdateData();

                        using (DatabaseContext database = new DatabaseContext())
                        {
                            SelectedAccount = database.Accounts.ToList().Find(x => x.Id == lastSelectedId);
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали счёт для редактирования.");
                    }
                }));
            }
        }

        public RelayCommand ShowDeleteAccountFrameCommand
        {
            get
            {
                return _showDeleteAccountFrameCommand ?? (_showDeleteAccountFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var messageViewModel = new MessageViewModel(
                                                "Внимание",
                                                "Вы действительно хотите удалить данный счёт?");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        if (messageViewModel.Result)
                        {
                            using (DatabaseContext database = new DatabaseContext())
                            {
                                Account item = database.Accounts.ToList().Find(x => x.Id == SelectedAccount.Id);
                                database.Accounts.Remove(item);
                                database.SaveChanges();
                            }
                            UpdateData();
                            SelectedAccount = null;
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали счёт для удаления.");
                    }
                }));
            }
        }

        public PlanningAccountViewModel()
        {
            UpdateData();
            FillAccountData();
        }

        private void UpdateData()
        {
            int currentUserId = Convert.ToInt32(Settings.Default["currentUserId"]);

            using (DatabaseContext database = new DatabaseContext())
            {
                AccountsList = from item in database.Accounts.ToList()
                               where item.User_Id == currentUserId
                               select item;
            }
        }

        private void FillAccountData()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (SelectedAccount != null)
                {
                    List<AccountOperationCommonListItem> accountOperationsTempList = new List<AccountOperationCommonListItem>();
                    int currentAmount = SelectedAccount.Current_Amount;

                    int index = 1;
                    foreach (var item in database.Account_Operations.ToList())
                    {
                        if (item.Account_Id == SelectedAccount.Id)
                        {
                            accountOperationsTempList.Add(new AccountOperationCommonListItem()
                            {
                                Number = index,
                                Id = item.Id,
                                Account_Id = item.Account_Id,
                                Amount = AdditionalFunctions.ConvertToCurrencyFormat(item.Amount),
                                Type = item.Type,
                                Date = item.Date
                            });

                            if (item.Type == "Снятие")
                            {
                                currentAmount -= item.Amount;
                            }
                            else
                            {
                                currentAmount += item.Amount;
                            }
                            index++;
                        }
                    }

                    InitialAmount = AdditionalFunctions.ConvertToCurrencyFormat(SelectedAccount.Current_Amount);
                    OperationsList = accountOperationsTempList;
                    CurrentNumberLabel = "Текущее состояние счёта №" + SelectedAccount.Number;
                    CurrentAmount = AdditionalFunctions.ConvertToCurrencyFormat(currentAmount);
                    CalculateForecast();
                    FillChartData();
                }
                else
                {
                    OperationsList = new List<AccountOperationCommonListItem>();
                    CurrentNumberLabel = "Выберите счёт...";
                    CurrentAmount = 0;
                    CurrentForecastMonth = 0;
                    CurrentForecastYear = 0;

                    ChartValues<double> chartValues = new ChartValues<double> { 0D };
                    SeriesCollection tempSeries = new SeriesCollection();
                    tempSeries.Add(new LineSeries
                    {
                        Title = "Выберите счёт",
                        Values = chartValues,
                        PointGeometrySize = 14,
                        PointForeground = Brushes.Green,
                        StrokeThickness = 3,
                        FontFamily = new FontFamily("Balsamiq Sans"),
                        FontSize = 20
                    });

                    OperationsSeries = tempSeries;
                    DatesLabels = new string[] { "" };
                }
            }
        }

        private void CalculateForecast()
        {
            int averageAmount = 0;
            int timeDiff = (DateTime.Now.Year - Convert.ToInt32(GetMinOperationsDate().Split('.')[2])) * 12
                        + DateTime.Now.Month - Convert.ToInt32(GetMinOperationsDate().Split('.')[1]);

            if (timeDiff != 0)
            {
                using (DatabaseContext database = new DatabaseContext())
                {
                    foreach (var item in database.Account_Operations.ToList())
                    {
                        if (item.Account_Id == SelectedAccount.Id && AdditionalFunctions.FindLessDate(item.Date, DateTime.Now.ToString("dd.MM.yyyy")))
                        {
                            if (item.Type == "Снятие")
                            {
                                averageAmount -= item.Amount;
                            }
                            else
                            {
                                averageAmount += item.Amount;
                            }
                        }
                    }
                }

                CurrentForecastMonth = CurrentAmount
                    + AdditionalFunctions.ConvertToCurrencyFormat(averageAmount) / timeDiff;

                CurrentForecastYear = CurrentAmount
                    + (AdditionalFunctions.ConvertToCurrencyFormat(averageAmount) / timeDiff) * (12 - DateTime.Now.Month + 1);
            }
            else
            {
                CurrentForecastMonth = 0;

                CurrentForecastYear = 0;
            }
        }

        private string GetMinOperationsDate()
        {
            string minDate = "99.99.9999";

            IEnumerable<string> tempDates = new List<string>();

            using (DatabaseContext database = new DatabaseContext())
            {
                tempDates = from item in database.Account_Operations.ToList()
                            where item.Account_Id == SelectedAccount.Id
                            select item.Date;
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

        private void FillChartData()
        {
            double currentSum = InitialAmount;

            SeriesCollection tempCollection = new SeriesCollection();
            List<string> tempDates = new List<string>
            {
                ""
            };

            ChartValues<double> chartValues = new ChartValues<double>
            {
                currentSum
            };

            using (DatabaseContext database = new DatabaseContext())
            {
                foreach (var item in database.Account_Operations.ToList())
                {
                    if (item.Account_Id == SelectedAccount.Id)
                    {
                        if (item.Type == "Снятие")
                        {
                            currentSum -= AdditionalFunctions.ConvertToCurrencyFormat(item.Amount);
                        }
                        else
                        {
                            currentSum += AdditionalFunctions.ConvertToCurrencyFormat(item.Amount);
                        }

                        chartValues.Add(currentSum);
                        tempDates.Add(item.Date);
                    }
                }
            }

            tempCollection.Add(
                new LineSeries
                {
                    Title = "Текущая сумма",
                    Values = chartValues,
                    PointGeometrySize = 14,
                    PointForeground = Brushes.Green,
                    StrokeThickness = 3,
                    FontFamily = new FontFamily("Balsamiq Sans"),
                    FontSize = 20,
                });

            OperationsSeries = tempCollection;
            DatesLabels = tempDates.ToArray();
        }
    }
}
