using System.Windows;
using Money_Vault.ViewModel;
using Money_Vault.View;
using System.Threading;

namespace Money_Vault
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public DisplayRootRegistry displayRootRegistry = new DisplayRootRegistry();
        private AuthViewModel authViewModel;

        public App()
        {
            displayRootRegistry.RegisterWindowType<AuthViewModel, AuthWindow>();
            displayRootRegistry.RegisterWindowType<MainViewModel, MainWindow>();
            displayRootRegistry.RegisterWindowType<RegistrationViewModel, RegistrationModalWindow>();
            displayRootRegistry.RegisterWindowType<MessageViewModel, MessageModalWindow>();
            displayRootRegistry.RegisterWindowType<IncomeGeneralAddViewModel, IncomeGeneralAddModalWindow>();
            displayRootRegistry.RegisterWindowType<IncomeGeneralEditViewModel, IncomeGeneralEditModalWindow>();
            displayRootRegistry.RegisterWindowType<CategoryAddViewModel, CategoryAddModalWindow>();
            displayRootRegistry.RegisterWindowType<CategoryEditViewModel, CategoryEditModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseGeneralShortAddViewModel, ExpenseGeneralShortAddModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseGeneralShortEditViewModel, ExpenseGeneralShortEditModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseGeneralFullInfoViewModel, ExpenseGeneralFullInfoModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseGeneralFullAddViewModel, ExpenseGeneralFullAddModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseGeneralFullEditViewModel, ExpenseGeneralFullEditModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseShopAddViewModel, ExpenseShopAddModalWindow>();
            displayRootRegistry.RegisterWindowType<ExpenseShopEditViewModel, ExpenseShopEditModalWindow>();
            displayRootRegistry.RegisterWindowType<PlanningNoteAddViewModel, PlanningNoteAddModalWindow>();
            displayRootRegistry.RegisterWindowType<PlanningNoteEditViewModel, PlanningNoteEditModalWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            authViewModel = new AuthViewModel();

            displayRootRegistry.ShowPresentation(authViewModel);
        }
    }
}
