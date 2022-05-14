using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningAccumulationPage.xaml
    /// </summary>
    public partial class PlanningAccumulationPage : Page
    {
        public PlanningAccumulationPage()
        {
            InitializeComponent();

            DataContext = new PlanningAccumulationViewModel();
        }
    }
}
