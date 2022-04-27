using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeCategoryEditWindow.xaml
    /// </summary>
    public partial class CategoryEditWindow : Window
    {
        public CategoryEditWindow()
        {
            InitializeComponent();

            DataContext = new CategoryEditViewModel();
        }
    }
}
