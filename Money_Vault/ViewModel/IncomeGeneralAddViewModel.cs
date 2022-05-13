using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class IncomeGeneralAddViewModel : BaseViewModel
    {
        private string _category;
        private string _amount;
        private DateTime _date;
        private string _note;
        private Visibility _isVisibleLabelPlaceHolderCategory;

        private RelayCommand<IClosable> _addCommand;

        private IEnumerable<string> _categoriesList;

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged("Category");

                IsVisibleLabelPlaceHolderCategory = Visibility.Hidden;
            }
        }

        public string Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged("Note");
            }
        }

        public Visibility IsVisibleLabelPlaceHolderCategory
        {
            get => _isVisibleLabelPlaceHolderCategory;
            set
            {
                _isVisibleLabelPlaceHolderCategory = value;
                OnPropertyChanged("IsVisibleLabelPlaceHolderCategory");
            }
        }

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && Category != null
                    && Date.ToString() != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            database.Incomes.Add(new Income()
                            {
                                User_Id = Convert.ToInt32(Settings.Default["currentUserId"]),
                                Income_Type_Id = database.Income_Types.ToList().Find(x => x.Name == Category).Id,
                                Total_Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount),
                                Date = Date.ToString("dd.MM.yyyy"),
                                Note = Note
                            });

                            database.SaveChanges();
                        }

                        if (window != null)
                        {
                            window.Close();
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Ошибка", "Заполнены не все поля или введены некорректные значения.");
                    }
                }));
            }
        }

        public IEnumerable<string> CategoriesList
        {
            get => _categoriesList;
            set
            {
                _categoriesList = value;
                OnPropertyChanged("CategoriesList");
            }
        }

        public IncomeGeneralAddViewModel()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                CategoriesList = from item in database.Income_Types.ToList()
                                 select item.Name;
            }

            Date = System.DateTime.Now;
            Amount = "";
        }
    }
}
