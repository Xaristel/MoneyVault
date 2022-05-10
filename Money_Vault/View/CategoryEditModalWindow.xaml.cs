using System.Windows;
using Money_Vault.ViewModel;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для IncomeCategoryEditWindow.xaml
    /// </summary>
    public partial class CategoryEditModalWindow : Window, IClosable
    {
        public CategoryEditModalWindow()
        {
            InitializeComponent();

            DataContext = new CategoryEditViewModel();
        }
    }
}
