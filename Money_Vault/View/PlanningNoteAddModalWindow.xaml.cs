using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningNoteAddModalWindow.xaml
    /// </summary>
    public partial class PlanningNoteAddModalWindow : Window, IClosable
    {
        public PlanningNoteAddModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningNoteAddViewModel();
        }
    }
}
