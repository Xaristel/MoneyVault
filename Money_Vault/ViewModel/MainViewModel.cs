using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Brush> _buttonsColorList = new ObservableCollection<Brush>()
        {
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White,
            Brushes.White
        };

        public ObservableCollection<Brush> ButtonsColorList
        {
            get
            {
                return _buttonsColorList;
            }
            set
            {
                _buttonsColorList = value;
            }
        }

        private List<string> _pagesPathList = new List<string>()
        {
            "/View/GeneralPage.xaml",
            "/View/IncomeMainPage.xaml",
            "/View/ExpenseMainPage.xaml",
            "/View/PlanningMainPage.xaml",
            "/View/HelpPage.xaml"
        };

        public MainViewModel()
        {
            CurrentPagePath = _pagesPathList[0];
        }

        public string CurrentPagePath
        {
            get => _currentPagePath;
            set
            {
                _currentPagePath = value;
                OnPropertyChanged("CurrentPagePath");

                AdditionalFunctions.SetColorForActiveButton(ButtonsColorList, _pagesPathList, CurrentPagePath);
            }
        }

        public RelayCommand ShowGeneralFrameCommand
        {
            get
            {
                return _showGeneralFrameCommand ?? (_showGeneralFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[0];
                }));
            }
        }

        public RelayCommand ShowIncomeFrameCommand
        {
            get
            {
                return _showIncomeFrameCommand ?? (_showIncomeFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[1];
                }));
            }
        }

        public RelayCommand ShowExpenseFrameCommand
        {
            get
            {
                return _showExpenseFrameCommand ?? (_showExpenseFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[2];
                }));
            }
        }

        public RelayCommand ShowPlanningFrameCommand
        {
            get
            {
                return _showPlanningFrameCommand ?? (_showPlanningFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[3];
                }));
            }
        }

        public RelayCommand ShowHelpFrameCommand
        {
            get
            {
                return _showHelpFrameCommand ?? (_showHelpFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[4];
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
