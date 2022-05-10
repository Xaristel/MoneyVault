using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class ExpenseShopEditViewModel : BaseViewModel
    {
        private int _id;
        private string _shop;
        private string _note;

        private RelayCommand<IClosable> _editCommand;
        private IEnumerable<string> _shopsList;

        public string Shop
        {
            get => _shop;
            set
            {
                _shop = value;
                OnPropertyChanged("Shop");
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

        public IEnumerable<string> ShopsList
        {
            get => _shopsList;
            set
            {
                _shopsList = value;
                OnPropertyChanged("ShopsList");
            }
        }

        public RelayCommand<IClosable> EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (Shop != null && Shop != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (database.Shops.ToList().Find(x => x.Name == Shop) == null)
                            {
                                Shop currentShop = database.Shops.ToList().Find(x => x.Id == _id);
                                currentShop.Name = Shop;
                                currentShop.Note = Note;

                                database.SaveChanges();

                                if (window != null)
                                {
                                    window.Close();
                                }
                            }
                            else
                            {
                                await CallErrorMessage("Такое название магазина уже существует!");
                            }
                        }
                    }
                    else
                    {
                        await CallErrorMessage("Заполнены не все поля или введены некорректные значения.");
                    }
                }));
            }
        }

        public ExpenseShopEditViewModel() { }

        public ExpenseShopEditViewModel(int id, string name, string note)
        {
            _id = id;
            Shop = name;
            Note = note;
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
