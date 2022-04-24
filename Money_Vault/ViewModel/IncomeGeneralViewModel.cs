using Money_Vault.Database;
using Money_Vault.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Money_Vault.ViewModel
{
    public class IncomeGeneralViewModel : BaseViewModel
    {
        private DatabaseContext _database;

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

        private IEnumerable<Income_Type> _income_Types;
        private IEnumerable<Income> _incomes;
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

        public IEnumerable<Income_Type> Income_Types
        {
            get => _income_Types;
            set
            {
                _income_Types = value;
                OnPropertyChanged("Income_Types");
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

        public IncomeGeneralViewModel()
        {
            _database = new DatabaseContext();

            Income_Types = _database.Income_Types.ToList();
            Incomes = _database.Incomes.ToList();

            CurrentMonth = _monthsList.ToList()[System.DateTime.Now.Month - 1];
            CurrentYear = Convert.ToString(System.DateTime.Now.Year);

            FillYearsList();
        }

        public void UpdateData()
        {
            List<IncomeCommonListItem> tempList = new List<IncomeCommonListItem>();

            foreach (var item in Incomes)
            {
                if (item.Date.Contains($".{CurrentMonth}.{CurrentYear}")
                    || item.Date.Contains($".0{CurrentMonth}.{CurrentYear}")
                    || (CurrentMonth == "Полный год" && item.Date.Contains($".{CurrentYear}"))
                    || CurrentYear == "Все годы")
                {
                    tempList.Add(new IncomeCommonListItem()
                    {
                        Id = item.Id,
                        TypeName = Income_Types.ToList().Find(x => x.Id == item.Income_Type_Id).Name,
                        Amount = ConvertToCurrencyFormat(item.Total_Amount),
                        Date = item.Date,
                        Note = item.Note
                    });
                }

            }

            IncomesList = tempList;
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

            YearsList.Sort();
            YearsList.Add("Все годы");
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
