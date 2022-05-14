using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningAccumulationPage.xaml
    /// </summary>
    public partial class PlanningAccountPage : Page
    {
        public PlanningAccountPage()
        {
            InitializeComponent();

            DataContext = new PlanningAccountViewModel();
        }
    }
}
