using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpensePricePage.xaml
    /// </summary>
    public partial class ExpenseShopPage : Page
    {
        public ExpenseShopPage()
        {
            InitializeComponent();

            DataContext = new ExpenseShopViewModel();
        }
    }
}
