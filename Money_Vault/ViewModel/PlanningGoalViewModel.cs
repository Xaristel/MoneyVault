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
    public class PlanningGoalViewModel : BaseViewModel
    {
        private RelayCommand _showAddGoalFrameCommand;
        private RelayCommand _showEditGoalFrameCommand;
        private RelayCommand _showDeleteGoalFrameCommand;

        private IEnumerable<Goal> _goalsList;
        private Goal _selectedGoal;

        private double _requiredAmount;
        private double _currentAmount;
        private double _remainingAmount;
        private string _forecastTime;

        private SeriesCollection _operationsSeries;
        private string[] _datesLabels;

        public IEnumerable<Goal> GoalsList
        {
            get => _goalsList;
            set
            {
                _goalsList = value;
                OnPropertyChanged("GoalsList");
            }
        }

        public Goal SelectedGoal
        {
            get => _selectedGoal;
            set
            {
                _selectedGoal = value;
                OnPropertyChanged("SelectedGoal");

                FillGoalData();
            }
        }

        public double RemainingAmount
        {
            get => _remainingAmount;
            set
            {
                _remainingAmount = value;
                OnPropertyChanged("RemainingAmount");
            }
        }

        public string ForecastTime
        {
            get => _forecastTime;
            set
            {
                _forecastTime = value;
                OnPropertyChanged("ForecastTime");
            }
        }

        public double RequiredAmount
        {
            get => _requiredAmount;
            set
            {
                _requiredAmount = value;
                OnPropertyChanged("RequiredAmount");
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

        public RelayCommand ShowAddGoalFrameCommand
        {
            get
            {
                return _showAddGoalFrameCommand ?? (_showAddGoalFrameCommand = new RelayCommand(async (args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var planningAccountAddViewModel = new PlanningAccountAddViewModel();
                    await _displayRootRegistry.ShowModalPresentation(planningAccountAddViewModel);

                    UpdateData();
                }));
            }
        }

        public RelayCommand ShowEditGoalFrameCommand
        {
            get
            {
                return _showEditGoalFrameCommand ?? (_showEditGoalFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedGoal != null)
                    {
                        int lastSelectedId = SelectedGoal.Id;

                        //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        //var planningAccountEditViewModel = new PlanningAccountEditViewModel(
                        //    SelectedGoal.Id,
                        //    SelectedGoal.Name,
                        //    SelectedGoal.Number,
                        //    SelectedGoal.Current_Amount);

                        //await _displayRootRegistry.ShowModalPresentation(planningAccountEditViewModel);

                        UpdateData();

                        using (DatabaseContext database = new DatabaseContext())
                        {
                            SelectedGoal = database.Goals.ToList().Find(x => x.Id == lastSelectedId);
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для редактирования.");
                    }
                }));
            }
        }

        public RelayCommand ShowDeleteGoalFrameCommand
        {
            get
            {
                return _showDeleteGoalFrameCommand ?? (_showDeleteGoalFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedGoal != null)
                    {
                        var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var messageViewModel = new MessageViewModel(
                                                "Внимание",
                                                "Вы действительно хотите удалить данную запись?");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        if (messageViewModel.Result)
                        {
                            using (DatabaseContext database = new DatabaseContext())
                            {
                                Goal item = database.Goals.ToList().Find(x => x.Id == SelectedGoal.Id);
                                database.Goals.Remove(item);
                                database.SaveChanges();
                            }
                            UpdateData();
                            SelectedGoal = null;
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для удаления.");
                    }
                }));
            }
        }

        public PlanningGoalViewModel()
        {
            UpdateData();
            FillGoalData();
        }

        private void UpdateData()
        {
            int currentUserId = Convert.ToInt32(Settings.Default["currentUserId"]);

            using (DatabaseContext database = new DatabaseContext())
            {
                GoalsList = from item in database.Goals.ToList()
                            where item.User_Id == currentUserId
                            select item;
            }
        }

        private void FillGoalData()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (SelectedGoal != null)
                {
                    RequiredAmount = AdditionalFunctions.ConvertToCurrencyFormat(SelectedGoal.Required_Amount);
                    int tempSum = 0;
                    tempSum = database.Accounts.ToList().Find(x => x.Id == SelectedGoal.Account_Id).Current_Amount;
                    foreach (var item in database.Account_Operations.ToList().Where(x => x.Account_Id == SelectedGoal.Account_Id))
                    {
                        if (item.Type == "Снятие")
                        {
                            tempSum -= item.Amount;
                        }
                        else
                        {
                            tempSum += item.Amount;
                        }
                    }

                    CurrentAmount = AdditionalFunctions.ConvertToCurrencyFormat(tempSum);

                    if (RequiredAmount - CurrentAmount < 0)
                    {
                        RemainingAmount = 0;
                    }
                    else
                    {
                        RemainingAmount = RequiredAmount - CurrentAmount;
                    }

                    CalculateForecast();
                    FillChartData();
                }
                else
                {
                    RequiredAmount = 0;
                    CurrentAmount = 0;
                    RemainingAmount = 0;
                    ForecastTime = "-";

                    ChartValues<double> chartValues = new ChartValues<double> { 0, 1 };
                    SeriesCollection tempSeries = new SeriesCollection();
                    tempSeries.Add(new LineSeries
                    {
                        Title = "Выберите цель",
                        Values = chartValues,
                        PointGeometrySize = 14,
                        PointForeground = Brushes.Green,
                        StrokeThickness = 3,
                        FontFamily = new FontFamily("Balsamiq Sans"),
                        FontSize = 20
                    });

                    OperationsSeries = tempSeries;
                    DatesLabels = new string[] { "", "" };
                }
            }
        }

        private void CalculateForecast()
        {
            int tempSum = 0;
            int timeDiff = (DateTime.Now.Year - Convert.ToInt32(GetMinOperationsDate().Split('.')[2])) * 12
                        + DateTime.Now.Month - Convert.ToInt32(GetMinOperationsDate().Split('.')[1]) + 1;

            using (DatabaseContext database = new DatabaseContext())
            {
                foreach (var item in database.Account_Operations.ToList().Where(x => x.Account_Id == SelectedGoal.Account_Id))
                {
                    if (item.Type == "Снятие")
                    {
                        tempSum -= item.Amount;
                    }
                    else
                    {
                        tempSum += item.Amount;
                    }
                }
            }

            double averageAmount = AdditionalFunctions.ConvertToCurrencyFormat(tempSum / timeDiff);

            if (averageAmount > 0)
            {
                int months = Convert.ToInt32(RemainingAmount / averageAmount);
                int monthsMod = months % 10;
                int monthsDiv = months / 10;

                if (monthsMod == 1 && months != 11)
                {
                    ForecastTime = months + " месяц";
                }
                else if ((monthsMod == 2 || monthsMod == 3 || monthsMod == 4) && monthsDiv != 1)
                {
                    ForecastTime = months + " месяца";
                }
                else if (monthsMod == 5 || monthsMod == 6 || monthsMod == 7 || monthsMod == 8 || monthsMod == 9 || monthsMod == 0)
                {
                    ForecastTime = months + " месяцев";
                }
            }
            else
            {
                ForecastTime = "-";
            }
        }
        private string GetMinOperationsDate()
        {
            string minDate = "99.99.9999";

            IEnumerable<string> tempDates = new List<string>();

            using (DatabaseContext database = new DatabaseContext())
            {
                tempDates = from item in database.Account_Operations.ToList()
                            where item.Account_Id == SelectedGoal.Account_Id
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
            using (DatabaseContext database = new DatabaseContext())
            {
                double currentSum = AdditionalFunctions.ConvertToCurrencyFormat(
                    database.Accounts.ToList().Find(x => x.Id == SelectedGoal.Account_Id).Current_Amount);

                SeriesCollection tempCollection = new SeriesCollection();
                List<string> tempDates = new List<string>
                {
                    ""
                };

                ChartValues<double> chartValues = new ChartValues<double>
                {
                    currentSum
                };

                foreach (var item in database.Account_Operations.ToList())
                {
                    if (item.Account_Id == SelectedGoal.Account_Id)
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
}
