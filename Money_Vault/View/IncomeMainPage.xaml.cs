using System.Windows.Controls;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeMainPage.xaml
    /// </summary>
    public partial class IncomeMainPage : Page
    {
        public IncomeMainPage()
        {
            InitializeComponent();

            DataContext = new IncomeMainViewModel();
        }
    }
}
