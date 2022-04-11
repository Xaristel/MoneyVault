using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для MessageModalWindow.xaml
    /// </summary>
    public partial class MessageModalWindow : Window
    {
        public MessageModalWindow()
        {
            InitializeComponent();

            DataContext = new MessageViewModel();
        }
    }
}
