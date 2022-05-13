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
        private IEnumerable<CategoryListItem> _subcategoriesList;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private CategoryListItem _selectedItem;

        private string _buttonContentCurrentMode;

        private Visibility _isVisibleExpenseSubMenu;
        private Visibility _isVisibleIncomeSubMenu;

        private int _widthMainDataGrid;
        private int _heightMainDataGrid;
        private Visibility _isVisibleSubcategoriesDataGrid;

        public IEnumerable<CategoryListItem> CategoriesList
        {
            get => _categoriesList;
            set
            {
                _categoriesList = value;
                OnPropertyChanged("CategoriesList");
            }
        }

        public IEnumerable<CategoryListItem> SubcategoriesList
        {
            get => _subcategoriesList;
            set
            {
                _subcategoriesList = value;
                OnPropertyChanged("SubcategoriesList");
            }
        }

        public string ButtonContentCurrentMode
        {
            get => _buttonContentCurrentMode;
            set
            {
                _buttonContentCurrentMode = value;
                OnPropertyChanged("ButtonContentCurrentMode");
            }
        }

        public Visibility IsVisibleExpenseSubMenu
        {
            get => _isVisibleExpenseSubMenu;
            set
            {
                _isVisibleExpenseSubMenu = value;
                OnPropertyChanged("IsVisibleExpenseSubMenu");
            }
        }

        public Visibility IsVisibleIncomeSubMenu
        {
            get => _isVisibleIncomeSubMenu;
            set
            {
                _isVisibleIncomeSubMenu = value;
                OnPropertyChanged("IsVisibleIncomeSubMenu");
            }
        }

        public int WidthMainDataGrid
        {
            get => _widthMainDataGrid;
            set
            {
                _widthMainDataGrid = value;
                OnPropertyChanged("WidthMainDataGrid");
            }
        }

        public int HeightMainDataGrid
        {
            get => _heightMainDataGrid;
            set
            {
                _heightMainDataGrid = value;
                OnPropertyChanged("HeightMainDataGrid");
            }
        }

        public Visibility IsVisibleSubcategoriesDataGrid
        {
            get => _isVisibleSubcategoriesDataGrid;
            set
            {
                _isVisibleSubcategoriesDataGrid = value;
                OnPropertyChanged("IsVisibleSubcategoriesDataGrid");
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
                    if (SelectedItem != null)
                    {
                        var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        var categoryEditViewModel = new CategoryEditViewModel(SelectedItem.Id, SelectedItem.Name, SelectedItem.Note);
                        await _displayRootRegistry.ShowModalPresentation(categoryEditViewModel);

                        UpdateData();
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Ошибка", "Вы не выбрали категорию для редактирования!");
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
                    if (SelectedItem != null)
                    {
                        var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                        var messageViewModel = new MessageViewModel(
                            "Внимание",
                            "Вы действительно хотите удалить данную запись?");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        if (messageViewModel.Result)
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
                                    if (database.Expense_Types.ToList().Find(x => (x.Name == SelectedItem.Name)) != null)
                                    {
                                        Expense_Type item = database.Expense_Types.ToList().Find(x => x.Id == SelectedItem.Id);
                                        database.Expense_Types.Remove(item);
                                    }
                                    else
                                    {
                                        Expense_Subtype item = database.Expense_Subtypes.ToList().Find(x => x.Id == SelectedItem.Id);
                                        database.Expense_Subtypes.Remove(item);
                                    }

                                    database.SaveChanges();
                                }
                            }
                            UpdateData();
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Ошибка", "Вы не выбрали категорию для удаления!");
                    }
                }));
            }
        }

        public CategoryViewModel()
        {
            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
            {
                IsVisibleIncomeSubMenu = Visibility.Visible;
                IsVisibleExpenseSubMenu = Visibility.Hidden;

                IsVisibleSubcategoriesDataGrid = Visibility.Hidden;
                WidthMainDataGrid = 1389;
                HeightMainDataGrid = 690;
            }
            else
            {
                if (Convert.ToBoolean(Settings.Default["currentExpenseMode"]))
                {
                    ButtonContentCurrentMode = "Сменить\nрежим на\nКраткий";
                }
                else
                {
                    ButtonContentCurrentMode = "Сменить\nрежим на\nПолный";
                }

                IsVisibleIncomeSubMenu = Visibility.Hidden;
                IsVisibleExpenseSubMenu = Visibility.Visible;

                IsVisibleSubcategoriesDataGrid = Visibility.Visible;
                WidthMainDataGrid = 685;
                HeightMainDataGrid = 650;
            }

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
                        if (item.UserId == Convert.ToInt32(Settings.Default["currentUserId"]))
                        {
                            categories.Add(new CategoryListItem()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Note = item.Note
                            });
                        }
                    }
                }
                else
                {
                    List<CategoryListItem> subcategories = new List<CategoryListItem>();

                    foreach (var item in database.Expense_Types.ToList())
                    {
                        if (item.UserId == Convert.ToInt32(Settings.Default["currentUserId"]))
                        {
                            categories.Add(new CategoryListItem()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Note = item.Note
                            });
                        }
                    }

                    foreach (var item in database.Expense_Subtypes.ToList())
                    {
                        if (item.UserId == Convert.ToInt32(Settings.Default["currentUserId"]))
                        {
                            subcategories.Add(new CategoryListItem()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                Note = item.Note
                            });
                        }
                    }

                    SubcategoriesList = subcategories;
                }

                CategoriesList = categories;
            }
        }
    }
}
