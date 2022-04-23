using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeGeneralAddWindow.xaml
    /// </summary>
    public partial class IncomeGeneralAddWindow : Window
    {
        public IncomeGeneralAddWindow()
        {
            InitializeComponent();

            DataContext = new IncomeGeneralAddViewModel();
        }
    }
}
