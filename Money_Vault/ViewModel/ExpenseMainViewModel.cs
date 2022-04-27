using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class ExpenseMainViewModel : BaseViewModel
    {
        private RelayCommand _showExpenseGeneralFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showExpenseReportFrameCommand;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private string _currentExpensePagePath;

        public string CurrentExpensePagePath
        {
            get => _currentExpensePagePath;
            set
            {
                _currentExpensePagePath = value;
                OnPropertyChanged("CurrentExpensePagePath");
            }
        }

        public ExpenseMainViewModel()
        {
            CurrentExpensePagePath = "/View/ExpenseGeneralPage.xaml";
        }

        //main income menu buttons
        public RelayCommand ShowExpenseGeneralFrameCommand
        {
            get
            {
                return _showExpenseGeneralFrameCommand ?? (_showExpenseGeneralFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = "/View/ExpenseGeneralPage.xaml";
                }));
            }
        }
        public RelayCommand ShowCategoryFrameCommand
        {
            get
            {
                return _showCategoryFrameCommand ?? (_showCategoryFrameCommand = new RelayCommand((args) =>
                {
                    Settings.Default["isIncomePage"] = false;
                    Settings.Default.Save();

                    CurrentExpensePagePath = "/View/CategoryPage.xaml";
                }));
            }
        }
        public RelayCommand ShowExpenseReportFrameCommand
        {
            get
            {
                return _showExpenseReportFrameCommand ?? (_showExpenseReportFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = "/View/ExpenseReportPage.xaml";
                }));
            }
        }

        //sub income buttons
        public RelayCommand ShowAddFrameCommand
        {
            get
            {
                return _showAddFrameCommand ?? (_showAddFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    switch (CurrentExpensePagePath)
                    {
                        case "/View/ExpenseGeneralPage.xaml":
                            {
                                var incomeGeneralAddViewModel = new ExpenseGeneralAddViewModel();
                                _displayRootRegistry.ShowPresentation(incomeGeneralAddViewModel);
                                break;
                            }
                        case "/View/ExpenseCategoryPage.xaml":
                            {
                                var incomeCategoryAddViewModel = new CategoryAddViewModel();
                                _displayRootRegistry.ShowPresentation(incomeCategoryAddViewModel);
                                break;
                            }
                        default:
                            break;
                    }
                }));
            }
        }
        public RelayCommand ShowEditFrameCommand
        {
            get
            {
                return _showEditFrameCommand ?? (_showEditFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    switch (CurrentExpensePagePath)
                    {
                        case "/View/ExpenseGeneralPage.xaml":
                            {
                                var incomeGeneralEditViewModel = new ExpenseGeneralEditViewModel();
                                _displayRootRegistry.ShowPresentation(incomeGeneralEditViewModel);
                                break;
                            }
                        case "/View/ExpenseCategoryPage.xaml":
                            {
                                var incomeCategoryEditViewModel = new CategoryEditViewModel();
                                _displayRootRegistry.ShowPresentation(incomeCategoryEditViewModel);
                                break;
                            }
                        default:
                            break;
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

                    switch (CurrentExpensePagePath)
                    {
                        case "/View/ExpenseGeneralPage.xaml":
                            {
                                var messageViewModel = new MessageViewModel(
                                    "Внимание",
                                    "Вы действительно хотите удалить данную запись?");

                                await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                                if (messageViewModel.Result)
                                {
                                    //
                                }
                                break;
                            }
                        case "/View/ExpenseCategoryPage.xaml":
                            {
                                var messageViewModel = new MessageViewModel(
                                    "Внимание",
                                    "Вы действительно хотите удалить данную категорию?");

                                await _displayRootRegistry.ShowModalPresentation(messageViewModel);

                                if (messageViewModel.Result)
                                {
                                    //
                                }
                                break;
                            }
                        default:
                            break;
                    }
                }));
            }
        }
    }
}
