using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningMainPage.xaml
    /// </summary>
    public partial class PlanningMainPage : Page
    {
        public PlanningMainPage()
        {
            InitializeComponent();

            DataContext = new PlanningMainViewModel();
        }
    }
}
