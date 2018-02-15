using System;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using ViewModel.Authentication;
using ViewModel.Interfaces;

namespace TestWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            //Create a custom principal with an anonymous identity at startup
            CustomPrincipal customPrincipal = new CustomPrincipal();
            AppDomain.CurrentDomain.SetThreadPrincipal(customPrincipal);

            base.OnStartup(e);

            //Show the login view
            AuthenticationViewModel viewModel = new AuthenticationViewModel();
            IView loginWindow = new LoginWindow(viewModel);
            loginWindow.Show();

        }
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
