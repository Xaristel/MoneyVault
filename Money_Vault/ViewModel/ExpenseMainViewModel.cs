using Money_Vault.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Money_Vault.ViewModel
{
    public class ExpenseMainViewModel : BaseViewModel
    {
        private RelayCommand _showExpenseGeneralFrameCommand;
        private RelayCommand _showExpenseShopFrameCommand;
        private RelayCommand _showExpenseStatisticFrameCommand;
        private RelayCommand _showCategoryFrameCommand;
        private RelayCommand _showExpenseReportFrameCommand;

        private string _currentExpensePagePath;
        private bool _currentExpenseMode;

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
            "/View/ExpenseGeneralPage.xaml",
            "/View/ExpenseStatisticPage.xaml",
            "/View/ExpenseShopPage.xaml",
            "/View/CategoryPage.xaml",
            "/View/ReportPage.xaml"
        };

        public string CurrentExpensePagePath
        {
            get => _currentExpensePagePath;
            set
            {
                _currentExpensePagePath = value;
                OnPropertyChanged("CurrentExpensePagePath");

                AdditionalFunctions.SetColorForActiveButton(ButtonsColorList, _pagesPathList, CurrentExpensePagePath);
            }
        }

        public bool CurrentExpenseMode
        {
            get => _currentExpenseMode;
            set
            {
                _currentExpenseMode = value;
                OnPropertyChanged("CurrentExpenseMode");
            }
        }

        public ExpenseMainViewModel()
        {
            CurrentExpensePagePath = _pagesPathList[0];
            CurrentExpenseMode = Convert.ToBoolean(Settings.Default["currentExpenseMode"]);

            Settings.Default["isIncomePage"] = false;
            Settings.Default.Save();

            DispatcherTimer timer = new DispatcherTimer();

            timer.Tick += new EventHandler(UpdateCurrentExpenseMode);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void UpdateCurrentExpenseMode(object sender, EventArgs e)
        {
            CurrentExpenseMode = Convert.ToBoolean(Settings.Default["currentExpenseMode"]);
        }

        public RelayCommand ShowExpenseGeneralFrameCommand
        {
            get
            {
                return _showExpenseGeneralFrameCommand ?? (_showExpenseGeneralFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = _pagesPathList[0];
                }));
            }
        }

        public RelayCommand ShowExpenseStatisticFrameCommand
        {
            get
            {
                return _showExpenseStatisticFrameCommand ?? (_showExpenseStatisticFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = _pagesPathList[1];
                }));
            }
        }

        public RelayCommand ShowExpenseShopFrameCommand
        {
            get
            {
                return _showExpenseShopFrameCommand ?? (_showExpenseShopFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = _pagesPathList[2];
                }));
            }
        }

        public RelayCommand ShowCategoryFrameCommand
        {
            get
            {
                return _showCategoryFrameCommand ?? (_showCategoryFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = _pagesPathList[3];
                }));
            }
        }
        public RelayCommand ShowExpenseReportFrameCommand
        {
            get
            {
                return _showExpenseReportFrameCommand ?? (_showExpenseReportFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = _pagesPathList[4];
                }));
            }
        }
    }
}
