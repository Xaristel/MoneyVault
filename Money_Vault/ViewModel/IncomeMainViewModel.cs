using Money_Vault.Database;
using Money_Vault.Model;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class IncomeMainViewModel : BaseViewModel
    {
        private RelayCommand _showIncomeGeneralFrameCommand;
        private RelayCommand _showIncomeStatisticFrameCommand;
        private RelayCommand _showIncomeCategoryFrameCommand;
        private RelayCommand _showIncomeReportFrameCommand;

        private RelayCommand _showAddFrameCommand;
        private RelayCommand _showEditFrameCommand;
        private RelayCommand _showDeleteFrameCommand;
        private RelayCommand _showInfoFrameCommand;

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
        public RelayCommand ShowIncomeStatisticFrameCommand
        {
            get
            {
                return _showIncomeStatisticFrameCommand ?? (_showIncomeStatisticFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = "/View/IncomeStatisticPage.xaml";
                }));
            }
        }
        public RelayCommand ShowIncomeCategoryFrameCommand
        {
            get
            {
                return _showIncomeCategoryFrameCommand ?? (_showIncomeCategoryFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = "/View/IncomeCategoryPage.xaml";
                }));
            }
        }
        public RelayCommand ShowIncomeReportFrameCommand
        {
            get
            {
                return _showIncomeReportFrameCommand ?? (_showIncomeReportFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = "/View/IncomeReportPage.xaml";
                }));
            }
        }

        //sub_buttons
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
                        case "/View/IncomeCategoryPage.xaml":
                            {
                                var incomeCategoryAddViewModel = new IncomeCategoryAddViewModel();
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

                    switch (CurrentIncomePagePath)
                    {
                        case "/View/IncomeGeneralPage.xaml":
                            {
                                var incomeGeneralEditViewModel = new IncomeGeneralEditViewModel();
                                _displayRootRegistry.ShowPresentation(incomeGeneralEditViewModel);
                                break;
                            }
                        case "/View/IncomeCategoryPage.xaml":
                            {
                                var incomeCategoryEditViewModel = new IncomeCategoryEditViewModel();
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
                return _showDeleteFrameCommand ?? (_showDeleteFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    switch (CurrentIncomePagePath)
                    {
                        case "/View/IncomeGeneralPage.xaml":
                            {
                                var messageViewModel = new MessageViewModel("Ошибка", "Пользователя не существует или были введены неверные данные.");
                                _displayRootRegistry.ShowPresentation(messageViewModel);
                                break;
                            }
                        case "/View/IncomeCategoryPage.xaml":
                            {
                                var messageViewModel = new MessageViewModel("Ошибка", "Пользователя не существует или были введены неверные данные.");
                                _displayRootRegistry.ShowPresentation(messageViewModel);
                                break;
                            }
                        default:
                            break;
                    }
                }));
            }
        }
        public RelayCommand ShowInfoFrameCommand
        {
            get
            {
                return _showInfoFrameCommand ?? (_showInfoFrameCommand = new RelayCommand((args) =>
                {
                    var _displayRootRegistry = (Application.Current as App).displayRootRegistry;

                    switch (CurrentIncomePagePath)
                    {
                        case "/View/IncomeGeneralPage.xaml":
                            {
                                var messageViewModel = new MessageViewModel("Ошибка", "Пользователя не существует или были введены неверные данные.");
                                _displayRootRegistry.ShowPresentation(messageViewModel);
                                break;
                            }
                        case "/View/IncomeCategoryPage.xaml":
                            {
                                var messageViewModel = new MessageViewModel("Ошибка", "Пользователя не существует или были введены неверные данные.");
                                _displayRootRegistry.ShowPresentation(messageViewModel);
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
