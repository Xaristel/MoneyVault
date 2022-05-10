using Money_Vault.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class IncomeMainViewModel : BaseViewModel
    {
        private RelayCommand _showIncomeGeneralFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showIncomeReportFrameCommand;

        private string _currentIncomePagePath;

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
            "/View/IncomeGeneralPage.xaml",
            "/View/CategoryPage.xaml",
            "/View/ReportPage.xaml"
        };

        public string CurrentIncomePagePath
        {
            get => _currentIncomePagePath;
            set
            {
                _currentIncomePagePath = value;
                OnPropertyChanged("CurrentIncomePagePath");

                AdditionalFunctions.SetColorForActiveButton(ButtonsColorList, _pagesPathList, CurrentIncomePagePath);
            }
        }

        public IncomeMainViewModel()
        {
            CurrentIncomePagePath = "/View/IncomeGeneralPage.xaml";

            Settings.Default["isIncomePage"] = true;
            Settings.Default.Save();
        }

        //main income menu buttons
        public RelayCommand ShowIncomeGeneralFrameCommand
        {
            get
            {
                return _showIncomeGeneralFrameCommand ?? (_showIncomeGeneralFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = _pagesPathList[0];
                }));
            }
        }
        public RelayCommand ShowCategoryFrameCommand
        {
            get
            {
                return _showCategoryFrameCommand ?? (_showCategoryFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = _pagesPathList[1];
                }));
            }
        }
        public RelayCommand ShowIncomeReportFrameCommand
        {
            get
            {
                return _showIncomeReportFrameCommand ?? (_showIncomeReportFrameCommand = new RelayCommand((args) =>
                {
                    CurrentIncomePagePath = _pagesPathList[2];
                }));
            }
        }
    }
}
