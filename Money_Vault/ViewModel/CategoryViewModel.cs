using Money_Vault.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Money_Vault.Model;
using Money_Vault.Properties;

namespace Money_Vault.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        private IEnumerable<CategoryListItem> _categoriesList;

        public IEnumerable<CategoryListItem> CategoriesList
        {
            get => _categoriesList;
            set
            {
                _categoriesList = value;
                OnPropertyChanged("CategoriesList");
            }
        }

        public CategoryViewModel()
        {
            UpdateData();
        }

        public void UpdateData()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                List<CategoryListItem> categories = new List<CategoryListItem>();

                if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
                {

                    foreach (var item in database.Income_Types.ToList())
                    {
                        categories.Add(new CategoryListItem()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Note = item.Note
                        });
                    }
                }
                else
                {
                    foreach (var item in database.Expense_Types.ToList())
                    {
                        categories.Add(new CategoryListItem()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Note = item.Note
                        });
                    }
                }

                CategoriesList = categories;
            }
        }
    }
}
