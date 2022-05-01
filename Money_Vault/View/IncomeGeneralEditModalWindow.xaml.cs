using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeGeneralEditWindow.xaml
    /// </summary>
    public partial class IncomeGeneralEditModalWindow : Window, IClosable
    {
        public IncomeGeneralEditModalWindow()
        {
            InitializeComponent();

            DataContext = new IncomeGeneralEditViewModel();
        }
    }
}
