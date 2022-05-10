using Money_Vault.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Money_Vault.ViewModel
{
    public class ExpenseGeneralFullInfoViewModel : BaseViewModel
    {
        private string _category;
        private string _shop;
        private string _date;
        private string _note;

        private List<TotalListItem> _expenseItemsList = new List<TotalListItem>();

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged("Category");
            }
        }

        public string Shop
        {
            get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged("Shop");
            }
        }

        public string Date
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

        public List<TotalListItem> ExpenseItemsList
        {
            get => _expenseItemsList;
            set
            {
                _expenseItemsList = value;
                OnPropertyChanged("ExpenseItemsList");
            }
        }

        public ExpenseGeneralFullInfoViewModel() { }

        public ExpenseGeneralFullInfoViewModel(int id, string category, string shop, string date, string note)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                Category = category;
                Note = note;
                Shop = shop;
                Date = date;

                List<TotalListItem> tempList = new List<TotalListItem>();

                var expenseItems = from item in database.Expense_Items.ToList()
                                   where item.Expense_Id == id
                                   select item;

                foreach (var item in expenseItems)
                {
                    tempList.Add(new TotalListItem()
                    {
                        TypeName = database.Expense_Subtypes.ToList().Find(x => x.Id == item.Expense_Subtype_Id).Name,
                        TotalAmount = AdditionalFunctions.ConvertToCurrencyFormat(item.Price)
                    });
                }
                ExpenseItemsList = tempList;
            }
        }
    }
}
