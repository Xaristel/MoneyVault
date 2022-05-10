using System.Windows;
using System.Windows.Media;

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

        private Brush _generalMenuItemColor;
        private Brush _incomeMenuItemColor;
        private Brush _expenseMenuItemColor;
        private Brush _planMenuItemColor;
        private Brush _helpMenuItemColor;

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

                SetColorForActiveMenuItem(CurrentPagePath);
            }
        }

        public Brush GeneralMenuItemColor
        {
            get => _generalMenuItemColor;
            set
            {
                _generalMenuItemColor = value;
                OnPropertyChanged("GeneralMenuItemColor");
            }
        }

        public Brush IncomeMenuItemColor
        {
            get => _incomeMenuItemColor;
            set
            {
                _incomeMenuItemColor = value;
                OnPropertyChanged("IncomeMenuItemColor");
            }
        }

        public Brush ExpenseMenuItemColor
        {
            get => _expenseMenuItemColor;
            set
            {
                _expenseMenuItemColor = value;
                OnPropertyChanged("ExpenseMenuItemColor");
            }
        }

        public Brush PlanMenuItemColor
        {
            get => _planMenuItemColor;
            set
            {
                _planMenuItemColor = value;
                OnPropertyChanged("PlanMenuItemColor");
            }
        }

        public Brush HelpMenuItemColor
        {
            get => _helpMenuItemColor;
            set
            {
                _helpMenuItemColor = value;
                OnPropertyChanged("HelpMenuItemColor");
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

        private void SetColorForActiveMenuItem(string path)
        {
            switch (path)
            {
                case "/View/GeneralPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.DeepSkyBlue;
                        IncomeMenuItemColor = Brushes.White;
                        ExpenseMenuItemColor = Brushes.White;
                        PlanMenuItemColor = Brushes.White;
                        HelpMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/IncomeMainPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        IncomeMenuItemColor = Brushes.DeepSkyBlue;
                        ExpenseMenuItemColor = Brushes.White;
                        PlanMenuItemColor = Brushes.White;
                        HelpMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/ExpenseMainPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        IncomeMenuItemColor = Brushes.White;
                        ExpenseMenuItemColor = Brushes.DeepSkyBlue;
                        PlanMenuItemColor = Brushes.White;
                        HelpMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/PlanningMainPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        IncomeMenuItemColor = Brushes.White;
                        ExpenseMenuItemColor = Brushes.White;
                        PlanMenuItemColor = Brushes.DeepSkyBlue;
                        HelpMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/HelpPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        IncomeMenuItemColor = Brushes.White;
                        ExpenseMenuItemColor = Brushes.White;
                        PlanMenuItemColor = Brushes.White;
                        HelpMenuItemColor = Brushes.DeepSkyBlue;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
