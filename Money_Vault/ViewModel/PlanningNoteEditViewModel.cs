using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class PlanningNoteEditViewModel : BaseViewModel
    {
        private int _id;
        private string _name;
        private string _note;

        private RelayCommand<IClosable> _editCommand;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
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
                    if (Name != "" && Note != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (database.Notes.ToList().Find(x => x.Name == Name && x.Id != _id) == null)
                            {
                                Note currentNote = database.Notes.ToList().Find(x => x.Id == _id);

                                currentNote.Name = Name;
                                currentNote.Text = Note;

                                database.SaveChanges();

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
                                    "Такое название заметки уже существует! Укажите другое название.");
                                await displayRootRegistry.ShowModalPresentation(messageViewModel);
                            }
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

        public PlanningNoteEditViewModel() { }

        public PlanningNoteEditViewModel(int id, string name, string note)
        {
            _id = id;
            Name = name;
            Note = note;
        }
    }
}
