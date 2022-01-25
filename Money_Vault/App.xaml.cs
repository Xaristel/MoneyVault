﻿using System.Windows;
using Money_Vault.ViewModel;
using Money_Vault.View;

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
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            authViewModel = new AuthViewModel();

            displayRootRegistry.ShowPresentation(authViewModel);

            //Shutdown();
        }
    }
}
