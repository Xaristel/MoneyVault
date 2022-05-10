using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralFullEditModalWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralFullEditModalWindow : Window, IClosable
    {
        public ExpenseGeneralFullEditModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralFullEditViewModel();
        }
    }
}
