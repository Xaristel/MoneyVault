using Money_Vault.Properties;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class IncomeMainViewModel : BaseViewModel
    {
        private RelayCommand _showIncomeGeneralFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showIncomeReportFrameCommand;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;

        private string _currentIncomePagePath;

        public string CurrentIncomePagePath
        {
            get => _currentIncomePagePath;
            set
            {
                _currentIncomePagePath = value;
                OnPropertyChanged("CurrentIncomePagePath");
            }
        }

        public IncomeMainViewModel()
        {
            CurrentIncomePagePath = "/View/IncomeGeneralPage.xaml";
        }

        //main income menu buttons
        public RelayCommand ShowIncomeGeneralFrameCommand
        {
            get
            {
                return _showIncomeGeneralFrameCommand ?? (_showIncomeGeneralFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = "/View/IncomeGeneralPage.xaml";
                }));
            }
        }
        public RelayCommand ShowCategoryFrameCommand
        {
            get
            {
                return _showCategoryFrameCommand ?? (_showCategoryFrameCommand = new RelayCommand((args) =>
                {
                    Settings.Default["isIncomePage"] = true;
                    Settings.Default.Save();

                    CurrentIncomePagePath = "/View/CategoryPage.xaml";
                }));
            }
        }
        public RelayCommand ShowIncomeReportFrameCommand
        {
            get
            {
                return _showIncomeReportFrameCommand ?? (_showIncomeReportFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = "/View/ReportPage.xaml";
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

                    switch (CurrentIncomePagePath)
                    {
                        case "/View/IncomeGeneralPage.xaml":
                            {
                                var incomeGeneralAddViewModel = new IncomeGeneralAddViewModel();
                                _displayRootRegistry.ShowPresentation(incomeGeneralAddViewModel);
                                break;
                            }
                        case "/View/CategoryPage.xaml":
                            {
                                var categoryAddViewModel = new CategoryAddViewModel();
                                _displayRootRegistry.ShowPresentation(categoryAddViewModel);
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

                    switch (CurrentIncomePagePath)
                    {
                        case "/View/IncomeGeneralPage.xaml":
                            {
                                var incomeGeneralEditViewModel = new IncomeGeneralEditViewModel();
                                _displayRootRegistry.ShowPresentation(incomeGeneralEditViewModel);
                                break;
                            }
                        case "/View/CategoryPage.xaml":
                            {
                                var categoryEditViewModel = new CategoryEditViewModel();
                                _displayRootRegistry.ShowPresentation(categoryEditViewModel);
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

                    switch (CurrentIncomePagePath)
                    {
                        case "/View/IncomeGeneralPage.xaml":
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
                        case "/View/IncomeCategoryPage.xaml":
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
