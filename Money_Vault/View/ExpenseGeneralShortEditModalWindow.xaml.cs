using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralEditWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralShortEditModalWindow : Window, IClosable
    {
        public ExpenseGeneralShortEditModalWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralShortEditViewModel();
        }
    }
}
