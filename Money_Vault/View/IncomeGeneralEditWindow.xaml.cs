using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeGeneralEditWindow.xaml
    /// </summary>
    public partial class IncomeGeneralEditWindow : Window
    {
        public IncomeGeneralEditWindow()
        {
            InitializeComponent();

            DataContext = new IncomeGeneralEditViewModel();
        }
    }
}
