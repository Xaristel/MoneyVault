using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseShopAddWindow.xaml
    /// </summary>
    public partial class ExpenseShopAddModalWindow : Window, IClosable
    {
        public ExpenseShopAddModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseShopAddViewModel();
        }
    }
}
