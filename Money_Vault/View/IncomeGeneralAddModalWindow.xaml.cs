using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeGeneralAddWindow.xaml
    /// </summary>
    public partial class IncomeGeneralAddModalWindow : Window, IClosable
    {
        public IncomeGeneralAddModalWindow()
        {
            InitializeComponent();

            DataContext = new IncomeGeneralAddViewModel();
        }
    }
}
