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

        public IEnumerable<CategoryListItem> CategoriesList
        {
            get => _categoriesList;
            set
            {
                _categoriesList = value;
                OnPropertyChanged("CategoriesList");
            }
        }

        public RelayCommand ShowAddFrameCommand
        {
            get
            {
                return _showAddFrameCommand ?? (_showAddFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
                    {
                        //var categoryAddViewModel = new CategoryAddViewModel();
                        //_displayRootRegistry.ShowPresentation(categoryAddViewModel);
                    }
                    else
                    {
                        //var categoryAddViewModel = new CategoryAddViewModel();
                        //_displayRootRegistry.ShowPresentation(categoryAddViewModel);
                    }
                }));
            }
        }
        public RelayCommand ShowEditFrameCommand
        {
            get
            {
                return _showEditFrameCommand ?? (_showEditFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
                    {
                        //var categoryAddViewModel = new CategoryAddViewModel();
                        //_displayRootRegistry.ShowPresentation(categoryAddViewModel);
                    }
                    else
                    {
                        //var categoryAddViewModel = new CategoryAddViewModel();
                        //_displayRootRegistry.ShowPresentation(categoryAddViewModel);
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
                        "Вы действительно хотите удалить данную категорию?");

                    await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                    if (messageViewModel.Result)
                    {
                        //
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
