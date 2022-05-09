using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class CategoryAddViewModel : BaseViewModel
    {
        private string _type;
        private string _category;
        private string _note;

        private RelayCommand<IClosable> _addCommand;
        private Visibility _isVisibleLabelPlaceHolderType;
        private IEnumerable<string> _typesList;

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged("Type");

                IsVisibleLabelPlaceHolderType = Visibility.Hidden;
            }
        }

        public IEnumerable<string> TypesList
        {
            get => _typesList;
            set
            {
                _typesList = value;
                OnPropertyChanged("TypesList");
            }
        }

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

        public Visibility IsVisibleLabelPlaceHolderType
        {
            get => _isVisibleLabelPlaceHolderType;
            set
            {
                _isVisibleLabelPlaceHolderType = value;
                OnPropertyChanged("IsVisibleLabelPlaceHolderType");
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
                                if (database.Income_Types.ToList().Find(x => x.Name == Category) == null)
                                {
                                    database.Income_Types.Add(new Income_Type()
                                    {
                                        Name = Category,
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
                                    await CallErrorMessage("Такое название категории уже существует!");
                                }
                            }
                            else
                            {
                                if (database.Expense_Types.ToList().Find(x => x.Name == Category) == null
                                && database.Expense_Subtypes.ToList().Find(x => x.Name == Category) == null)
                                {
                                    if (Type == "Категория")
                                    {
                                        database.Expense_Types.Add(new Expense_Type()
                                        {
                                            Name = Category,
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
                                        database.Expense_Subtypes.Add(new Expense_Subtype()
                                        {
                                            Name = Category,
                                            Note = Note
                                        });
                                        database.SaveChanges();

                                        if (window != null)
                                        {
                                            window.Close();
                                        }
                                    }
                                }
                                else
                                {
                                    await CallErrorMessage("Такое название подкатегории или категории уже существует!");
                                }
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

        public CategoryAddViewModel()
        {
            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
            {
                TypesList = new List<string>() { "Категория" };
            }
            else
            {
                TypesList = new List<string>() { "Категория", "Подкатегория" };
            }

            IsVisibleLabelPlaceHolderType = Visibility.Visible;
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
