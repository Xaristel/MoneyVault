using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class ExpenseGeneralFullAddViewModel : BaseViewModel
    {
        private string _category;
        private string _shop;
        private DateTime _date;
        private string _note;
        private Visibility _isVisibleLabelPlaceHolderCategory;
        private Visibility _isVisibleLabelPlaceHolderShop;

        private RelayCommand<IClosable> _addCommand;

        private IEnumerable<string> _categoriesList;
        private IEnumerable<string> _subcategoriesList;
        private IEnumerable<string> _shopsList;
        private List<TotalListItem> _expenseItemsList = new List<TotalListItem>();

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

        public string Shop
        {
            get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged("Shop");

                IsVisibleLabelPlaceHolderShop = Visibility.Hidden;
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

        public Visibility IsVisibleLabelPlaceHolderShop
        {
            get => _isVisibleLabelPlaceHolderShop;
            set
            {
                _isVisibleLabelPlaceHolderShop = value;
                OnPropertyChanged("IsVisibleLabelPlaceHolderShop");
            }
        }

        public List<TotalListItem> ExpenseItemsList
        {
            get => _expenseItemsList;
            set
            {
                _expenseItemsList = value;
                OnPropertyChanged("ExpenseItemsList");
            }
        }

        public IEnumerable<string> SubcategoriesList
        {
            get => _subcategoriesList;
            set
            {
                _subcategoriesList = value;
                OnPropertyChanged("SubcategoriesList");
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

        public IEnumerable<string> ShopsList
        {
            get => _shopsList;
            set
            {
                _shopsList = value;
                OnPropertyChanged("ShopsList");
            }
        }

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    bool isCorrectDatagridData = true;

                    foreach (var item in ExpenseItemsList)
                    {
                        if (item.TypeName == "" || item.TotalAmount == 0)
                        {
                            isCorrectDatagridData = false;
                            break;
                        }
                    }

                    if (Category != null
                    && Date.ToString() != ""
                    && Shop != null
                    && isCorrectDatagridData)
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            database.Expenses.Add(new Expense()
                            {
                                User_Id = Convert.ToInt32(Settings.Default["currentUserId"]),
                                Expense_Type_Id = database.Expense_Types.ToList().Find(x => x.Name == Category).Id,
                                Total_Price = 0,
                                Shop_Id = database.Shops.ToList().Find(x => x.Name == Shop).Id,
                                Date = Date.ToString("dd.MM.yyyy"),
                                Note = Note
                            });

                            database.SaveChanges();

                            double totalPrice = 0;

                            foreach (var item in ExpenseItemsList)
                            {
                                database.Expense_Items.Add(new Expense_Item()
                                {
                                    Expense_Id = database.Expenses.ToList().Last().Id,
                                    Expense_Subtype_Id = database.Expense_Subtypes.ToList().Find(x => x.Name == item.TypeName).Id,
                                    Price = AdditionalFunctions.ConvertFromCurrencyFormat(item.TotalAmount.ToString())
                                });
                                totalPrice += item.TotalAmount;
                            }

                            Expense currentExpense = database.Expenses.ToList().Last();
                            currentExpense.Total_Price = AdditionalFunctions.ConvertFromCurrencyFormat(totalPrice.ToString());

                            database.SaveChanges();
                        }

                        if (window != null)
                        {
                            window.Close();
                        }
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

        public ExpenseGeneralFullAddViewModel()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                CategoriesList = from item in database.Expense_Types.ToList()
                                 select item.Name;

                SubcategoriesList = from item in database.Expense_Subtypes.ToList()
                                    select item.Name;

                ShopsList = from item in database.Shops.ToList()
                            select item.Name;
            }

            Date = System.DateTime.Now;
            //Amount = "";
        }
        private async Task CallErrorMessage(string message)
        {
            var displayRootRegistry = (Application.Current as App).displayRootRegistry;
            var messageViewModel = new MessageViewModel(
                "Ошибка",
                message);
            await displayRootRegistry.ShowModalPresentation(messageViewModel);
        }
    }
}
