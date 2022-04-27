using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseMainPage.xaml
    /// </summary>
    public partial class ExpenseMainPage : Page
    {
        public ExpenseMainPage()
        {
            InitializeComponent();

            DataContext = new ExpenseMainViewModel();
        }
    }
}
