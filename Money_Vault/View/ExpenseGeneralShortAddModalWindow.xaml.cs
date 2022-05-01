using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralAddWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralShortAddModalWindow : Window, IClosable
    {
        public ExpenseGeneralShortAddModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralShortAddViewModel();
        }
    }
}
