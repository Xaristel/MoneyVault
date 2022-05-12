using Money_Vault.ViewModel;
using System.Windows;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningNoteEditModalWindow.xaml
    /// </summary>
    public partial class PlanningNoteEditModalWindow : Window, IClosable
    {
        public PlanningNoteEditModalWindow()
        {
            InitializeComponent();

            DataContext = new PlanningNoteEditViewModel();
        }
    }
}
