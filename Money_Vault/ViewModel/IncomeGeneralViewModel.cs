using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class IncomeGeneralViewModel : BaseViewModel
    {
        private RelayCommand _selectMonthCommand;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private List<string> _yearsList;
        private string _currentYear;
        private string _currentMonth;
        private bool _isEnableMonthsButtons;
        private IncomeCommonListItem _selectedItem;
        private IEnumerable<IncomeCommonListItem> _incomesList;

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

        private ObservableCollection<Brush> _buttonsColorList = new ObservableCollection<Brush>()
        {
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White
        };

        public ObservableCollection<Brush> ButtonsColorList
        {
            get
            {
                return _buttonsColorList;
            }
            set
            {
                _buttonsColorList = value;
            }
        }

        public IncomeCommonListItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

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

                AdditionalFunctions.SetColorForActiveButton(ButtonsColorList, _monthsList, CurrentMonth);

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

        public IEnumerable<IncomeCommonListItem> IncomesList
        {
            get => _incomesList;
            set
            {
                _incomesList = value;
                OnPropertyChanged("IncomesList");
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
                return _showAddFrameCommand ?? (_showAddFrameCommand = new RelayCommand(async (args) =>
               {
                   var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                   var incomeGeneralAddViewModel = new IncomeGeneralAddViewModel();
                   await _displayRootRegistry.ShowModalPresentation(incomeGeneralAddViewModel);

                   UpdateData();
               }));
            }
        }

        public RelayCommand ShowEditFrameCommand
        {
            get
            {
                return _showEditFrameCommand ?? (_showEditFrameCommand = new RelayCommand(async (args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    if (SelectedItem != null)
                    {
                        var incomeGeneralEditViewModel = new IncomeGeneralEditViewModel(
                                                SelectedItem.Id,
                                                SelectedItem.TypeName,
                                                SelectedItem.Amount.ToString(),
                                                SelectedItem.Date.ToString("dd.MM.yyyy"),
                                                SelectedItem.Note);

                        await _displayRootRegistry.ShowModalPresentation(incomeGeneralEditViewModel);

                        UpdateData();
                    }
                    else
                    {
                        var messageViewModel = new MessageViewModel(
                            "Внимание",
                            "Вы не выбрали запись для редактирования.");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);
                    }
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

                    if (SelectedItem != null)
                    {
                        var messageViewModel = new MessageViewModel(
                                                "Внимание",
                                                "Вы действительно хотите удалить данную запись?");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        if (messageViewModel.Result)
                        {
                            using (DatabaseContext database = new DatabaseContext())
                            {
                                Income item = database.Incomes.ToList().Find(x => x.Id == SelectedItem.Id);
                                database.Incomes.Remove(item);
                                database.SaveChanges();
                            }
                            UpdateData();
                        }
                    }
                    else
                    {
                        var messageViewModel = new MessageViewModel(
                            "Внимание",
                            "Вы не выбрали запись для удаления.");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);
                    }
                }));
            }
        }

        public IncomeGeneralViewModel()
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
                List<IncomeCommonListItem> tempList = new List<IncomeCommonListItem>();

                foreach (var item in database.Incomes.ToList())
                {
                    if (item.Date.Contains($".{_monthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                        || item.Date.Contains($".0{_monthsList.IndexOf(CurrentMonth) + 1}.{CurrentYear}")
                        || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                        || CurrentYear == "Все годы")
                    {
                        tempList.Add(new IncomeCommonListItem()
                        {
                            Id = item.Id,
                            TypeName = database.Income_Types.ToList().Find(x => x.Id == item.Income_Type_Id).Name,
                            Amount = AdditionalFunctions.ConvertToCurrencyFormat(item.Total_Amount),
                            Date = new DateTime(Convert.ToInt32(item.Date.Split('.')[2]), Convert.ToInt32(item.Date.Split('.')[1]), Convert.ToInt32(item.Date.Split('.')[0])),
                            Note = item.Note
                        });
                    }
                }
                IncomesList = tempList;
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
                foreach (var item in database.Incomes.ToList())
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
