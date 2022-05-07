using Money_Vault.Properties;
using System.Windows;

namespace Money_Vault.ViewModel
{
    public class ExpenseMainViewModel : BaseViewModel
    {
        private RelayCommand _showExpenseGeneralFrameCommand;
        private RelayCommand _showExpensePriceFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showExpenseReportFrameCommand;

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

        public RelayCommand ShowExpensePriceFrameCommand
        {
            get
            {
                return _showExpensePriceFrameCommand ?? (_showExpensePriceFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = "/View/ExpensePricePage.xaml";
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
                    CurrentExpensePagePath = "/View/ReportPage.xaml";
                }));
            }
        }
    }
}
