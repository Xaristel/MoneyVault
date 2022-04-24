using System.Windows;
using System.Windows.Data;
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

            Closed += (o, args) => BindableDialogResult = DialogResult;
            SetBinding(BindableDialogResultProperty, new Binding("Result"));

            DataContext = new MessageViewModel();
        }

        public bool? BindableDialogResult
        {
            get { return (bool?)GetValue(BindableDialogResultProperty); }
            set { SetValue(BindableDialogResultProperty, value); }
        }

        public static readonly DependencyProperty BindableDialogResultProperty =
            DependencyProperty.Register("BindableDialogResult", typeof(bool?), typeof(MessageModalWindow),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private void button_Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
