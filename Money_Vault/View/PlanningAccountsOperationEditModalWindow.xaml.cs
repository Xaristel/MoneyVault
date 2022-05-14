using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningAccountsOperationEditModalWindow.xaml
    /// </summary>
    public partial class PlanningAccountsOperationEditModalWindow : Window, IClosable
    {
        public PlanningAccountsOperationEditModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningAccountsOperationEditViewModel();
        }
    }
}
