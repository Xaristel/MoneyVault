using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningGoalEditModalWindow.xaml
    /// </summary>
    public partial class PlanningGoalEditModalWindow : Window, IClosable
    {
        public PlanningGoalEditModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningGoalEditViewModel();
        }
    }
}
