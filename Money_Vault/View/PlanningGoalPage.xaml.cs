using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningGoalPage.xaml
    /// </summary>
    public partial class PlanningGoalPage : Page
    {
        public PlanningGoalPage()
        {
            InitializeComponent();

            DataContext = new PlanningGoalViewModel();
        }
    }
}
