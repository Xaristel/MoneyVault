using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseStatisticPage.xaml
    /// </summary>
    public partial class ExpenseStatisticPage : Page
    {
        public ExpenseStatisticPage()
        {
            InitializeComponent();

            DataContext = new ExpenseStatisticViewModel();
        }
    }
}
