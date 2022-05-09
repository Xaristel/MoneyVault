using Money_Vault.Properties;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class IncomeMainViewModel : BaseViewModel
    {
        private RelayCommand _showIncomeGeneralFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showIncomeReportFrameCommand;

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
    }
}
