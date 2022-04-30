﻿using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class IncomeGeneralEditViewModel : BaseViewModel
    {
        private int _id;
        private string _category;
        private string _amount;
        private DateTime _date;
        private string _note;

        private Visibility _isVisibleLabelPlaceHolderCategory;

        private RelayCommand _editCommand;
        private RelayCommand _cancelCommand;

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

        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand(async (args) =>
                {
                    if (AdditionalFunctions.CheckAmountFormat(Amount)
                    && Category != null
                    && Date.ToString() != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            Income currentIncome = database.Incomes.ToList().Find(x => x.Id == _id);

                            currentIncome.Income_Type_Id = database.Income_Types.ToList().Find(x => x.Name == Category).Id;
                            currentIncome.Total_Amount = AdditionalFunctions.ConvertFromCurrencyFormat(Amount);
                            currentIncome.Date = Date.ToString("dd.MM.yyyy");
                            currentIncome.Note = Note;

                            database.SaveChanges();
                        }

                        var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        displayRootRegistry.HidePresentation(this);
                    }
                    else
                    {
                        var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var messageViewModel = new MessageViewModel(
                            "Ошибка",
                            "Заполнены не все поля или введены некорректные значения.");
                        await displayRootRegistry.ShowModalPresentation(messageViewModel);
                    }
                }));
            }
        }

        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new RelayCommand((args) =>
                {
                    var displayRootRegistry = (Application.Current as App).displayRootRegistry;
                    displayRootRegistry.HidePresentation(this);
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

        public IncomeGeneralEditViewModel()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                CategoriesList = from item in database.Income_Types.ToList()
                                 select item.Name;
            }

            Date = System.DateTime.Now;
            Amount = "";
        }

        public IncomeGeneralEditViewModel(int id, string category, string amount, string date, string note)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                CategoriesList = from item in database.Income_Types.ToList()
                                 select item.Name;
            }

            _id = id;
            Date = new DateTime(
                Convert.ToInt32(date.Split('.')[2]),
                Convert.ToInt32(date.Split('.')[1]),
                Convert.ToInt32(date.Split('.')[0]));
            Amount = amount;
            Category = category;
            Note = note;
        }
    }
}
