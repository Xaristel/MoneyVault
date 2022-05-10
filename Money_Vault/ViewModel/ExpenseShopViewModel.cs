using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class ExpenseShopViewModel : BaseViewModel
    {
        private IEnumerable<Shop> _shopsList;
        private Shop _selectedItem;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private string _buttonContentCurrentMode;

        public IEnumerable<Shop> ShopsList
        {
            get => _shopsList;
            set
            {
                _shopsList = value;
                OnPropertyChanged("ShopsList");
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

        public Shop SelectedItem
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

                    var expenseShopAddViewModel = new ExpenseShopAddViewModel();
                    await _displayRootRegistry.ShowModalPresentation(expenseShopAddViewModel);

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
                        var expenseShopEditViewModel = new ExpenseShopEditViewModel(SelectedItem.Id, SelectedItem.Name, SelectedItem.Note);
                        await _displayRootRegistry.ShowModalPresentation(expenseShopEditViewModel);

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
                            Shop item = database.Shops.ToList().Find(x => x.Id == SelectedItem.Id);
                            database.Shops.Remove(item);
                            database.SaveChanges();
                        }
                        UpdateData();
                    }
                }));
            }
        }

        public ExpenseShopViewModel()
        {
            if (Convert.ToBoolean(Settings.Default["currentExpenseMode"]))
            {
                ButtonContentCurrentMode = "Сменить\nрежим на\nКраткий";
            }
            else
            {
                ButtonContentCurrentMode = "Сменить\nрежим на\nПолный";
            }

            UpdateData();
        }

        public void UpdateData()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                ShopsList = database.Shops.ToList();
            }
        }
    }
}
