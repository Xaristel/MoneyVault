using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class CategoryEditViewModel : BaseViewModel
    {
        private int _id;
        private string _category;
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
                                Income_Type item = database.Income_Types.ToList().Find(x => x.Id == _id);

                                item.Name = Category;
                                item.Note = Note;
                            }
                            else
                            {
                                Expense_Type item = database.Expense_Types.ToList().Find(x => x.Id == _id);

                                item.Name = Category;
                                item.Note = Note;
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

        public CategoryEditViewModel() { }

        public CategoryEditViewModel(int id, string category, string note)
        {
            _id = id;
            Category = category;
            Note = note;
        }
    }
}
