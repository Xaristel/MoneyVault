using System.Windows.Controls;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeGeneralPage.xaml
    /// </summary>
    public partial class IncomeGeneralPage : Page
    {
        public IncomeGeneralPage()
        {
            InitializeComponent();

            DataContext = new IncomeGeneralViewModel();
        }
    }
}
