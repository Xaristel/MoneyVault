using Money_Vault.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Money_Vault.Model;
using System.Windows;
using Money_Vault.Properties;

namespace Money_Vault.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        private DatabaseContext _database;
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
            _database = new DatabaseContext();

            UpdateData();
        }

        public void UpdateData()
        {
            List<CategoryListItem> categories = new List<CategoryListItem>();

            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
            {
                foreach (var item in _database.Income_Types.ToList())
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
                foreach (var item in _database.Expense_Types.ToList())
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
