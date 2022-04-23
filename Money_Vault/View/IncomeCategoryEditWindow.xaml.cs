using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeCategoryEditWindow.xaml
    /// </summary>
    public partial class IncomeCategoryEditWindow : Window
    {
        public IncomeCategoryEditWindow()
        {
            InitializeComponent();

            DataContext = new IncomeCategoryEditViewModel();
        }
    }
}
