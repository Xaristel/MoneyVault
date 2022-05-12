using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для PlanningNotePage.xaml
    /// </summary>
    public partial class PlanningNotePage : Page
    {
        public PlanningNotePage()
        {
            InitializeComponent();

            DataContext = new PlanningNoteViewModel();
        }
    }
}
