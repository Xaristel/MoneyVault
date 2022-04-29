using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralPage.xaml
    /// </summary>
    public partial class ExpenseGeneralPage : Page
    {
        public ExpenseGeneralPage()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralViewModel();
        }
    }
}
