using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralFullInfoModalWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralFullInfoModalWindow : Window, IClosable
    {
        public ExpenseGeneralFullInfoModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralFullInfoViewModel();
        }
    }
}
