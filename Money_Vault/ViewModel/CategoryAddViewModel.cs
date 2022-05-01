using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class CategoryAddViewModel : BaseViewModel
    {
        private string _category;
        private string _note;

        private RelayCommand<IClosable> _addCommand;

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged("Category");
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

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (Category != null && Category != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
                            {
                                database.Income_Types.Add(new Income_Type()
                                {
                                    Name = Category,
                                    Note = Note
                                });
                            }
                            else
                            {
                                database.Expense_Types.Add(new Expense_Type()
                                {
                                    Name = Category,
                                    Note = Note
                                });
                            }

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
    }
}
