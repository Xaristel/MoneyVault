using System.Windows;

namespace Money_Vault.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private RelayCommand _showGeneralFrameCommand;
        private RelayCommand _showIncomeFrameCommand;
        private RelayCommand _showExpenseFrameCommand;
        private RelayCommand _showPlanningFrameCommand;
        private RelayCommand _showHelpFrameCommand;
        private RelayCommand _exitCommand;

        private string _currentPagePath;

        public MainViewModel()
        {
            CurrentPagePath = "/View/GeneralPage.xaml";
        }

        public string CurrentPagePath
        {
            get => _currentPagePath;
            set
            {
                _currentPagePath = value;
                OnPropertyChanged("CurrentPagePath");
            }
        }

        public RelayCommand ShowGeneralFrameCommand
        {
            get
            {
                return _showGeneralFrameCommand ?? (_showGeneralFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = "/View/GeneralPage.xaml";
                }));
            }
        }

        public RelayCommand ShowIncomeFrameCommand
        {
            get
            {
                return _showIncomeFrameCommand ?? (_showIncomeFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = "/View/IncomeMainPage.xaml";
                }));
            }
        }

        public RelayCommand ShowExpenseFrameCommand
        {
            get
            {
                return _showExpenseFrameCommand ?? (_showExpenseFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = "/View/ExpenseMainPage.xaml";
                }));
            }
        }

        public RelayCommand ShowPlanningFrameCommand
        {
            get
            {
                return _showPlanningFrameCommand ?? (_showPlanningFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = "/View/PlanningMainPage.xaml";
                }));
            }
        }

        public RelayCommand ShowHelpFrameCommand
        {
            get
            {
                return _showHelpFrameCommand ?? (_showHelpFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = "/View/HelpPage.xaml";
                }));
            }
        }

        public RelayCommand ExitCommand
        {
            get
            {
                return _exitCommand ?? (_exitCommand = new RelayCommand((args) =>
                {
                    (Application.Current as App).displayRootRegistry.HidePresentation(this);
                }));
            }
        }
    }
}
