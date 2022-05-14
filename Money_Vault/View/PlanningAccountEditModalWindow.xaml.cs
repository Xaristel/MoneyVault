using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningAccountEditModalWindow.xaml
    /// </summary>
    public partial class PlanningAccountEditModalWindow : Window, IClosable
    {
        public PlanningAccountEditModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningAccountEditViewModel();
        }
    }
}
