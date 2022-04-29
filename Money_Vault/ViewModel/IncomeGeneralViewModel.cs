using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Money_Vault.ViewModel
{
    public class IncomeGeneralViewModel : BaseViewModel
    {
        private RelayCommand _selectMonthCommand;

        private List<string> _yearsList;
        private string _currentYear;
        private string _currentMonth;
        private bool _isEnableMonthsButtons;

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

        private IEnumerable<IncomeCommonListItem> _incomesList;

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
