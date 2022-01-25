using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для RegistrationModalWindow.xaml
    /// </summary>
    public partial class RegistrationModalWindow : Window
    {
        public RegistrationModalWindow()
        {
            InitializeComponent();

            DataContext = new RegistrationViewModel();
        }
    }
}
