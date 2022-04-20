using Money_Vault.ViewModel;
using System.Windows.Controls;

namespace Money_Vault.View
{
    /// <summary>
    /// Логика взаимодействия для GeneralPage.xaml
    /// </summary>
    public partial class GeneralPage : Page
    {
        public GeneralPage()
        {
            InitializeComponent();

            DataContext = new GeneralViewModel();
        }

        private void ComboBox_Year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedItem = (sender as ComboBox).SelectedItem as string;

            if (selectedItem == "Все годы")
            {
                comboBox_Month.IsEnabled = false;
            }
            else
            {
                comboBox_Month.IsEnabled = true;
            }
        }
    }
}
