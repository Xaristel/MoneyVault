using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralFullAddModalWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralFullAddModalWindow : Window, IClosable
    {
        public ExpenseGeneralFullAddModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralFullAddViewModel();
        }
    }
}
