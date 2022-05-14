using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class PlanningMainViewModel : BaseViewModel
    {
        private RelayCommand _showPlanningNoteFrameCommand;
        private RelayCommand _showPlanningAccumulationFrameCommand;
        private RelayCommand _showPlanningGoalFrameCommand;

        private string _currentPagePath;

        private ObservableCollection<Brush> _buttonsColorList = new ObservableCollection<Brush>()
        {
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
            "/View/PlanningNotePage.xaml",
            "/View/PlanningAccountPage.xaml",
            "/View/PlanningGoalPage.xaml"
        };

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

        public PlanningMainViewModel()
        {
            CurrentPagePath = "/View/PlanningNotePage.xaml";
        }

        //main menu buttons
        public RelayCommand ShowPlanningNoteFrameCommand
        {
            get
            {
                return _showPlanningNoteFrameCommand ?? (_showPlanningNoteFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[0];
                }));
            }
        }
        public RelayCommand ShowPlanningAccumulationFrameCommand
        {
            get
            {
                return _showPlanningAccumulationFrameCommand ?? (_showPlanningAccumulationFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[1];
                }));
            }
        }
        public RelayCommand ShowPlanningGoalFrameCommand
        {
            get
            {
                return _showPlanningGoalFrameCommand ?? (_showPlanningGoalFrameCommand = new RelayCommand((args) =>
                {
                    CurrentPagePath = _pagesPathList[2];
                }));
            }
        }
    }
}