using Money_Vault.Properties;
using System.Windows;
using System.Windows.Media;

namespace Money_Vault.ViewModel
{
    public class IncomeMainViewModel : BaseViewModel
    {
        private RelayCommand _showIncomeGeneralFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showIncomeReportFrameCommand;

        private string _currentIncomePagePath;

        private Brush _generalMenuItemColor;
        private Brush _categoryMenuItemColor;
        private Brush _reportMenuItemColor;

        public Brush GeneralMenuItemColor
        {
            get => _generalMenuItemColor;
            set
            {
                _generalMenuItemColor = value;
                OnPropertyChanged("GeneralMenuItemColor");
            }
        }

        public Brush CategoryMenuItemColor
        {
            get => _categoryMenuItemColor;
            set
            {
                _categoryMenuItemColor = value;
                OnPropertyChanged("CategoryMenuItemColor");
            }
        }

        public Brush ReportMenuItemColor
        {
            get => _reportMenuItemColor;
            set
            {
                _reportMenuItemColor = value;
                OnPropertyChanged("ReportMenuItemColor");
            }
        }

        public string CurrentIncomePagePath
        {
            get => _currentIncomePagePath;
            set
            {
                _currentIncomePagePath = value;
                OnPropertyChanged("CurrentIncomePagePath");

                SetColorForActiveMenuItem(CurrentIncomePagePath);
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

        private void SetColorForActiveMenuItem(string path)
        {
            switch (path)
            {
                case "/View/ExpenseGeneralPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.DeepSkyBlue;
                        CategoryMenuItemColor = Brushes.White;
                        ReportMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/CategoryPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        CategoryMenuItemColor = Brushes.DeepSkyBlue;
                        ReportMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/ReportPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        CategoryMenuItemColor = Brushes.White;
                        ReportMenuItemColor = Brushes.DeepSkyBlue;
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
