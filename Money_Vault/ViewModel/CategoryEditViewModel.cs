using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class CategoryEditViewModel : BaseViewModel
    {
        private int _id;
        private string _category;
        private string _initialCategory;
        private string _note;

        private RelayCommand<IClosable> _editCommand;

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

        public RelayCommand<IClosable> EditCommand
        {
            get
            {
                return _editCommand ?? (_editCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (Category != null && Category != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (Convert.ToBoolean(Settings.Default["isIncomePage"]))
                            {
                                if (database.Income_Types.ToList().Find(x => x.Name == Category) == null)
                                {
                                    Income_Type item = database.Income_Types.ToList().Find(x => x.Id == _id);

                                    item.Name = Category;
                                    item.Note = Note;
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
                                    if (database.Expense_Types.ToList().Find(x => x.Name == _initialCategory) != null)
                                    {
                                        Expense_Type item = database.Expense_Types.ToList().Find(x => x.Id == _id);

                                        item.Name = Category;
                                        item.Note = Note;
                                        database.SaveChanges();

                                        if (window != null)
                                        {
                                            window.Close();
                                        }
                                    }
                                    else
                                    {
                                        Expense_Subtype item = database.Expense_Subtypes.ToList().Find(x => x.Id == _id);

                                        item.Name = Category;
                                        item.Note = Note;
                                        database.SaveChanges();

                                        if (window != null)
                                        {
                                            window.Close();
                                        }
                                    }
                                }
                                else
                                {
                                    await CallErrorMessage("Такое название категории или подкатегории уже существует!");
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

        public CategoryEditViewModel() { }

        public CategoryEditViewModel(int id, string category, string note)
        {
            _id = id;
            Category = category;
            _initialCategory = category;
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
