using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для ExpenseGeneralEditWindow.xaml
    /// </summary>
    public partial class ExpenseGeneralEditWindow : Window
    {
        public ExpenseGeneralEditWindow()
        {
            InitializeComponent();

            DataContext = new ExpenseGeneralEditViewModel();
        }
    }
}
