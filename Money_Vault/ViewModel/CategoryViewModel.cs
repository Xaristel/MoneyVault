using Money_Vault.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Money_Vault.Model;
using Money_Vault.Properties;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class CategoryViewModel : BaseViewModel
    {
        private IEnumerable<CategoryListItem> _categoriesList;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private CategoryListItem _selectedItem;

        public IEnumerable<CategoryListItem> CategoriesList
        {
            get => _categoriesList;
            set
            {
                _categoriesList = value;
                OnPropertyChanged("CategoriesList");
            }
        }

        public CategoryListItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public RelayCommand ShowAddFrameCommand
        {
            get
            {
                return _showAddFrameCommand ?? (_showAddFrameCommand = new RelayCommand(async (args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var categoryAddViewModel = new CategoryAddViewModel();
                    await _displayRootRegistry.ShowModalPresentation(categoryAddViewModel);

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
                        var categoryEditViewModel = new CategoryEditViewModel(SelectedItem.Id, SelectedItem.Name, SelectedItem.Note);
                        await _displayRootRegistry.ShowModalPresentation(categoryEditViewModel);

                        UpdateData();
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

                    var messageViewModel = new MessageViewModel(
                        "Внимание",
                        "Вы действительно хотите удалить данную запись?");

                    await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                    if (messageViewModel.Result && SelectedItem != null)
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
                            {
                                Income_Type item = database.Income_Types.ToList().Find(x => x.Id == SelectedItem.Id);
                                database.Income_Types.Remove(item);
                                database.SaveChanges();
                            }
                            else
                            {
                                Expense_Type item = database.Expense_Types.ToList().Find(x => x.Id == SelectedItem.Id);
                                database.Expense_Types.Remove(item);
                                database.SaveChanges();
                            }
                        }
                        UpdateData();
                    }
                }));
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
