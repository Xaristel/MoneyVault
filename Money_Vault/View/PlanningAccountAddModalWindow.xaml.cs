using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningAccountAddModalWindow.xaml
    /// </summary>
    public partial class PlanningAccountAddModalWindow : Window, IClosable
    {
        public PlanningAccountAddModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningAccountAddViewModel();
        }
    }
}
