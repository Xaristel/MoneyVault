using GalaSoft.MvvmLight.CommandWpf;
using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class PlanningNoteAddViewModel : BaseViewModel
    {
        private string _name;
        private string _note;

        private RelayCommand<IClosable> _addCommand;

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

        public RelayCommand<IClosable> AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new RelayCommand<IClosable>(async (window) =>
                {
                    if (Name != "" && Note != "")
                    {
                        using (DatabaseContext database = new DatabaseContext())
                        {
                            if (database.Notes.ToList().Find(x => x.Name == Name) == null)
                            {
                                database.Notes.Add(new Note()
                                {
                                    User_Id = Convert.ToInt32(Settings.Default["currentUserId"]),
                                    Name = Name,
                                    Text = Note
                                });

                                database.SaveChanges();

                                if (window != null)
                                {
                                    window.Close();
                                }
                            }
                            else
                            {
                                await AdditionalFunctions.CallModalMessage("Ошибка", "Такое название заметки уже существует! Укажите другое название.");
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
