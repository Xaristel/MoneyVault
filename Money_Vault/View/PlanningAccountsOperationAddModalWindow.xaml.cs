using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningAccountsOperationAddModalWindow.xaml
    /// </summary>
    public partial class PlanningAccountsOperationAddModalWindow : Window, IClosable
    {
        public PlanningAccountsOperationAddModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningAccountsOperationAddViewModel();
        }
    }
}
