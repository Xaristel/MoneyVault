using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeCategoryAddWindow.xaml
    /// </summary>
    public partial class CategoryAddWindow : Window
    {
        public CategoryAddWindow()
        {
            InitializeComponent();

            DataContext = new CategoryAddViewModel();
        }
    }
}
