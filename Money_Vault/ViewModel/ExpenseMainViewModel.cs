using Money_Vault.Properties;
using System;
using System.Collections.Generic;
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

        private Brush _generalMenuItemColor;
        private Brush _shopMenuItemColor;
        private Brush _statisticMenuItemColor;
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

        public Brush ShopMenuItemColor
        {
            get => _shopMenuItemColor;
            set
            {
                _shopMenuItemColor = value;
                OnPropertyChanged("ShopMenuItemColor");
            }
        }

        public Brush StatisticMenuItemColor
        {
            get => _statisticMenuItemColor;
            set
            {
                _statisticMenuItemColor = value;
                OnPropertyChanged("StatisticMenuItemColor");
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

        public string CurrentExpensePagePath
        {
            get => _currentExpensePagePath;
            set
            {
                _currentExpensePagePath = value;
                OnPropertyChanged("CurrentExpensePagePath");

                SetColorForActiveMenuItem(CurrentExpensePagePath);
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
            CurrentExpensePagePath = "/View/ExpenseGeneralPage.xaml";
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
                    CurrentExpensePagePath = "/View/ExpenseGeneralPage.xaml";
                }));
            }
        }

        public RelayCommand ShowExpenseShopFrameCommand
        {
            get
            {
                return _showExpenseShopFrameCommand ?? (_showExpenseShopFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = "/View/ExpenseShopPage.xaml";
                }));
            }
        }

        public RelayCommand ShowExpenseStatisticFrameCommand
        {
            get
            {
                return _showExpenseStatisticFrameCommand ?? (_showExpenseStatisticFrameCommand = new RelayCommand((args) =>
                {
                    CurrentExpensePagePath = "/View/ExpenseStatisticPage.xaml";
                }));
            }
        }

        public RelayCommand ShowCategoryFrameCommand
        {
            get
            {
                return _showCategoryFrameCommand ?? (_showCategoryFrameCommand = new RelayCommand((args) =>
                {
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

        private void SetColorForActiveMenuItem(string path)
        {
            switch (path)
            {
                case "/View/ExpenseGeneralPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.DeepSkyBlue;
                        ShopMenuItemColor = Brushes.White;
                        StatisticMenuItemColor = Brushes.White;
                        CategoryMenuItemColor = Brushes.White;
                        ReportMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/ExpenseShopPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        ShopMenuItemColor = Brushes.DeepSkyBlue;
                        StatisticMenuItemColor = Brushes.White;
                        CategoryMenuItemColor = Brushes.White;
                        ReportMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/ExpenseStatisticPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        ShopMenuItemColor = Brushes.White;
                        StatisticMenuItemColor = Brushes.DeepSkyBlue;
                        CategoryMenuItemColor = Brushes.White;
                        ReportMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/CategoryPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        ShopMenuItemColor = Brushes.White;
                        StatisticMenuItemColor = Brushes.White;
                        CategoryMenuItemColor = Brushes.DeepSkyBlue;
                        ReportMenuItemColor = Brushes.White;
                        break;
                    }
                case "/View/ReportPage.xaml":
                    {
                        GeneralMenuItemColor = Brushes.White;
                        ShopMenuItemColor = Brushes.White;
                        StatisticMenuItemColor = Brushes.White;
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
