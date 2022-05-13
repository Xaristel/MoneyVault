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
    public class PlanningNoteViewModel : BaseViewModel
    {
        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private string _text;
        private Note _selectedItem;
        private IEnumerable<Note> _notesList;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        public Note SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");

                if (SelectedItem != null)
                {
                    Text = SelectedItem.Text;
                }
                else
                {
                    Text = string.Empty;
                }
            }
        }

        public IEnumerable<Note> NotesList
        {
            get => _notesList;
            set
            {
                _notesList = value;
                OnPropertyChanged("NotesList");
            }
        }

        public RelayCommand ShowAddFrameCommand
        {
            get
            {
                return _showAddFrameCommand ?? (_showAddFrameCommand = new RelayCommand(async (args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    var planningNoteAddViewModel = new PlanningNoteAddViewModel();
                    await _displayRootRegistry.ShowModalPresentation(planningNoteAddViewModel);

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
                        var planningNoteEditViewModel = new PlanningNoteEditViewModel(
                            SelectedItem.Id,
                            SelectedItem.Name,
                            SelectedItem.Text);

                        await _displayRootRegistry.ShowModalPresentation(planningNoteEditViewModel);

                        UpdateData();
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для редактирования.");
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

                    if (SelectedItem != null)
                    {
                        var messageViewModel = new MessageViewModel(
                                                "Внимание",
                                                "Вы действительно хотите удалить данную запись?");

                        await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        if (messageViewModel.Result)
                        {
                            using (DatabaseContext database = new DatabaseContext())
                            {
                                Note item = database.Notes.ToList().Find(x => x.Id == SelectedItem.Id);
                                database.Notes.Remove(item);
                                database.SaveChanges();
                            }
                            UpdateData();
                        }
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для удаления.");
                    }
                }));
            }
        }

        public PlanningNoteViewModel()
        {
            UpdateData();
        }

        private void UpdateData()
        {
            int currentUserId = Convert.ToInt32(Settings.Default["currentUserId"]);

            using (DatabaseContext database = new DatabaseContext())
            {
                NotesList = from item in database.Notes.ToList()
                            where item.User_Id == currentUserId
                            select item;
            }
        }
    }
}
