using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using System.Collections.Generic;
using System.Linq;

namespace Money_Vault.ViewModel
{
    public class ExpenseShopAddViewModel : BaseViewModel
    {
        private string _shop;
        private string _note;

        private RelayCommand<IClosable> _addCommand;
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

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (Shop != null && Shop != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (database.Shops.ToList().Find(x => x.Name == Shop) == null)
                            {
                                database.Shops.Add(new Shop()
                                {
                                    Name = Shop,
                                    Note = Note
                                });
                                database.SaveChanges();

                                if (window != null)
                                {
                                    window.Close();
                                }
                            }
                            else
                            {
                                await AdditionalFunctions.CallModalMessage("Ошибка", "Такое название магазина уже существует!");
                            }
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Ошибка", "Заполнены не все поля или введены некорректные значения.");
                    }
                }));
            }
        }
    }
}
