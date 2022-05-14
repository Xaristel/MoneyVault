using Money_Vault.Database;
using Money_Vault.Model;
using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class PlanningAccountViewModel : BaseViewModel
    {
        private RelayCommand _showAddOperationFrameCommand;
        private RelayCommand _showEditOperationFrameCommand;
        private RelayCommand _showDeleteOperationFrameCommand;

        private RelayCommand _showAddAccountFrameCommand;
        private RelayCommand _showEditAccountFrameCommand;
        private RelayCommand _showDeleteAccountFrameCommand;

        private IEnumerable<Account> _accountsList;
        private Account _selectedAccount;

        private IEnumerable<Account_Operation> _operationsList;
        private Account_Operation _selectedOperation;

        private int _currentForecastMonth;
        private int _currentForecastYear;

        public IEnumerable<Account> AccountsList
        {
            get => _accountsList;
            set
            {
                _accountsList = value;
                OnPropertyChanged("AccountsList");
            }
        }

        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged("SelectedAccount");

                FillOperationList();
            }
        }

        public IEnumerable<Account_Operation> OperationsList
        {
            get => _operationsList;
            set
            {
                _operationsList = value;
                OnPropertyChanged("OperationsList");
            }
        }

        public Account_Operation SelectedOperation
        {
            get => _selectedOperation;
            set
            {
                _selectedOperation = value;
                OnPropertyChanged("SelectedOperation");
            }
        }

        public int CurrentForecastMonth
        {
            get => _currentForecastMonth;
            set
            {
                _currentForecastMonth = value;
                OnPropertyChanged("ForecastMonth");
            }
        }

        public int CurrentForecastYear
        {
            get => _currentForecastYear;
            set
            {
                _currentForecastYear = value;
                OnPropertyChanged("ForecastYear");
            }
        }

        public RelayCommand ShowAddOperationFrameCommand
        {
            get
            {
                return _showAddOperationFrameCommand ?? (_showAddOperationFrameCommand = new RelayCommand(async (args) =>
                {
                    //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    //var planningNoteAddViewModel = new PlanningNoteAddViewModel();
                    //await _displayRootRegistry.ShowModalPresentation(planningNoteAddViewModel);

                    //UpdateData();
                }));
            }
        }

        public RelayCommand ShowEditOperationFrameCommand
        {
            get
            {
                return _showEditOperationFrameCommand ?? (_showEditOperationFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedOperation != null)
                    {
                        //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        //var planningNoteEditViewModel = new PlanningNoteEditViewModel(
                        //    SelectedItem.Id,
                        //    SelectedItem.Name,
                        //    SelectedItem.Text);

                        //await _displayRootRegistry.ShowModalPresentation(planningNoteEditViewModel);

                        //UpdateData();
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для редактирования.");
                    }
                }));
            }
        }

        public RelayCommand ShowDeleteOperationFrameCommand
        {
            get
            {
                return _showDeleteOperationFrameCommand ?? (_showDeleteOperationFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedOperation != null)
                    {
                        //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        //var messageViewModel = new MessageViewModel(
                        //                        "Внимание",
                        //                        "Вы действительно хотите удалить данную запись?");

                        //await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        //if (messageViewModel.Result)
                        //{
                        //    using (DatabaseContext database = new DatabaseContext())
                        //    {
                        //        Note item = database.Notes.ToList().Find(x => x.Id == SelectedItem.Id);
                        //        database.Notes.Remove(item);
                        //        database.SaveChanges();
                        //    }
                        //    UpdateData();
                        //}
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для удаления.");
                    }
                }));
            }
        }

        public RelayCommand ShowAddAccountFrameCommand
        {
            get
            {
                return _showAddAccountFrameCommand ?? (_showAddAccountFrameCommand = new RelayCommand(async (args) =>
                {
                    //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    //var planningNoteAddViewModel = new PlanningNoteAddViewModel();
                    //await _displayRootRegistry.ShowModalPresentation(planningNoteAddViewModel);

                    //UpdateData();
                }));
            }
        }

        public RelayCommand ShowEditAccountFrameCommand
        {
            get
            {
                return _showEditAccountFrameCommand ?? (_showEditAccountFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        //var planningNoteEditViewModel = new PlanningNoteEditViewModel(
                        //    SelectedItem.Id,
                        //    SelectedItem.Name,
                        //    SelectedItem.Text);

                        //await _displayRootRegistry.ShowModalPresentation(planningNoteEditViewModel);

                        //UpdateData();
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для редактирования.");
                    }
                }));
            }
        }

        public RelayCommand ShowDeleteAccountFrameCommand
        {
            get
            {
                return _showDeleteAccountFrameCommand ?? (_showDeleteAccountFrameCommand = new RelayCommand(async (args) =>
                {
                    if (SelectedAccount != null)
                    {
                        //var _displayRootRegistry = (Application.Current as App).displayRootRegistry;
                        //var messageViewModel = new MessageViewModel(
                        //                        "Внимание",
                        //                        "Вы действительно хотите удалить данную запись?");

                        //await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                        //if (messageViewModel.Result)
                        //{
                        //    using (DatabaseContext database = new DatabaseContext())
                        //    {
                        //        Note item = database.Notes.ToList().Find(x => x.Id == SelectedItem.Id);
                        //        database.Notes.Remove(item);
                        //        database.SaveChanges();
                        //    }
                        //    UpdateData();
                        //}
                    }
                    else
                    {
                        await AdditionalFunctions.CallModalMessage("Внимание", "Вы не выбрали запись для удаления.");
                    }
                }));
            }
        }

        public PlanningAccountViewModel()
        {
            UpdateData();
        }

        private void UpdateData()
        {
            int currentUserId = Convert.ToInt32(Settings.Default["currentUserId"]);

            using (DatabaseContext database = new DatabaseContext())
            {
                AccountsList = from item in database.Accounts.ToList()
                               where item.User_Id == currentUserId
                               select item;

            }

            FillOperationList();
        }

        private void FillOperationList()
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                if (SelectedAccount != null)
                {
                    OperationsList = from item in database.Account_Operations.ToList()
                                     where item.Account_Id == SelectedAccount.Id
                                     select item;
                }
            }
        }
    }
}
