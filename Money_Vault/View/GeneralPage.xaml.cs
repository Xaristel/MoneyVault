using Money_Vault.ViewModel;
using System.Windows.Controls;
using System.Windows.Media;

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

        private void ColorTotalRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;

            if (e.Row.DataContext is TotalListItem data)
            {
                if (data.TypeName == "Итого")
                {
                    row.Background = Brushes.LightGray;
                }
                else
                {
                    row.Background = Brushes.White;
                }
            }
        }
    }
}
