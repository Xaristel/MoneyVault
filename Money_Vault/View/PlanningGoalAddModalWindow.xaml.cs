using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningGoalAddModalWindow.xaml
    /// </summary>
    public partial class PlanningGoalAddModalWindow : Window, IClosable
    {
        public PlanningGoalAddModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningGoalAddViewModel();
        }
    }
}
