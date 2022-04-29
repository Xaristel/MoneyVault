using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralAddWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralAddWindow : Window
    {
        public ExpenseGeneralAddWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralAddViewModel();
        }
    }
}
