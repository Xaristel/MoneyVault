using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseShopEditWindow.xaml
    /// </summary>
    public partial class ExpenseShopEditModalWindow : Window, IClosable
    {
        public ExpenseShopEditModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseShopEditViewModel();
        }
    }
}
